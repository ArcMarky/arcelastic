using EmployeeRecordKeeping.Models;
using EmployeeRecordKeeping.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.BLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<ResponseObject> GetAllEmployeesAsync();
        Task<ResponseObject> SaveEmployee(EmployeeDto model);
        Task<ResponseObject> MapAndIndexEmployee(Employee model);
    }
}
