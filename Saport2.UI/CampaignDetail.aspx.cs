using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using Saport2.Business;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.UI
{
    public partial class CampaignDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Load Campaign Details
                int? campId = Convert.ToInt32(Request.QueryString["CampId"]);
                if (campId.HasValue && campId > 0)
                {
                    CampaignModel.Campaign camp = CampaignService.QueryCampaignDetails((int)campId);
                    ltrTitle.Text = camp.Title;
                    ltrEndDate.Text = Shared.Helpers.GetDateTurkishCulture(Convert.ToDateTime(camp.CampaignEndDate));
                    ltrPageContent.Text = camp.PublishingPageContent;
                }
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

            #endregion
        }
    }
}