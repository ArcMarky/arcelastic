using System;
using System.Collections.Generic;

namespace EmployeeRecordKeeping.Models
{
    public partial class Employee
    {
        public int Employeeid { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public int? Age { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    }
}
