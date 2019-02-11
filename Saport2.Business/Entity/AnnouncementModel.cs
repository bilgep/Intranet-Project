using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using SP = Microsoft.SharePoint.Client;
using HLP = Saport2.Shared.Helpers;

namespace Saport2.Business.Entity
{
    public class AnnouncementModel
    {
        #region About
        /* Saport => TR => Duyurular Site'ındaki SAYFALAR Document Library'deki içerikler baz alınarak oluşturuldu */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Statics & Constants
        public const string sayfalarListName = "Sayfalar";
        public const string goruntulerListName = "Görüntüler";
        public const string redirectPage = "/AnnouncementDetail.aspx?AnnId={0}";
        public const string queryUrl = "https://saport.X.com/tr-tr/duyurular/_api/web/lists/GetByTitle('Sayfalar')/items?$select=ID,Title,PublishingPageContent,OrderNumber,FixedAnnouncement,AuthorId,PublishingContactId,ArticleStartDate&$top=10&$orderby=Id desc";
        public const string announcementsSiteUrl = "https://saport.X.com/tr-tr/duyurular/";
        public const string announcementsSaveFileName = "Announcements.xml";
        public const string announcementsSaveFolder = "temp";

        public const string announcementsCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Created'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/><FieldRef Name='ListImage'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>{0}</RowLimit>
                                  </View>";
        public const string announcementDetailCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Created'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/><FieldRef Name='PublishingPageContent'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='ID'/><Value Type='Number'>{0}</Value></Eq></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string announcementsLightCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Created'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>100</RowLimit>
                                  </View>";
        public const string announcementsLightCamlQueryForHomePage = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Created'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>{0}</RowLimit>
                                  </View>";
        public const string announcementDetailForBannerCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='Title'/><FieldRef Name='Name'/></ViewFields>
                                    <Query><Where><Contains><FieldRef Name='FileLeafRef'/><Value Type='Text'>{0}</Value></Contains></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string announcementItemHtml = "<div class=\"media\" style=\"margin-bottom:40px\"><a class=\"pull-left\" href=\"#\">{0}</a><div class=\"media-body\"><h4 class=\"media-heading\" style=\"margin-bottom:25px\"><a href=\"{1}\">{2}</a></h4><div class=\"media\"><div class=\"navbar nav-actions\"><div class=\"navbar-inner\"><ul class=\"nav pull-left\" style=\" margin-top:10px\"><li><b>Tarih:</b> {3}</li></ul><ul class=\"nav pull-right\"><li><a href=\"{4}\">Duyuru Devamı <i class=\"icon-angle-double-right\"></i></a></li></ul></div></div></div></div></div>";
        public const string announcementDetailImageHtml = "<li><img src=\"{0}\"/></li>";

        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties 

        public class PublishingImage
        {
            public string Title { get; set; }
            public string VideoUrl { get { return VideoUrl; }
                set
                {
                    VideoUrl = value.Split(',')[0];
                }
            }
        }

        [XmlRoot("Announcement")]
        public class Announcement
        {
            [XmlElement("ID")]
            public int? ID { get; set; }

            [XmlElement("Created")]
            public DateTime? Created { get; set; }
            public SP.FieldUserValue PublishingContactUser { get; set; }
            public string Author {
                get {
                    return (PublishingContactUser != null) ?  PublishingContactUser.LookupValue.ToString().Split('#')[0] : "Saport";
                }}

            [XmlElement("ListImage")]
            public string ListImage { get; set;}

            public string PublishingPageContent { get; set; }

            [XmlElement("Title")]
            public string Title { get; set; }
            public string BaseName { get; set; }
            public string RedirectPage { get { return "/AnnouncementDetail.aspx?AnnId=" + ID; } }

        }
        #endregion
    }
}
