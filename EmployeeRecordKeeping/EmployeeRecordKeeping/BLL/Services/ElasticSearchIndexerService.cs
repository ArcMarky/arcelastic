using AutoMapper;
using EmployeeRecordKeeping.BLL.Interfaces;
using EmployeeRecordKeeping.Configuration;
using EmployeeRecordKeeping.DAL.Interfaces;
using EmployeeRecordKeeping.Helper;
using EmployeeRecordKeeping.Models;
using EmployeeRecordKeeping.ViewModel;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static EmployeeRecordKeeping.Utilities.Enums;

namespace EmployeeRecordKeeping.BLL.Services
{
    public class ElasticSearchIndexerService : IElasticSearchIndexerService
    {
        public static Uri EsNode;
        public static ConnectionSettings EsConfig;
        public static ElasticClient EsClient;

        public ElasticSearchIndexerService()
        {
            EsNode = new Uri(Hosts.DefaultActiveHost);
            EsConfig = new ConnectionSettings(EsNode);
        }

        #region Indexing Related
        /// <summary>
        /// Creates an index for the employee record that is being inserted. Pass also the string index that will be used.
        /// </summary>
        /// <param name="model">data that was inserted in the database</param>
        /// <param name="index">index that will be used</param>
        /// <returns>response object</returns>
        public async Task<ResponseObject> CreateIndex(dynamic model, string index)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                EsConfig.DefaultIndex(index);
                EsClient = new ElasticClient(EsConfig);
                var createIndexResponse = await EsClient.IndexDocumentAsync(model);
                if (!createIndexResponse.IsValid)
                {
                    response = ErrorHandling.LogCustomError(ResponseMessage.EmployeeIndexError);
                }
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        #endregion

        #region Search Related 
        /// <summary>
        /// Handles all the queries from the front end
        /// </summary>
        /// <param name="model">filters</param>
        /// <param name="indexToSearch">Refer to the enums model for proper index value</param>
        /// <returns>records from the search engine</returns>
        public async Task<ResponseObject> RetrieveResults(ElasticSearchConfigDto model, int indexToSearch)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                switch (indexToSearch)
                {
                    case 1: //employee
                        {
                            var fetchData = await EmployeeElasticQueries(model);
                            if (fetchData.IsSuccess)
                            {
                                response.Data = fetchData.Data;
                            }
                            else
                            {
                                response = new ResponseObject(ResponseType.Error, ResponseMessage.SearchError);
                            }
                        }
                        break;

                }
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        /// <summary>
        /// Search the employee index (employee table if sql)
        /// </summary>
        /// <param name="model">filters</param>
        /// <returns>response object</returns>
        public async Task<ResponseObject> EmployeeElasticQueries(ElasticSearchConfigDto model)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                EsConfig.DefaultIndex(IndicesConfig.Employee);
                EsClient = new ElasticClient(EsConfig);
                var query = model.SearchTerm.ToLower();

                if (model.FilterTypeId == 0) //all
                {
                    var searchResults = await GetAllElasticEmployee(query);
                    if (searchResults.IsSuccess)
                    {
                        response.Data = searchResults.Data;
                    }
                }
                else if (model.FilterTypeId == 2) //smart search
                {
                    var searchResults = await GetSmartSearchElasticEmployee(query);
                    if (searchResults.IsSuccess)
                    {
                        response.Data = searchResults.Data;
                    }
                }
                else if (model.FilterTypeId == 3) //highlight
                {
                    var searchResults = await GetHighlightedElasticEmployee(query);
                    if (searchResults.IsSuccess)
                    {
                        response.Data = searchResults.Data;
                    }
                }
                else if (model.FilterTypeId == 4) //must not match
                {
                    var searchResults = await GetMustNotMatchElasticEmployee(query);
                    if (searchResults.IsSuccess)
                    {
                        response.Data = searchResults.Data;
                    }
                }
                //else if (model.FilterTypeId == 5)
                //{
                //    var searchResults = await GetLanguageSearchElasticEmployee(query);
                //    if (searchResults.IsSuccess)
                //    {
                //        response.Data = searchResults.Data;
                //    }
                //}
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        /// <summary>
        /// Translates the search response from NEST and converts it into a view model
        /// </summary>
        /// <param name="model">search response</param>
        /// <returns>transmuted data</returns>
        public ResponseObject TransmuteEmployeeElasticResults(ISearchResponse<EmployeeElasticDto> model)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                var results = new ElasticSearchResultsSummary();
                if (model.Hits.Any())
                {
                    List<ElasticSearchResults> elasticConsolidatedResults = new List<ElasticSearchResults>();
                    results.SearchDuration = model.Took.ToString();
                    results.Hits = model.Documents.Count();
                    foreach (var item in model.Hits)
                    {
                        ElasticSearchResults modelToAppend = new ElasticSearchResults();
                        modelToAppend.Id = item.Id;
                        modelToAppend.Index = item.Index;
                        modelToAppend.Score = item.Score.ToString();
                        modelToAppend.StringSearchResults = item.Source;
                        elasticConsolidatedResults.Add(modelToAppend);
                    }
                    results.SearchOutPut = elasticConsolidatedResults;
                    response.Data = results;
                }
                else
                {
                    response.Data = results;
                }
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        /// <summary>
        /// Maps the key value into a view model from the search response. the highlighted characters are with the html tags <b></b>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseObject TransmuteAndHighlightEmployeeElasticResults(ISearchResponse<EmployeeElasticDto> model)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                var results = new ElasticSearchResultsSummary();
                if (model.Hits.Any())
                {
                    List<ElasticSearchResults> elasticConsolidatedResults = new List<ElasticSearchResults>();
                    results.SearchDuration = model.Took.ToString();
                    results.Hits = model.Documents.Count();
                    foreach (var item in model.Hits)
                    {
                        ElasticSearchResults modelToAppend = new ElasticSearchResults();
                        modelToAppend.Id = item.Id;
                        modelToAppend.Index = item.Index;
                        modelToAppend.Score = item.Score.ToString();
                        var mappedHighlightedData = new EmployeeElasticDto();
                        if (item.Highlight.Any())
                        {
                            foreach (var x in item.Highlight)
                            {
                                if (x.Key.Equals("address"))
                                {
                                    mappedHighlightedData.Address = x.Value.FirstOrDefault();
                                }
                                else if (x.Key.Equals("notes"))
                                {
                                    mappedHighlightedData.Notes = x.Value.FirstOrDefault();
                                }
                                else if (x.Key.Equals("fullName"))
                                {
                                    mappedHighlightedData.FullName = x.Value.FirstOrDefault();
                                }
                                else if (x.Key.Equals("position"))
                                {
                                    mappedHighlightedData.Position = x.Value.FirstOrDefault();
                                }
                                else if (x.Key.Equals("department"))
                                {
                                    mappedHighlightedData.Department = x.Value.FirstOrDefault();
                                }
                                else if (x.Key.Equals("age"))
                                {
                                    mappedHighlightedData.Age = x.Value.FirstOrDefault();
                                }
                            }
                            modelToAppend.StringSearchResults = mappedHighlightedData;
                        }
                        elasticConsolidatedResults.Add(modelToAppend);
                    }
                    results.SearchOutPut = elasticConsolidatedResults;
                    response.Data = results;
                }
                else
                {
                    response.Data = results;
                }
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        /// <summary>
        /// Fetches all records that matches the query
        /// </summary>
        /// <param name="query">search term inputted by the user</param>
        /// <returns>response data</returns>
        public async Task<ResponseObject> GetAllElasticEmployee(string query)
        {
            var response = new ResponseObject(ResponseType.Success,string.Empty);
            try
            {
                var searchResults = await EsClient.SearchAsync<EmployeeElasticDto>(s => s
                                   .From(0)
                                   .Size(10000)
                                   .Index(IndicesConfig.Employee)
                                   .Query(q => q
                                   .Bool(b => b
                                   .Should(
                                   bs => bs.Match(p => p.Field(f => f.FullName).Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Address).Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Notes).Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Age).Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Department).Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Position).Query(query))))));
                response.Data = TransmuteEmployeeElasticResults(searchResults).Data;
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        /// <summary>
        /// Fetches all records that matches the query in a smart manner based on the analyzer configured.
        /// </summary>
        /// <param name="query">search term inputted by the user</param>
        /// <returns>response data</returns>
        public async Task<ResponseObject> GetSmartSearchElasticEmployee(string query)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                var searchResults = await EsClient.SearchAsync<EmployeeElasticDto>(s => s
                                   .From(0)
                                   .Size(10000)
                                   .Index(IndicesConfig.Employee)
                                   .Query(q => q
                                   .Bool(b => b
                                   .Should(
                                   bs => bs.Match(p => p.Field(f => f.FullName).Analyzer("my_synonyms").Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Address).Analyzer("my_synonyms").Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Notes).Analyzer("my_synonyms").Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Age).Analyzer("my_synonyms").Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Department).Analyzer("my_synonyms").Query(query)),
                                   bs => bs.Match(p => p.Field(f => f.Position).Analyzer("my_synonyms").Query(query))))));
                response.Data = TransmuteEmployeeElasticResults(searchResults).Data;
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        /// <summary>
        /// Fetches all records that matches the query and highlights the full text that it matches
        /// modify the pre tags and post tags to change the html element that renders the highlighted data.
        /// </summary>
        /// <param name="query">search term inputted by the user</param>
        /// <returns>response data</returns>
        public async Task<ResponseObject> GetHighlightedElasticEmployee(string query)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                var searchResults = await EsClient.SearchAsync<EmployeeElasticDto>(s => s
                                    .From(0)
                                    .Size(10000)
                                    .Index(IndicesConfig.Employee)
                                    .Query(q => q
                                    .Bool(b => b
                                    .Should(
                                    bs => bs.Match(p => p.Field(f => f.FullName).Query(query)),
                                    bs => bs.Match(p => p.Field(f => f.Address).Query(query)),
                                    bs => bs.Match(p => p.Field(f => f.Notes).Query(query)),
                                    bs => bs.Match(p => p.Field(f => f.Age).Query(query)),
                                    bs => bs.Match(p => p.Field(f => f.Department).Query(query)),
                                    bs => bs.Match(p => p.Field(f => f.Position).Query(query)
                                    ))))
                                    .Highlight(x =>
                                    x.PreTags("<b>")
                                    .PostTags("</b>")
                                    .Fields(
                                        fs =>
                                            fs.Field(y => y.Address)
                                            .Type("plain")
                                            .FragmentSize(150)
                                            .ForceSource()
                                            .NumberOfFragments(3)
                                            .Fragmenter(HighlighterFragmenter.Span)
                                            .NoMatchSize(150)
                                            .HighlightQuery(m => m
                                            .Match(b => b
                                            .Field(k => k.Address)
                                            .Query(query))),
                                       fs =>
                                            fs.Field(y => y.Age)
                                            .Type("plain")
                                            .FragmentSize(150)
                                            .ForceSource()
                                            .NumberOfFragments(3)
                                            .Fragmenter(HighlighterFragmenter.Span)
                                            .NoMatchSize(150)
                                            .HighlightQuery(m => m
                                            .Match(b => b
                                            .Field(k => k.Age)
                                            .Query(query))),
                                       fs =>
                                          fs.Field(y => y.Department)
                                            .Type("plain")
                                            .FragmentSize(150)
                                            .ForceSource()
                                            .NumberOfFragments(3)
                                            .Fragmenter(HighlighterFragmenter.Span)
                                            .NoMatchSize(150)
                                            .HighlightQuery(m => m
                                            .Match(b => b
                                            .Field(k => k.Department)
                                            .Query(query))),
                                       fs =>
                                           fs.Field(y => y.FullName)
                                            .Type("plain")
                                            .FragmentSize(150)
                                            .ForceSource()
                                            .NumberOfFragments(3)
                                            .Fragmenter(HighlighterFragmenter.Span)
                                            .NoMatchSize(150)
                                            .HighlightQuery(m => m
                                            .Match(b => b
                                            .Field(k => k.FullName)
                                            .Query(query))),
                                       fs =>
                                           fs.Field(y => y.Position)
                                            .Type("plain")
                                            .FragmentSize(150)
                                            .ForceSource()
                                            .NumberOfFragments(3)
                                            .Fragmenter(HighlighterFragmenter.Span)
                                            .NoMatchSize(150)
                                            .HighlightQuery(m => m
                                            .Match(b => b
                                            .Field(k => k.Position)
                                            .Query(query)))
                                    )));
                response.Data = TransmuteAndHighlightEmployeeElasticResults(searchResults).Data;
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
        /// <summary>
        /// Fetches all records and excludes the texts which contains the search term
        /// </summary>
        /// <param name="query">search term inputted by the user</param>
        /// <returns>response data</returns>
        public async Task<ResponseObject> GetMustNotMatchElasticEmployee(string query)
        {
            var response = new ResponseObject(ResponseType.Success, string.Empty);
            try
            {
                var searchResults = await EsClient.SearchAsync<EmployeeElasticDto>(s => s
                                  .From(0)
                                  .Size(10000)
                                  .Index(IndicesConfig.Employee)
                                  .Query(q => q
                                  .Bool(b => b
                                  .MustNot(
                                  bs => bs.Match(p => p.Field(f => f.FullName).Query(query)),
                                  bs => bs.Match(p => p.Field(f => f.Address).Query(query)),
                                  bs => bs.Match(p => p.Field(f => f.Notes).Query(query)),
                                  bs => bs.Match(p => p.Field(f => f.Age).Query(query)),
                                  bs => bs.Match(p => p.Field(f => f.Department).Query(query)),
                                  bs => bs.Match(p => p.Field(f => f.Position).Query(query)
                                  )))));
                response.Data = TransmuteEmployeeElasticResults(searchResults).Data;
            }
            catch (Exception e)
            {
                response = ErrorHandling.LogError(e);
            }
            return response;
        }
       
        #endregion
    }
}
