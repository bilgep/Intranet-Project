using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saport2.Business.Entity;
using System.Xml.Serialization;
using System.Configuration;
using System.Xml;

namespace Saport2.Business.Entity
{
    public class PortalFeedsModel
    {

        #region About
        /* This model combines News, Announcements, Campaigns and shows a brief collection. (Mapping) */
        #endregion

        #region Enums
        public enum FeedCategory
        {
            Announcement = 1,
            News = 2,
            Campaign = 3
        }
        #endregion

        #region Statics & Constants
        public const string portalFeedsSaveFolder = "temp";
        public const string portalFeedSavedFileName = "PortalFeeds.xml";
        public const string redirectToAnnouncementDetailUrl = "/AnnouncementDetail.aspx?AnnId={0}";
        public const string redirectToNewsDetailUrl = "/NewsDetail.aspx?NewsId={0}";
        public const string redirectToCampaignDetailUrl = "/CampaignDetail.aspx?CampId={0}";
        public const string htmlForHomePage = "<li style=\"margin-top: 0px;\"><span><i class=\"sp-feed-category-{0}\"></i><a href=\"{1}\" title=\"{2}\"><label>{3}</label></a></span></li>";
        public const string htmlForPortalFeedsPage = "<li><span><i class=\"atr\"></i><i  class=\"sp-feed-category-{0}\"></i><a href=\"{1}\">{2} - {3} </a></span></li>";


        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        [Serializable, XmlRoot("PortalFeed")]
        public class PortalFeed
        {
            [XmlElement("Category")]
            public string Category { get; set; }

            [XmlElement("Id")]
            public int Id { get; set; }

            [XmlElement("Title")]
            public string Title { get; set; }

            [XmlElement("Created")]
            public DateTime Created { get; set; }

            public int IconCode {
                get
                {
                    int iconCode = 1;
                    switch (Category)
                    {
                        case "Announcement":
                            iconCode = 1;
                            break;
                        case "News":
                            iconCode = 2;
                            break;
                        case "Campaign":
                            iconCode = 3;
                            break;
                        default:
                            iconCode = 1;
                            break;
                    }
                    return iconCode;
                }
            }

        }

        #endregion








    }
}
