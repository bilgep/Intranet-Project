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
    public partial class Campaigns : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    List<CampaignModel.Campaign> camps = CampaignService.DeserializeXMLToCampsLight(CampaignModel.campaignsSaveFolder, CampaignModel.campaignsSaveFileName, 10);
                    foreach (var camp in camps)
                    {
                        ltrMain.Text += string.Format(CampaignModel.htmlForCampaignsPage, camp.ListImage, camp.RedirectPage, camp.Title);
                    }

                    ViewState.Add("LastCampaignId", camps[camps.Count-1].ID.ToString());
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
                int lastCampaignId = Convert.ToInt32(ViewState["LastCampaignId"]);

                List<CampaignModel.Campaign> sessionCampaigns = CampaignService.DeserializeXMLToCampsLight(CampaignModel.campaignsSaveFolder, CampaignModel.campaignsSaveFileName, 100);


                if (sessionCampaigns != null)
                {
                    var lastCampaign = sessionCampaigns.Where(x => x.ID == lastCampaignId).ToList();
                    int index = sessionCampaigns.IndexOf((CampaignModel.Campaign)lastCampaign[0]);

                    if (index < sessionCampaigns.Count - 2)
                    {
                        index++;
                        int lastIndex = index + 1; 
                        while (index <= lastIndex)
                        {
                            ltrAddition.Text += string.Format(CampaignModel.htmlForCampaignsPage,sessionCampaigns[index].ListImage, sessionCampaigns[index].RedirectPage,  sessionCampaigns[index].Title);
                            index++;

                        }

                        ViewState.Add("LastCampaignId", sessionCampaigns[lastIndex].ID.ToString());
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