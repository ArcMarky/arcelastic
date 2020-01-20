using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.ViewModel
{
    public class ElasticSearchConfigDto
    {
        //public bool IsExactWords { get; set; }
        //public bool IsSmartSearch { get; set; }
        //public bool IsHighlightResults { get; set; }
        //public bool DoesNotContain { get; set; }
        public string SearchTerm { get; set; }
        public int FilterTypeId { get; set; }
        //0 = all, 2 = smart search , 3 = highlight match keywords, 4 = must not match, 5 = language search
    }
    public class ElasticSearchResultsSummary
    {
        public IEnumerable<ElasticSearchResults> SearchOutPut { get; set; }
        public int Hits { get; set; }
        public string SearchDuration { get; set; }
    }
    public class ElasticSearchResults
    {
        public dynamic StringSearchResults { get; set; }
        public string Id { get; set; }
        public string Index { get; set; }
        public string Score { get; set; }
    }
}
