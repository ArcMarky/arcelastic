using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRecordKeeping.BLL.Interfaces;
using EmployeeRecordKeeping.ViewModel;
using Microsoft.AspNetCore.Mvc;
using static EmployeeRecordKeeping.Utilities.Enums;

namespace EmployeeRecordKeeping.Controllers
{
    /**********************************************************************
    * THIS APPLICATION IS A PROOF OF CONCEPT ONLY. THIS WILL NEVER BE DEVELOPED IN PRODUCTION.
    * Author:        Mark Yu
    * Description:   Employee Management API
    *                Commented methods are unused.            
    * History:
    *   01-07-2020  MarkYu    Created. 
    ***********************************************************************/
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IElasticSearchIndexerService _elasticSearchIndexerService;
        public EmployeeController(IEmployeeService employeeService, IElasticSearchIndexerService elasticSearchIndexerService)
        {
            _employeeService = employeeService;
            _elasticSearchIndexerService = elasticSearchIndexerService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseObject> GetEmployeeList()
        {
            return await _employeeService.GetAllEmployeesAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject> SaveEmployee([FromBody]EmployeeDto model)
        {
            return await _employeeService.SaveEmployee(model);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseObject> SearchIndexedEmployees([FromBody]ElasticSearchConfigDto model)
        {
            return await _elasticSearchIndexerService.RetrieveResults(model, (int)IndicesValue.Employee);
        }
    }

}