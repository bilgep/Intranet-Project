using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.UI
{
    public partial class NewsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    #region Load News details
                    int? newsId = Convert.ToInt32(Request.QueryString["NewsId"]);
                    if (newsId.HasValue && newsId > 0)
                    {
                        NewsModel.News thisNews = NewsService.QueryNewsDetails((int)newsId);
                        ltrModified.Text = Shared.Helpers.GetDateTurkishCulture(thisNews.Modified);
                        ltrTitle.Text = thisNews.Title;
                        ltrPageContent.Text = thisNews.PublishingPageContent;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

        }
    }
}