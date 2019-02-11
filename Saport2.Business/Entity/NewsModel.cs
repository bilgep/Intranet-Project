using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SP = Microsoft.SharePoint.Client;

namespace Saport2.Business.Entity
{
    public class NewsModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Statics & Constants
        public const string sayfalarListName = "Sayfalar";
        public const string newsSiteUrl = "https://saport.x.com/tr-tr/haberler/";
        public const string newsSaveFileName = "News.xml";
        public const string newsSaveFolder = "temp";
        public const string redirectPage = "/NewsDetail.aspx?NewsId={0}";
        public const string newsLightCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Created'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>100</RowLimit>
                                  </View>";
        public const string newsCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Created'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/><FieldRef Name='ListImage'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>10</RowLimit>
                                  </View>";
        public const string newsLimitedCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Created'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/><FieldRef Name='ListImage'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>{0}</RowLimit>
                                  </View>";
        public const string newsDetailCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='Created'/><FieldRef Name='Modified'/><FieldRef Name='MetaInfo'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='ID'/><Value Type='Number'>{0}</Value></Eq></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string newsDetailForBannerCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='Title'/><FieldRef Name='Name'/></ViewFields>
                                    <Query><Where><Contains><FieldRef Name='FileLeafRef'/><Value Type='Text'>{0}</Value></Contains></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string newsPageHtml = "<div class=\"media\" style=\"margin-bottom:40px\"><a class=\"pull-left\" href=\"#\"><img class=\"media-object\" src=\"data:image/png;base64,{0}\"/></a><div class=\"media-body\"><h4 class=\"media-heading\" style=\"margin-bottom:25px\"><a href=\"{1}\" >{2}</a></h4><div class=\"media\"><div></div><div class=\"navbar nav-actions\"><div class=\"navbar-inner\"><ul class=\"nav pull-left\" style=\" margin-top:10px\"><li><b>Tarih:</b> {3}</li></ul><ul class=\"nav pull-right\"><li><a href=\"{4}\">Haberin Devamı<i class=\"icon-angle-double-right\"></i></a></li></ul></div></div></div></div></div>";
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties

        public class NewsObject
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public List<News> Data { get; set; }
        }

        [Serializable, XmlRoot("News")]
        public class News
        {
            [XmlElement("ID")]
            public int? ID { get; set; }

            [XmlElement("Created")]
            public DateTime? Created { get; set; }
            public DateTime Modified { get; set; }

            public string ImageUrl { get; set; }
            public string MobileImageUrl { get; set; }

            [XmlElement("ListImage")]
            public string ListImage { get; set; }
            public string PublishingPageContent { get; set; }

            [XmlElement("Title")]
            public string Title { get; set; }
            public string BaseName { get; set; }
            public string RedirectPage { get { return "/NewsDetail.aspx?NewsId=" + ID; } }
        }
        #endregion


    }
}
