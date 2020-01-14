using EmployeeRecordKeeping.DAL.Interfaces;
using EmployeeRecordKeeping.Models;
using EmployeeRecordKeeping.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly rksContext _context;

        public EmployeeRepository(rksContext context)
        {
            _context = context;
        }
        #region Employee Related

        /// <summary>
        /// Retrieves all records of all the employees in an IQueryable state
        /// </summary>
        /// <returns>queryable employee data</returns>
        public IQueryable<Employee> GetAllEmployees()
        {
            return _context.Employee;
        }

        /// <summary>
        /// Saves a single data of an employee in the schema
        /// </summary>
        /// <param name="model">Employee model</param>
        /// <returns></returns>
        public async Task<int> CreateEmployeeAsync(Employee model)
        {
            _context.Employee.Add(model);
            return await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Updates the data of the employee based on the changes that were made by the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> UpdateEmployeeAsync(Employee model)
        {
            _context.Employee.Update(model);
            return await _context.SaveChangesAsync();
        }

        #endregion
    }
}
