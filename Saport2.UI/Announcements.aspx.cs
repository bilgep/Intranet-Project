using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using HLP = Saport2.Shared.Exceptions;
using Saport2.Shared;

namespace Saport2.UI
{
    public partial class Announcements : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Load Announcements
                List<AnnouncementModel.Announcement> announs = AnnouncementService.DeserializeXMLToAnnsLight(AnnouncementModel.announcementsSaveFolder, AnnouncementModel.announcementsSaveFileName, 80);
                foreach (var ann in announs)
                {
                    string redirectUrl = string.Format(ann.RedirectPage, ann.ID);
                    string thumbImage = "<img class=\"media-object\" src=\"data:image/png;base64," + ann.ListImage + "\"/>";
                    ltrMain.Text += string.Format(AnnouncementModel.announcementItemHtml, thumbImage, redirectUrl, ann.Title, Shared.Helpers.GetDateTurkishCulture(Convert.ToDateTime(ann.Created)), redirectUrl);
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