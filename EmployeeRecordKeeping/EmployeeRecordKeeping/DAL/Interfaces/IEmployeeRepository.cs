using EmployeeRecordKeeping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> GetAllEmployees();
        Task<int> CreateEmployeeAsync(Employee model);
        Task<int> UpdateEmployeeAsync(Employee model);
    }
}
