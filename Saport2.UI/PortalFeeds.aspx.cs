using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using HLP = Saport2.Shared.Helpers;
using EXP = Saport2.Shared.Exceptions;
using System.Text;

namespace Saport2.UI
{
    public partial class PortalFeeds : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    #region Load Portalfeeds
                    List<PortalFeedsModel.PortalFeed> allFeeds = PortalFeedsService.DeserializeXMLToPortalFeedsLight(PortalFeedsModel.portalFeedsSaveFolder, PortalFeedsModel.portalFeedSavedFileName, 50);

                    int i = 0;
                    foreach (var item in allFeeds)
                    {
                        i++;
                        ltrMain.Text += string.Format(PortalFeedsModel.htmlForPortalFeedsPage, item.IconCode, PortalFeedsService.GetRedirectUrl(item.Category.ToString(), item.Id), HLP.GetDateTurkishCulture(Convert.ToDateTime(item.Created)), item.Title);
                        if (i == allFeeds.Count)
                        {
                            ViewState.Add("LastPortalFeedId", item.Id.ToString());
                            ViewState.Add("LastPortalFeedCategory", item.Category.ToString());
                        }
                    }
                    #endregion
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

                int lastPortalFeedId = Convert.ToInt32(ViewState["LastPortalFeedId"]);
                string lastPortalFeedCat= ViewState["LastPortalFeedCategory"] != null ? ViewState["LastPortalFeedCategory"].ToString() : "";
                List<PortalFeedsModel.PortalFeed> sessionPortalFeeds = PortalFeedsService.DeserializeXMLToPortalFeedsLight(PortalFeedsModel.portalFeedsSaveFolder, PortalFeedsModel.portalFeedSavedFileName);


                if (sessionPortalFeeds != null)
                {
                    var lastPortalFeed = sessionPortalFeeds.Where(x => x.Id == lastPortalFeedId && x.Category == lastPortalFeedCat).ToList();
                    int index = sessionPortalFeeds.IndexOf((PortalFeedsModel.PortalFeed)lastPortalFeed[0]);

                    if (index+10 < sessionPortalFeeds.Count-1)
                    {
                        index++;
                        int lastIndex = index + 4; // adding 5 more portalfeeds
                        while (index <= lastIndex)
                        {
                            ltrMain.Text += string.Format(PortalFeedsModel.htmlForPortalFeedsPage, sessionPortalFeeds[index].IconCode, PortalFeedsService.GetRedirectUrl(sessionPortalFeeds[index].Category.ToString(), sessionPortalFeeds[index].Id), HLP.GetDateTurkishCulture(Convert.ToDateTime(sessionPortalFeeds[index].Created)), sessionPortalFeeds[index].Title);
                            index++;
                        }

                        ViewState.Add("LastPortalFeedId", sessionPortalFeeds[lastIndex].Id.ToString());
                        ViewState.Add("LastPortalFeedCategory", sessionPortalFeeds[lastIndex].Category.ToString());
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