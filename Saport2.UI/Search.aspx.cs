using Microsoft.SharePoint.Client;
using Saport2.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.UI
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region SearchBox
                if (Request.QueryString["k"] != null) Session.Add("searchValue", Request.QueryString["k"].ToString());
                #endregion

                // Retrieve Query String
                string searchText = Request.QueryString["k"];

                // Execute Query
                if (!string.IsNullOrEmpty(searchText))
                {
                    List<SearchModel> searchResults = new List<SearchModel>();

                    foreach (var item in SearchModel.contentTypeIds)
                    {
                        string contentTypeIdSplited = item.Split(';')[0].ToString();
                        int contentTypeCode = Convert.ToInt32(item.Split(';')[1]);
                        string contentTypeName = item.Split(';')[2].ToString();
                        string redirectUrl = item.Split(';')[3].ToString();
                        searchResults.AddRange(SearchService.QuerySearchResults(Data.DataStatics.saportHostURL, searchText, contentTypeIdSplited, contentTypeCode,contentTypeName, redirectUrl));
                    }

                    string htmlString = string.Empty;

                    foreach (SearchModel srcResult in searchResults)
                    {
                        if (!string.IsNullOrEmpty( srcResult.ListItemId))
                        {
                            htmlString += string.Format(SearchModel.resultItemHtmlTemplate, srcResult.RedirectUrlComplete, srcResult.Title);
                        }

                    }

                    ltrResult.Text = !string.IsNullOrEmpty(htmlString) ? htmlString: "Sonuç bulunamadı.";
                }
                else
                {
                    ltrResult.Text = "Sonuç Bulunamadı";
                }

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }
    }
}