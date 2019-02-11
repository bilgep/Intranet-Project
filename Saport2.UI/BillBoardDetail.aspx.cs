using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using SP = Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.UI
{
    public partial class BillBoardDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string category = string.Empty;
                    int advertId = 0;
                    advertId = Request.QueryString["AdvertId"] != null ? Convert.ToInt32(Request.QueryString["AdvertId"]) : 0;
                    category = Request.QueryString["Cat"] != null ? Request.QueryString["Cat"].ToString() : "";

                    if (advertId > 0 && !string.IsNullOrEmpty(category))
                    {

                        #region Load Advert Images
                        List<string> imageServerUrls = DAT.DataQuery.QueryFolderFileUrls(BillBoardModel.advertsSiteUrl, BillBoardModel.goruntulerListName, advertId.ToString());
                        if (imageServerUrls.Count > 0)
                        {
                            foreach (var item in imageServerUrls)
                            {
                                ltrAnnouncementImages2.Text = ltrAnnouncementImages.Text += string.Format(AnnouncementModel.announcementDetailImageHtml, HLP.ResizeAndSaveRemoteImageToLocal(item.ToString(), HttpContext.Current.Session.SessionID, true, 200, 100, 14));
                            }
                        }
                        else
                        {
                            ltrAnnouncementImages.Text += "<img src=\"data:image/png;base64," + BillBoardModel.defaultImageBase64 + "\"/>";
                        }

                        #endregion

                        #region Load Advert Info
                        BillBoardModel.Advert advert = BillBoardService.QueryAdvertDetails(advertId)[0];
                        ltrTitle.Text = advert.Title;
                        ltrCategory.Text = category;
                        ltrDateCreated.Text = HLP.GetDateTurkishCulture(advert.Created);
                        ltrDetails.Text = advert.Detail;
                        ltrPrice.Text = advert.Price.ToString();
                        ltrSpotText.Text = advert.Description;
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {

                EXP.RedirectToErrorPage(ex.Message + " - " + ex.Source + " - " + ex.Data);
            }
        

        }
    }
}