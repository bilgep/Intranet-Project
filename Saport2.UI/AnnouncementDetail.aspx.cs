using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using HLP = Saport2.Shared.Exceptions;

namespace Saport2.UI
{
    public partial class AnnouncementDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Load Announcement details
                int? announcementId = Convert.ToInt32(Request.QueryString["AnnId"]);
                if (announcementId.HasValue && announcementId > 0)
                {
                    AnnouncementModel.Announcement thisAnn = AnnouncementService.QueryAnnouncementDetails((int)announcementId);
                    ltrAnnContent.Text = thisAnn.PublishingPageContent;
                    ltrTitle.Text = thisAnn.Title;
                }
                else
                {
                    Response.Redirect("Announcements.aspx",false);
                }
                #endregion
            }
            catch (Exception ex)
            {
                HLP.RedirectToErrorPage(ex.Message);
            }

        }
    }
}