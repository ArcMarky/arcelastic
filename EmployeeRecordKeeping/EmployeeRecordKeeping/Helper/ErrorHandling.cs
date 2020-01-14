using EmployeeRecordKeeping.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.Helper
{
    public class ErrorHandling
    {
        public static ResponseObject LogError(System.Exception e)
        {
            ResponseObject response = new ResponseObject(ResponseType.Error, e.Message);
            //for future development of logging
            return response;
        }
        public static ResponseObject LogCustomError(string errorMessage)
        {
            ResponseObject response = new ResponseObject(ResponseType.Error, errorMessage);
            return response;
        }
    }
}
