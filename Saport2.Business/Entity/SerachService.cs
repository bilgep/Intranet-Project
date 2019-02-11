using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXP = Saport2.Shared.Exceptions;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System.Text.RegularExpressions;

namespace Saport2.Business.Entity
{
    public class SearchService : SearchModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        public static List<SearchModel> QuerySearchResults(string siteUrl, string queryText, string contentTypeId, int contentTypeCode, string contentTypeName, string redirectUrl)
        {
            try
            {

                List<SearchModel> searchResults = new List<SearchModel>();

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(Data.DataStatics.UserNameForService, Data.DataStatics.PasswordForService, Data.DataStatics.DomainForService);


                ClientContext clientContext = new ClientContext(siteUrl);
                clientContext.Credentials = credentials;

                KeywordQuery keywordQuery = new KeywordQuery(clientContext);
                keywordQuery.QueryText = queryText;
                keywordQuery.SortList.Add("Created", SortDirection.Descending);
                string filter1 = "contenttypeid:equals(\"{0}\")";
                keywordQuery.RefinementFilters.Add(string.Format(filter1, contentTypeId));
                keywordQuery.SelectProperties.Add("ContentTypeId");
                keywordQuery.SelectProperties.Add("Title");
                keywordQuery.SelectProperties.Add("ListItemID");
                keywordQuery.RowLimit = 10; 
                keywordQuery.ClientType = "ContentSearchRegular";

                SearchExecutor searchExecutor = new SearchExecutor(clientContext);
                ClientResult<ResultTableCollection> results = searchExecutor.ExecuteQuery(keywordQuery);
                clientContext.ExecuteQuery();

                IEnumerable<IDictionary<string,object>> resultTable = results.Value[0].ResultRows;

                if (resultTable != null)
                {
                    foreach (var resultRow in resultTable)
                    {
                        SearchModel newSearchItem = new SearchModel();
                        newSearchItem.Title = resultRow["Title"].ToString();
                        newSearchItem.ListItemId = resultRow["ListItemID"].ToString();
                        newSearchItem.ContentTypeId = resultRow["ContentTypeId"].ToString();
                        newSearchItem.ContentTypeName = contentTypeName;
                        newSearchItem.ContentTypeCode = contentTypeCode;
                        newSearchItem.RedirectUrl = redirectUrl;
                        searchResults.Add(newSearchItem);
                    }
                }

                clientContext.Dispose();

                return searchResults;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        //public static string GenerateSearchResultHtml(List<SearchModel> searchResults)
        //{
        //    try
        //    {
                
        //        string resultsHtmlString = string.Empty;

        //        foreach (var item in searchResults)
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        EXP.RedirectToErrorPage(ex.Message);
        //        return null;
        //    }
        //}
        #endregion
    }
}
