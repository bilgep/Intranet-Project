using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using EXP = Saport2.Shared.Exceptions;
using HLP = Saport2.Shared.Helpers;
using System.Net;
using System.Globalization;
using System.Text;

namespace Saport2.UI
{
    public partial class Home : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {

                    #region Load Posts
                    List<PostModel.Post> homePosts = PostService.DeserializeXMLToPostsLight(PostModel.postsLightSaveFolder, PostModel.postsLightFileName, 4);

                    if (homePosts.Count > 0)
                    {
                        foreach (var item in homePosts)
                        {
                            string postDetailUrl = string.Format(PostModel.postDetailUrl, item.Id, item.CategoryName);
                            ltrPosts.Text += string.Format(PostModel.htmlDivPostsForHomePage, postDetailUrl, postDetailUrl, item.Title);
                        }
                    }

                    #endregion

                    #region Load Campaigns
                    List<CampaignModel.Campaign> homeCamps = CampaignService.DeserializeXMLToCampsLight(CampaignModel.campaignsSaveFolder, CampaignModel.campaignsSaveFileName, 6);

                    if (homeCamps.Count == 0)
                    {
                        homeCamps.Clear();
                        homeCamps = CampaignService.QueryLastSixCampaigns();

                        foreach (var item in homeCamps)
                        {
                            item.Title = item.Title.Length > 30 ? item.Title.Substring(0, 29) + "..." : item.Title;
                            ltrCampaigns.Text += string.Format(CampaignModel.htmlDivForHomePage2, item.ListImage, item.RedirectPage, item.RedirectPage, item.Title);
                        }
                    }
                    else
                    {
                        foreach (var item in homeCamps)
                        {
                            item.Title = item.Title.Length > 30 ? item.Title.Substring(0, 29) + "..." : item.Title;
                            ltrCampaigns.Text += string.Format(CampaignModel.htmlDivForHomePage, item.ListImage, item.RedirectPage, item.RedirectPage, item.Title);
                        }
                    }


                    if (homeCamps.Count <= 3) divCampaigns.Attributes.Add("style", "min-height:180px");


                    #endregion

                    #region Load Portal Feeds
                    List<PortalFeedsModel.PortalFeed> homeFeeds = PortalFeedsService.DeserializeXMLToPortalFeedsLight("temp", "PortalFeeds.xml", 10);

                    if (homeFeeds.Count > 0)
                    {
                        foreach (var item in homeFeeds)
                        {
                            string redirectUrl = string.Empty;
                            switch (item.Category)
                            {
                                case "Announcement":
                                    redirectUrl = string.Format(PortalFeedsModel.redirectToAnnouncementDetailUrl, item.Id);
                                    break;
                                case "Campaign":
                                    redirectUrl = string.Format(PortalFeedsModel.redirectToCampaignDetailUrl, item.Id);
                                    break;
                                case "News":
                                    redirectUrl = string.Format(PortalFeedsModel.redirectToNewsDetailUrl, item.Id);
                                    break;
                            }

                            ltrPortalFeeds.Text += string.Format(PortalFeedsModel.htmlForHomePage, item.IconCode, redirectUrl, item.Title, item.Title);
                        }
                    }

                    #endregion

                    #region Load Banners
                    List<BannerModel.Banner> activeBanners = BannerService.DeserializeXMLToBannersLight(BannerModel.announcementsSaveFolder, BannerModel.announcementsSaveFileName, 5);
                    if (activeBanners.Count > 0)
                    {
                        foreach (var item in activeBanners)
                        {
                            //ltrBanners.Text += string.Format(BannerModel.wrapTextForHomePage, item.RedirectPage, BannerService.GetRemoteImageAsBase64(item.ImageUrl));
                            ltrBanners.Text += string.Format(BannerModel.wrapTextForHomePage, item.RedirectPage, HLP.SaveFileToTempFolderForBanners(HLP.GetNetworkCredential(), item.ImageUrl));
                        }
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message + ex.StackTrace.ToString());
            }
        }


    }
}