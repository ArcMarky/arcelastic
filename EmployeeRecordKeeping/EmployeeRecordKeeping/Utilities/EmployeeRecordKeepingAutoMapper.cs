using AutoMapper;
using EmployeeRecordKeeping.Models;
using EmployeeRecordKeeping.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.Utilities
{
    public class EmployeeRecordKeepingAutoMapper : Profile
    {
        public EmployeeRecordKeepingAutoMapper()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<Employee, EmployeeElasticDto>();
            CreateMap<EmployeeElasticDto, Employee>();
        }
    }
}
