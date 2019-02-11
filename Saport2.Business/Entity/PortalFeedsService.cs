using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using SP = Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;
using System.Xml.Serialization;
using System.Xml;
using System.Security;

namespace Saport2.Business.Entity
{
    public class PortalFeedsService : PortalFeedsModel
    {

        #region
        /* This class includes methods related to PortalServiceModel */
        #endregion

        #region Methods
        public static List<PortalFeed> QueryPortalFeedsLight()
        {
            try
            {
                List<PortalFeed> feeds = new List<PortalFeed>();

                #region Retrieve Announcements Light
                List<AnnouncementModel.Announcement> annFeeds = AnnouncementService.QueryAnnouncementsLight();
                foreach (var item in annFeeds)
                {
                    PortalFeed feedItem = new PortalFeed();
                    feedItem.Category = PortalFeedsModel.FeedCategory.Announcement.ToString();
                    feedItem.Id = item.ID != null ? Convert.ToInt32(item.ID) : 0;
                    feedItem.Created = item.Created != null ? Convert.ToDateTime(item.Created) : DateTime.MinValue;
                    feedItem.Title = item.Title != null ? SecurityElement.Escape(item.Title.ToString().Replace("\u0003", "[0x03]").Replace("\\", "")) : string.Empty;
                    feeds.Add(feedItem);
                }
                #endregion

                #region Retrieve News Light
                List<NewsModel.News> newsFeeds = NewsService.QueryLatestNewsLight();
                foreach (var item in newsFeeds)
                {
                    PortalFeed feedItem = new PortalFeed();
                    feedItem.Category = PortalFeedsModel.FeedCategory.News.ToString();
                    feedItem.Id = item.ID != null ? Convert.ToInt32(item.ID) : 0;
                    feedItem.Created = item.Created != null ? Convert.ToDateTime(item.Created) : DateTime.MinValue;
                    feedItem.Title = item.Title != null ? SecurityElement.Escape(item.Title.ToString().Replace("\u0003", "[0x03]").Replace("\\", "")) : string.Empty;
                    feeds.Add(feedItem);
                }
                #endregion

                #region Retrieve Campaigns Light
                List<CampaignModel.Campaign> campsFeeds = CampaignService.QueryLatestCampaignsLight();
                foreach (var item in campsFeeds)
                {
                    PortalFeed feedItem = new PortalFeed();
                    feedItem.Category = PortalFeedsModel.FeedCategory.Campaign.ToString();
                    feedItem.Id = item.ID != null ? Convert.ToInt32(item.ID) : 0;
                    feedItem.Created = item.Created != null ? Convert.ToDateTime(item.Created) : DateTime.MinValue;
                    feedItem.Title = item.Title != null ? item.Title.ToString().Replace("\u0003", "[0x03]").Replace("\\", "") : string.Empty;
                    feeds.Add(feedItem);
                }
                #endregion

                feeds =  (from x in feeds orderby x.Created descending select x).ToList();
                return feeds;
            }
            catch (Exception ex) 
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<PortalFeed> DeserializeXMLToPortalFeedsLight(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<PortalFeed> portalFeedsLight = new List<PortalFeed>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PortalFeed>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    portalFeedsLight = (List<PortalFeed>)writer.Deserialize(file);
                    file.Close();
                }
                else
                {
                    portalFeedsLight = QueryPortalFeedsLight();
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PortalFeed>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, portalFeedsLight);
                    file.Close();
                }

                return portalFeedsLight;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }



        public static List<PortalFeed> DeserializeXMLToPortalFeedsLightForGlobalAsax(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<PortalFeed> portalFeedsLight = new List<PortalFeed>();
                portalFeedsLight = QueryPortalFeedsLight();
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PortalFeed>));

                HLP.DeleteFile(filefolder, fileName);

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, portalFeedsLight);
                file.Close();

                return portalFeedsLight;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<PortalFeed> DeserializeXMLToPortalFeedsLight(string filefolder, string fileName, int itemCount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<PortalFeed> portalFeedsLight = new List<PortalFeed>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PortalFeed>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    portalFeedsLight = (List<PortalFeed>)writer.Deserialize(file);
                    file.Close();
                }
                else
                {
                    portalFeedsLight = QueryPortalFeedsLight();
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PortalFeed>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, portalFeedsLight);
                    file.Close();
                }
                List<PortalFeed> result = portalFeedsLight.Take(10).ToList();
                return result;

            }
            catch (Exception ex)
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;
                System.IO.File.Delete(fileLocation);
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string GetRedirectUrl(string category, int? id)
        {
            try
            {
                string redUrl = category == FeedCategory.Announcement.ToString() ? string.Format(redirectToAnnouncementDetailUrl, id) : (category == FeedCategory.Campaign.ToString() ? string.Format(redirectToCampaignDetailUrl, id) : (category == FeedCategory.News.ToString() ? string.Format(redirectToNewsDetailUrl, id) : ""));
                return redUrl;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
