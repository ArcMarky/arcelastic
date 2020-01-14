using EmployeeRecordKeeping.ViewModel;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordKeeping.BLL.Interfaces
{
    public interface IElasticSearchIndexerService
    {
        Task<ResponseObject> CreateIndex(dynamic model, string index);
        Task<ResponseObject> RetrieveResults(ElasticSearchConfigDto model, int indexToSearch);
        Task<ResponseObject> EmployeeElasticQueries(ElasticSearchConfigDto model);
        ResponseObject TransmuteEmployeeElasticResults(ISearchResponse<EmployeeElasticDto> model);
        ResponseObject TransmuteAndHighlightEmployeeElasticResults(ISearchResponse<EmployeeElasticDto> model);
        Task<ResponseObject> GetAllElasticEmployee(string query);
        Task<ResponseObject> GetSmartSearchElasticEmployee(string query);
        Task<ResponseObject> GetHighlightedElasticEmployee(string query);
        Task<ResponseObject> GetMustNotMatchElasticEmployee(string query);
    }
}
