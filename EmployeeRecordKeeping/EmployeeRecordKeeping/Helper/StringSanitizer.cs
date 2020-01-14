using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.Helper
{
    public class StringSanitizer
    {
        public static string ToLowerAndTrim(string model)
        {
            if (!string.IsNullOrEmpty(model))
            {
                return model.Trim().ToLower();
            }
            return "";
        }
    }
}
