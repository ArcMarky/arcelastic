using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.ViewModel
{
    public class ResponseMessage
    {
        public const string EmployeeSaveSuccess = "Employee has been added successfully.";
        public const string EmployeeIndexError = "Something went wrong while creating the index. Try again later.";

        //general message for exception error
        public const string SaveError = "Something went wrong while saving the data, Please try again later.";
        public const string SearchError = "Something went wrong while retrieving results. Please try again later.";
    }
}
