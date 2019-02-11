using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business;
using Saport2.Business.Entity;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.UI
{
    public partial class News : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    #region Load News
                    List<NewsModel.News> newss = NewsService.DeserializeXMLToNewsLight(NewsModel.newsSaveFolder, NewsModel.newsSaveFileName, 10);
                    foreach (var news in newss)
                    {
                        ltrMain.Text += string.Format(NewsModel.newsPageHtml, news.ListImage, news.RedirectPage, news.Title, Shared.Helpers.GetDateTurkishCulture( Convert.ToDateTime(news.Created)), news.RedirectPage);
                    }
                    #endregion
                    ViewState.Add("LastNewsId", newss[newss.Count - 1].ID.ToString());

                }

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int lastNewsId = Convert.ToInt32(ViewState["LastNewsId"]);

                List<NewsModel.News> sessionNews = NewsService.DeserializeXMLToNewsLight(NewsModel.newsSaveFolder, NewsModel.newsSaveFileName, 50); 

                if (sessionNews != null)
                {
                    var lastNews = sessionNews.Where(x => x.ID == lastNewsId).ToList();
                    int index = sessionNews.IndexOf((NewsModel.News)lastNews[0]);

                    if (index < sessionNews.Count - 1)
                    {
                        index++;
                        int lastIndex = index;
                        while (index <= lastIndex)
                        {
                            ltrAddition.Text += string.Format(NewsModel.newsPageHtml, sessionNews[index].ListImage, sessionNews[index].RedirectPage, sessionNews[index].Title, Shared.Helpers.GetDateTurkishCulture(Convert.ToDateTime(sessionNews[index].Created)), sessionNews[index].RedirectPage);
                            index++;
                        }

                        ViewState.Add("LastNewsId", sessionNews[lastIndex].ID.ToString());
                    }
                    else
                    {
                        Button1.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }
    }
}