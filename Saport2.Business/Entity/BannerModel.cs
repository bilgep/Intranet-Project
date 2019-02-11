using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using HLP = Saport2.Shared.Helpers;

namespace Saport2.Business.Entity
{
    public class BannerModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Statics & Constants
        private const string userNameForService = "x";
        private const string passwordForService = "x";
        private const string domainForService = "x";

        public static ListItemCollection QueryListItems(string siteUrl, string listName, string queryText)
        {
            try
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userNameForService, passwordForService, domainForService);
                ClientContext clientContext = new ClientContext(siteUrl);
                clientContext.Credentials = credentials;
                //clientContext.ExecuteQuery();
                List oList = clientContext.Web.Lists.GetByTitle(listName);
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = queryText;
                ListItemCollection collListItems = oList.GetItems(camlQuery);
                clientContext.Load(collListItems);
                clientContext.ExecuteQuery();
                clientContext.Dispose();

                return collListItems;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public const string bannersListName = "Banners";
        public const string bannersSiteUrl = "https://saport.X.com/tr-tr/";
        public const string bannersListUrl = "https://saport.X.com/tr-tr/Lists/Banners/";
        public const string announcementsSaveFileName = "Banners.xml";
        public const string announcementsSaveFolder = "temp";
        public const string latestBannersCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='URL'/><FieldRef Name='FileLeafRef'/><FieldRef Name='MetaInfo'/><FieldRef Name='Created'/></ViewFields>
                                    <Query>
                                    <OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy>
                                    </Query>
                                    <RowLimit>5</RowLimit>
                                    </View>";
        public const string latestBannersForXmlCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='URL'/><FieldRef Name='FileLeafRef'/><FieldRef Name='MetaInfo'/></ViewFields>
                                    <Query>
                                    <OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy>
                                    </Query>
                                    <RowLimit>{0}</RowLimit>
                                    </View>";
        public const string bannerDetailCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='URL'/><FieldRef Name='FileLeafRef'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='ID'/><Value Type='Number'>{0}</Value></Eq></Where></Query>
                                    <RowLimit>1</RowLimit>
                                    </View>";
        //public static string wrapTextForHomePage = "<div><a href=\"{0}\"><img src=\"data:image/png;base64,{1}\" width=\"0\" height=\"0\" border=\"0\" alt=\"\"/></a></div>";
        public const string wrapTextForHomePage = "<div><a href=\"{0}\"><img src=\"{1}\" width=\"0\" height=\"0\" border=\"0\" alt=\"\"/></a></div>";
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        public enum BannerType
        {
            News,
            Announcement,
            Campaign,
            Post,
            Other
        }

        public class Banner
        {
            public string _imageUrl = string.Empty;
            public string _mobileImageUrl = string.Empty;
            public string _linkUrl = string.Empty;
            public bool _isExternalLink = true;
            public string _title = string.Empty;

            public int ID { get; set; }

            public string ImageUrl
            {
                get
                {
                    if (!_imageUrl.Contains("http"))
                    {
                        _imageUrl = bannersListUrl + "/" + _imageUrl;
                    }
                    return _imageUrl;
                }
                set
                {
                    _imageUrl = value;
                }
            }


            public string MobileImageUrl
            {
                get; set;
            }


            public string LinkUrlForMobile
            {
                get { return _linkUrl; }
                set
                {
                    _linkUrl = HttpUtility.UrlDecode(value);

                    if (IsExternalLink == false)
                    {
                        if (_linkUrl.Contains("goo.gl"))
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_linkUrl);
                            request.AllowAutoRedirect = false;
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            if ((int)response.StatusCode == 301 || (int)response.StatusCode == 302)
                            {
                                _linkUrl = response.Headers["Location"];
                                _linkUrl = HttpUtility.UrlEncode(_linkUrl);
                            }
                            if (_linkUrl.Contains("/haberler/"))
                            {
                                this.BannerTypeName = BannerType.News;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);

                                //ItemId = (int)NewsService.QueryNewsDetailsForBanner(_linkUrl);
                                Microsoft.SharePoint.Client.ListItemCollection coll = QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, string.Format(NewsModel.newsDetailForBannerCamlQuery, _linkUrl));
                                ListItem item = coll[0];
                                ItemId = Convert.ToInt32(item["ID"]);


                                RedirectPage = string.Format(NewsModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/kampanyalar/"))
                            {
                                this.BannerTypeName = BannerType.Campaign;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);

                                //ItemId = (int)CampaignService.QueryCampaignDetailsForBanner(_linkUrl);
                                ListItemCollection coll = QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignDetailForBannerCamlQuery, _linkUrl));
                                ListItem item = coll[0];
                                ItemId = Convert.ToInt32(item["ID"]);


                                RedirectPage = string.Format(CampaignModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/duyurular/"))
                            {
                                this.BannerTypeName = BannerType.Announcement;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);

                                //ItemId = (int)AnnouncementService.QueryAnnouncementDetailsForBanner(_linkUrl);
                                Microsoft.SharePoint.Client.ListItemCollection coll = QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, string.Format(AnnouncementModel.announcementDetailForBannerCamlQuery, _linkUrl));
                                ListItem item = coll[0];
                                ItemId = Convert.ToInt32(item["ID"]);


                                RedirectPage = string.Format(AnnouncementModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/koseyazilari/"))
                            {
                                this.BannerTypeName = BannerType.Post;

                                string category = string.Empty;
                                var array1 = _linkUrl.Split('/');
                                int indexOfCategory = array1.ToList().IndexOf("koseyazilari");
                                category = array1[indexOfCategory + 1].ToString();

                                if (_linkUrl.Contains('?'))
                                {
                                    var array2 = _linkUrl.Split('?');
                                    string idQueryString = array2[1];
                                    var id = idQueryString.Split('=')[1];
                                    RedirectPage = string.Format(PostModel.postDetailUrl, id, category);
                                }
                                else
                                {
                                    //PostModel.Post post = PostService.QueryPostDetail(Title, category);
                                    PostModel.Post post = new PostModel.Post();
                                    Microsoft.SharePoint.Client.ListItemCollection coll = QueryListItems(string.Format(PostModel.postsSiteUrl, category), PostModel.postsListName, string.Format(PostModel.postDetailCamlQueryByTitle, Title));
                                    if (coll.Count > 0 && coll[0] != null)
                                    {
                                        ListItem item = coll[0];
                                        post.Id = Convert.ToInt32(item["ID"]);
                                        post.CategoryName = category;
                                    }


                                    RedirectPage = string.Format(PostModel.postDetailUrl, post.Id, category);
                                }

                            }
                        }
                        else
                        {
                            if (_linkUrl.Contains("/haberler/"))
                            {
                                this.BannerTypeName = BannerType.News;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);

                                //ItemId = (int)NewsService.QueryNewsDetailsForBanner(_linkUrl);
                                Microsoft.SharePoint.Client.ListItemCollection coll = QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, string.Format(NewsModel.newsDetailForBannerCamlQuery, _linkUrl));
                                ListItem item = coll[0];
                                ItemId = Convert.ToInt32(item["ID"]);

                                RedirectPage = string.Format(NewsModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/kampanyalar/"))
                            {
                                this.BannerTypeName = BannerType.Campaign;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);

                                //ItemId = (int)CampaignService.QueryCampaignDetailsForBanner(_linkUrl);
                                ListItemCollection coll = QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignDetailForBannerCamlQuery, _linkUrl));
                                ListItem item = coll[0];
                                ItemId = Convert.ToInt32(item["ID"]);


                                RedirectPage = string.Format(CampaignModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/duyurular/"))
                            {
                                this.BannerTypeName = BannerType.Announcement;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);

                                //ItemId = (int)AnnouncementService.QueryAnnouncementDetailsForBanner(_linkUrl);
                                Microsoft.SharePoint.Client.ListItemCollection coll = QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, string.Format(AnnouncementModel.announcementDetailForBannerCamlQuery, _linkUrl));
                                ListItem item = coll[0];
                                ItemId = Convert.ToInt32(item["ID"]);

                                RedirectPage = string.Format(AnnouncementModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/koseyazilari/"))
                            {
                                this.BannerTypeName = BannerType.Post;

                                string category = string.Empty;
                                var array1 = _linkUrl.Split('/');
                                int indexOfCategory = array1.ToList().IndexOf("koseyazilari");
                                category = array1[indexOfCategory + 1].ToString();

                                if (_linkUrl.Contains('?'))
                                {
                                    var array2 = _linkUrl.Split('?');
                                    string idQueryString = array2[1];
                                    var id = idQueryString.Split('=')[1];

                                    RedirectPage = string.Format(PostModel.postDetailUrl, id, category);
                                }
                                else
                                {
                                    //PostModel.Post post = PostService.QueryPostDetail(Title, category);
                                    PostModel.Post post = new PostModel.Post();
                                    Microsoft.SharePoint.Client.ListItemCollection coll = QueryListItems(string.Format(PostModel.postsSiteUrl, category), PostModel.postsListName, string.Format(PostModel.postDetailCamlQueryByTitle, Title));
                                    if (coll.Count > 0 && coll[0] != null)
                                    {
                                        ListItem item = coll[0];
                                        post.Id = Convert.ToInt32(item["ID"]);
                                        post.CategoryName = category;
                                    }


                                    RedirectPage = string.Format(PostModel.postDetailUrl, post.Id, category);
                                }

                            }
                            else if (!_linkUrl.Contains("saport.x.com") && (_linkUrl.Contains("http://") || _linkUrl.Contains("https://")))
                            {
                                this.BannerTypeName = BannerType.Other;

                                RedirectPage = _linkUrl;
                            }
                        }
                    }
                }
            }

            [ScriptIgnore]
            public string LinkUrl
            {
                get { return _linkUrl; }
                set
                {
                    _linkUrl = HttpUtility.UrlDecode(value);

                    if (IsExternalLink == false)
                    {
                        if (_linkUrl.Contains("goo.gl"))
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_linkUrl);
                            request.AllowAutoRedirect = false;
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            if ((int)response.StatusCode == 301 || (int)response.StatusCode == 302)
                            {
                                _linkUrl = response.Headers["Location"];
                                _linkUrl = HttpUtility.UrlEncode(_linkUrl);
                            }
                            if (_linkUrl.Contains("/haberler/"))
                            {
                                this.BannerTypeName = BannerType.News;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);
                                ItemId = (int)NewsService.QueryNewsDetailsForBanner(_linkUrl);
                                RedirectPage = string.Format(NewsModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/kampanyalar/"))
                            {
                                this.BannerTypeName = BannerType.Campaign;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);
                                ItemId = (int)CampaignService.QueryCampaignDetailsForBanner(_linkUrl);
                                RedirectPage = string.Format(CampaignModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/duyurular/"))
                            {
                                this.BannerTypeName = BannerType.Announcement;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);
                                ItemId = (int)AnnouncementService.QueryAnnouncementDetailsForBanner(_linkUrl);
                                RedirectPage = string.Format(AnnouncementModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/koseyazilari/"))
                            {
                                this.BannerTypeName = BannerType.Post;

                                string category = string.Empty;
                                var array1 = _linkUrl.Split('/');
                                int indexOfCategory = array1.ToList().IndexOf("koseyazilari");
                                category = array1[indexOfCategory + 1].ToString();

                                if (_linkUrl.Contains('?'))
                                {
                                    var array2 = _linkUrl.Split('?');
                                    string idQueryString = array2[1];
                                    var id = idQueryString.Split('=')[1];
                                    RedirectPage = string.Format(PostModel.postDetailUrl, id, category);
                                }
                                else
                                { 
                                    PostModel.Post post = PostService.QueryPostDetail(_title, category);
                                    RedirectPage = string.Format(PostModel.postDetailUrl, post.Id, category);
                                }

                            }
                        }
                        else
                        {
                            if (_linkUrl.Contains("/haberler/"))
                            {
                                this.BannerTypeName = BannerType.News;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);
                                ItemId = (int)NewsService.QueryNewsDetailsForBanner(_linkUrl);
                                RedirectPage = string.Format(NewsModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/kampanyalar/"))
                            {
                                this.BannerTypeName = BannerType.Campaign;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);
                                ItemId = (int)CampaignService.QueryCampaignDetailsForBanner(_linkUrl);
                                RedirectPage = string.Format(CampaignModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/duyurular/"))
                            {
                                this.BannerTypeName = BannerType.Announcement;
                                var array = _linkUrl.Split('/');
                                _linkUrl = array[array.Length - 1];
                                _linkUrl = HttpUtility.UrlDecode(_linkUrl);
                                ItemId = (int)AnnouncementService.QueryAnnouncementDetailsForBanner(_linkUrl);
                                RedirectPage = string.Format(AnnouncementModel.redirectPage, ItemId);
                            }
                            else if (_linkUrl.Contains("/koseyazilari/"))
                            {
                                this.BannerTypeName = BannerType.Post;

                                string category = string.Empty;
                                var array1 = _linkUrl.Split('/');
                                int indexOfCategory = array1.ToList().IndexOf("koseyazilari");
                                category = array1[indexOfCategory + 1].ToString();

                                if (_linkUrl.Contains('?'))
                                {
                                    var array2 = _linkUrl.Split('?');
                                    string idQueryString = array2[1];
                                    var id = idQueryString.Split('=')[1];

                                    RedirectPage = string.Format(PostModel.postDetailUrl, id, category);
                                }
                                else
                                {
                                    PostModel.Post post = PostService.QueryPostDetail(_title, category);
                                    RedirectPage = string.Format(PostModel.postDetailUrl, post.Id, category);
                                }

                            }
                            else if (!_linkUrl.Contains("saport.x.com") && (_linkUrl.Contains("http://") || _linkUrl.Contains("https://")))
                            {
                                this.BannerTypeName = BannerType.Other;

                                RedirectPage = _linkUrl;
                            }
                        }
                    }
                }
            }
            public string Title
            {
                get
                { return _title; }
                set
                { _title = value; }
            }

            [ScriptIgnore]
            public bool IsExternalLink
            {
                get
                {
                    return _isExternalLink;
                }
                set
                {
                    _isExternalLink = value;
                }
            }

            public DateTime Created { get; set; }
            public BannerType BannerTypeName { get; set; }

            [ScriptIgnore]
            public int ItemId { get; set; }

            [ScriptIgnore]
            public string RedirectPage { get; set; }
        }
        #endregion
    }
}
