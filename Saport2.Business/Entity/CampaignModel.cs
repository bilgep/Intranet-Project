using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Saport2.Business.Entity
{
    public class CampaignModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Statics
        public const string sayfalarListName = "Sayfalar";
        public const string goruntulerListName = "PublishingImages";
        public const string campaignsSiteUrl = "https://saport.x.com/tr-tr/kampanyalar/";
        public const string campaignsSaveFileName = "Campaigns.xml";
        public const string campaignsSaveFolder = "temp";
        public const string redirectPage = "/CampaignDetail.aspx?CampId={0}";
        public const string campaignsLightCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='Created'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>100</RowLimit>
                                  </View>";
        public const string campaignsCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='MetaInfo'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>30</RowLimit>
                                  </View>";
        public const string campaignsCamlQuery6 = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='MetaInfo'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>6</RowLimit>
                                  </View>";
        public const string campaignsCamlQueryForService = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='MetaInfo'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>{0}</RowLimit>
                                  </View>";
        public const string campaignsLimitedCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='MetaInfo'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>{0}</RowLimit>
                                  </View>";
        public const string campaignsCamlQueryForHomePage = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='MetaInfo'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>20</RowLimit>
                                  </View>";
        public const string campaignDetailCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='Created'/><FieldRef Name='MetaInfo'/><FieldRef Name='FileLeafRef'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='ID'/><Value Type='Number'>{0}</Value></Eq></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string campaignDetailForBannerCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='Title'/><FieldRef Name='Name'/></ViewFields>
                                    <Query><Where><Contains><FieldRef Name='FileLeafRef'/><Value Type='Text'>{0}</Value></Contains></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string htmlDivForHomePage = "<li><div class=\"resim\"><a href=\"{2}\"><img src=\"data:image/png;base64,{0}\" width=\"110px\" height=\"70px\"></a></div><div class=\"icerik\"><a href=\"{2}\">{3}</a></div></li>";
        public const string htmlDivForHomePage2 = "<li><div class=\"resim\"><a href=\"{2}\"><img src=\"{0}\" width=\"110px\" height=\"70px\"></a></div><div class=\"icerik\"><a href=\"{2}\">{3}</a></div></li>";
        public const string htmlForCampaignsPage = "<li><div class=\"resim\"><img width=\"75px\" height=\"70px\" src=\"data:image/png;base64,{0}\"></div><div class=\"icerik\" ><a href=\"{1}\">{2}</a></div></li>";
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties

        [XmlRoot("Announcement")]
        public class Campaign
        {
            [XmlElement("ID")]
            public int? ID { get; set; }
            public DateTime? CampaignStartDate { get; set; }

            [XmlElement("CampaignEndDate")]
            public DateTime? CampaignEndDate { get; set; }

            public DateTime? Created { get; set; }

            [XmlElement("ListImage")]
            public string ListImage { get; set; }

            public string PublishingPageContent { get; set; }

            [XmlElement("Title")]
            public string Title { get; set; }

            [ScriptIgnore]
            public string RedirectPage { get { return "/CampaignDetail.aspx?CampId=" + ID; } }
        }

        #endregion
    }
}
