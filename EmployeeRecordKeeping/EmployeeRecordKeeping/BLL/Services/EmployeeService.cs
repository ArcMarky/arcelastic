using AutoMapper;
using EmployeeRecordKeeping.BLL.Interfaces;
using EmployeeRecordKeeping.DAL.Interfaces;
using EmployeeRecordKeeping.Helper;
using EmployeeRecordKeeping.Models;
using EmployeeRecordKeeping.ViewModel;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EmployeeRecordKeeping.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IElasticSearchIndexerService _elasticSearchIndexerService;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IElasticSearchIndexerService elasticSearchIndexerService)
        {
            _employeeRepository = employeeRepository;
            _elasticSearchIndexerService = elasticSearchIndexerService;
            _mapper = mapper;
        }
        #region Employee Management Related

        /// <summary>
        /// Retrieves all records of all the employees available in the schema
        /// </summary>
        /// <returns>Projected list of all employees</returns>
        public async Task<ResponseObject> GetAllEmployeesAsync()
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                var data = await _employeeRepository.GetAllEmployees().AsNoTracking().ToListAsync();
                response.Data = _mapper.Map<List<EmployeeDto>>(data);
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }

        /// <summary>
        /// Saves the employee record, if the employee id has values then it would proceed with the 
        /// update, else it would execute the insert method.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseObject> SaveEmployee(EmployeeDto model)
        {
            var response = new ResponseObject(ResponseType.Success, ResponseMessage.EmployeeSaveSuccess);
            try
            {
                //Defer for future development if more than 1 table insertion
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //update
                    var entity = await _employeeRepository.GetAllEmployees().FirstOrDefaultAsync(x => x.Employeeid == model.Employeeid);
                    if (entity != null)
                    {
                        entity.FullName = model.FullName;
                        entity.Address = model.Address;
                        entity.Notes = model.Notes;
                        entity.Age = model.Age;
                        entity.Position = model.Position;
                        entity.Department = model.Department;
                        await _employeeRepository.UpdateEmployeeAsync(entity);
                    }
                    //create
                    else 
                    {
                        var entityToInsert = _mapper.Map<Employee>(model);
                        await _employeeRepository.CreateEmployeeAsync(entityToInsert);

                     
                        var createIndexResponse = await MapAndIndexEmployee(entityToInsert);
                        if (!createIndexResponse.IsSuccess)
                        {
                            response = createIndexResponse;
                            scope.Dispose();
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
                response.Message = ResponseMessage.SaveError;
            }
            return response;
        }

        /// <summary>
        /// Maps the entity into a indexable object
        /// </summary>
        /// <param name="model">model to map</param>
        /// <returns>response object</returns>
        public async Task<ResponseObject> MapAndIndexEmployee(Employee model)
        {
            var response = new ResponseObject(ResponseType.Success,string.Empty);
            try
            {
                var entityToIndex = _mapper.Map<EmployeeElasticDto>(model);
                entityToIndex.Id = model.Employeeid;
                var createIndexResponse = await _elasticSearchIndexerService.CreateIndex(entityToIndex, IndicesConfig.Employee);
                if (!createIndexResponse.IsSuccess)
                {
                    response = createIndexResponse;
                }
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
                response.Message = ResponseMessage.SaveError;
            }
            return response;
        }

        #endregion
    }
}
