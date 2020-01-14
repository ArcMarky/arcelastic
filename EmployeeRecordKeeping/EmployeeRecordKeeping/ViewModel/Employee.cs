using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.ViewModel
{
    public class EmployeeDto
    {
        public int Employeeid { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public int? Age { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    }
    public class EmployeeElasticDto
    {
        public int Id { get; set; }
        public string Employeeid { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string Age { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    }

  
    

}
