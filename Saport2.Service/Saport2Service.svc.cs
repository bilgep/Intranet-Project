using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Saport2.Business.Entity;
using System.ServiceModel.Activation;
using Saport2.Data;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Saport2.Shared;
using EXP = Saport2.Shared.Exceptions;
using HLP = Saport2.Shared.Helpers;
using Microsoft.SharePoint.Client;
using System.Web;

namespace Saport2.Service
{
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Saport2Service : ISaport2Service
    {
        public List<NavModel> GetNavigationJson()
        {
            try
            {
                List<NavModel> navList = NavService.GetNavigationList();
                return navList;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string CheckRemoteService(string secretKey)
        {
            try
            {
                string serviceStatus = "OFFLINE";

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        serviceStatus = DataService.IsServiceUp(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.saportServiceURL(DataStatics.saportServiceTest), true, true, 0, "application/json; charset=UTF-8") == true ? "OK" : "OFFLINE";
                    }
                }

                return serviceStatus;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetCityWeatherStatus(string cityName)
        {
            try
            {
                HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.saportServiceURL(DataStatics.saportServiceGetCityWeatherStatus)
, true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "POST");

                string parsedContent = "{\"CityName\":\"" + cityName + "\"}";
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] bytes = encoding.GetBytes(parsedContent);
                request.ContentLength = bytes.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                WebResponse response = request.GetResponse();
                string responseString = DataService.RestfulReader(response);

                return responseString;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetLatestExchangeRates()
        {
            try
            {
                HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.saportServiceURL(DataStatics.saportServiceGetLatestExchangeRates), true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "GET");

                request.ContentLength = 0;

                WebResponse response = request.GetResponse();
                string responseString = DataService.RestfulReader(response);

                return responseString;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetLatestBlogPosts(string lastPostDate, int itemCount, string secretKey)
        {
            try
            {
                string returnString = "";

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.saportServiceURL(DataStatics.saportServiceGetLatestBlogPosts)
, true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "POST");

                        string parsedContent = "{\"LastPostDate\":\"" + lastPostDate + "\" , \"ItemCount\":" + itemCount + "}";
                        UTF8Encoding encoding = new UTF8Encoding();
                        Byte[] bytes = encoding.GetBytes(parsedContent);
                        request.ContentLength = bytes.Length;
                        Stream newStream = request.GetRequestStream();
                        newStream.Write(bytes, 0, bytes.Length);
                        newStream.Close();

                        WebResponse response = request.GetResponse();
                        returnString = DataService.RestfulReader(response);
                    }
                }

                return returnString;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetSocialFeeds(string newestDate, int itemCount, string secretKey)
        {
            try
            {
                string returnString = "";

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.saportServiceURL(DataStatics.saportServiceGetSocialFeeds), true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "POST");

                        string parsedContent = "{\"NewestDate\":\"" + newestDate + "\",\"ItemCount\":" + itemCount + ",\"Index\":0,\"Filter\":\"All\"}";
                        UTF8Encoding encoding = new UTF8Encoding();
                        Byte[] bytes = encoding.GetBytes(parsedContent);
                        request.ContentLength = bytes.Length;
                        Stream newStream = request.GetRequestStream();
                        newStream.Write(bytes, 0, bytes.Length);
                        newStream.Close();

                        WebResponse response = request.GetResponse();
                        returnString = DataService.RestfulReader(response);
                    }
                }

                return returnString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetLatestAnnouncements(string newestDate, int itemCount, string secretKey)
        {
            try
            {
                string returnString = "";

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, "https://saport.x.com/tr-tr/duyurular/_vti_bin/xPortal/PortalService.svc/GetLatestAnnouncements", true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "POST"); // URL is static because of its location is a subsite

                        string parsedContent = "{\"Filter\":{\"OrderType\":\"\",\"RateOrder\":false},\"NewestDate\":\"" + newestDate + "\",\"ItemCount\":" + itemCount + ",\"Index\":0}";
                        UTF8Encoding encoding = new UTF8Encoding();
                        Byte[] bytes = encoding.GetBytes(parsedContent);
                        request.ContentLength = bytes.Length;
                        Stream newStream = request.GetRequestStream();
                        newStream.Write(bytes, 0, bytes.Length);
                        newStream.Close();

                        WebResponse response = request.GetResponse();
                        returnString = DataService.RestfulReader(response);
                    }
                }

                return returnString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetLatestNews(string newestDate, int itemCount, string secretKey)
        {
            try
            {
                string returnString = "";

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, "https://saport.x.com/tr-tr/haberler/_vti_bin/xPortal/PortalService.svc/GetLatestNews", true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "POST"); // URL is static because of its location is a subsite

                        string parsedContent = "{\"Filter\":{\"OrderType\":\"Order\",\"FirstResponse\":true,\"RateOrder\":false},\"NewestDate\":\"" + newestDate + "\",\"ItemCount\":" + itemCount + ",\"Index\":0}";
                        UTF8Encoding encoding = new UTF8Encoding();
                        Byte[] bytes = encoding.GetBytes(parsedContent);
                        request.ContentLength = bytes.Length;
                        Stream newStream = request.GetRequestStream();
                        newStream.Write(bytes, 0, bytes.Length);
                        newStream.Close();

                        WebResponse response = request.GetResponse();
                        returnString = DataService.RestfulReader(response);

                        var serializer = new JavaScriptSerializer();
                        NewsModel.NewsObject newsListObj = serializer.Deserialize<NewsModel.NewsObject>(returnString);
                        List<NewsModel.News> newsList = newsListObj.Data;

                        foreach (NewsModel.News news in newsList)
                        {
                            string imgUrl = !news.ImageUrl.Contains("http") ? Saport2.Data.DataStatics.saportHostURL + news.ImageUrl.ToString() : news.ImageUrl;
                            news.MobileImageUrl = Saport2.Shared.Helpers.SaveFileForMobile( Helpers.GetNetworkCredential(), imgUrl,"News");
                        }

                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        jsonSerializer.MaxJsonLength = Int32.MaxValue;
                        returnString = jsonSerializer.Serialize(newsList);

                    }
                }

                return returnString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        private NetworkCredential GetNetworkCredential()
        {
            throw new NotImplementedException();
        }

        public string GetNewsDetail(int id, string secretKey)
        {
            try
            {
                string jsonString = string.Empty;

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        #region Retrieve News Detail
                        //NewsModel.News newsDetail = NewsService.QueryNewsDetails(id);

                        NewsModel.News news = new NewsModel.News();
                        Microsoft.SharePoint.Client.ListItemCollection coll = QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, string.Format(NewsModel.newsDetailCamlQuery, id));
                        ListItem item = coll[0];
                        news.ID = id;
                        news.Modified = Convert.ToDateTime(item["Modified"]);
                        news.Title = item["Title"].ToString();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("PublishingPageContent:SW")) news.PublishingPageContent = HLP.TransformHtmlStringForMobile(i.Split('|')[1].Replace("\r", ""));

                        }
                        #endregion

                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        jsonString = jsonSerializer.Serialize(news);
                    }
                }

                return jsonString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetPortalFeeds(string secretKey)
        {
            try
            {
                string jsonString = string.Empty;

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        #region Retrieve Social Feeds
                        List<PortalFeedsModel.PortalFeed> latestPortalFeeds = PortalFeedsService.QueryPortalFeedsLight();
                        #endregion

                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        jsonString = jsonSerializer.Serialize(latestPortalFeeds);
                    }
                }

                return jsonString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetEventList(string includeDate, int count, string secretKey)
        {
            try
            {
                string returnString = "";

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, "https://saport.x.com/tr-TR/_vti_bin/xPortal/PortalService.svc/GetEventList", true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "POST"); // URL is static because of its location is a subsite

                        string parsedContent = "{\"IncludeDate\":\"" + includeDate + "\",\"IsWeekly\": true , \"Count\":" + 10 + "}";
                        UTF8Encoding encoding = new UTF8Encoding();
                        Byte[] bytes = encoding.GetBytes(parsedContent);
                        request.ContentLength = bytes.Length;
                        Stream newStream = request.GetRequestStream();
                        newStream.Write(bytes, 0, bytes.Length);
                        newStream.Close();

                        WebResponse response = request.GetResponse();
                        returnString = DataService.RestfulReader(response);
                    }
                }

                return returnString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetLatestCampaigns(int amount, string secretKey)
        {
            try
            {
                string jsonString = string.Empty;

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {

                        #region Prepare Campaigns
                        //List<CampaignModel.Campaign> activeCampaigns = CampaignService.QueryLatestCampaignsForService(amount);

                        List<CampaignModel.Campaign> camps = new List<CampaignModel.Campaign>();
                        ListItemCollection coll = QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignsCamlQueryForService, amount * 3));
                        if (coll.Count > 0)
                        {
                            foreach (ListItem item in coll)
                            {
                                CampaignModel.Campaign camp = new CampaignModel.Campaign();
                                string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                                foreach (var i in metaInfo)
                                {
                                    if (i.Contains("vti_cachedcustomprops")) continue;
                                    //if (i.Contains("PublishingPageContent:SW"))
                                    //{
                                    //    camp.PublishingPageContent = HLP.TransformHtmlStringForMobile(i.Split('|')[1].Replace("\r", ""));
                                    //}
                                    if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                                    if (i.Contains("ListImage"))
                                    {
                                        if (camp.CampaignEndDate != null && camp.CampaignEndDate >= DateTime.Now)
                                        {
                                            camp.ListImage = DataStatics.saportHostURL + HLP.TransformHtmlStringAndGetFileUrl(i.Split('|')[1].Replace("\r", ""));
                                            break;
                                        }
                                    }
                                }
                                if (camp.CampaignEndDate >= DateTime.Now)
                                {
                                    if (camp.ListImage != string.Empty)
                                        camp.ListImage = "/mobile/" + HLP.ResizeAndSaveFileToMobileFolder(HLP.GetNetworkCredential(), camp.ListImage);
                                    camp.ID = Convert.ToInt32(item["ID"]);
                                    camp.Title = item["Title"].ToString();
                                    camps.Add(camp);
                                }
                            }
                        }

                        #endregion

                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        jsonString = jsonSerializer.Serialize(camps.Take(amount).ToList());

                    }
                }
                return jsonString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetLatestAdverts(int amount, string category, string secretKey)
        {
            try
            {
                string jsonString = string.Empty;

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        #region Prepare Adverts
                        //List<BillBoardModel.Advert> activeAdverts = BillBoardService.QueryLatestCategoryAdvertsForXml(amount, category);

                        List<BillBoardModel.Advert> adverts = new List<BillBoardModel.Advert>();
                        ListItemCollection coll = QueryListItems(BillBoardModel.advertsSiteUrl, BillBoardModel.sayfalarListName, string.Format(BillBoardModel.categoryAdvertsLimitedCamlQuery, category, amount));
                        if (coll.Count > 0)
                        {
                            foreach (ListItem item in coll)
                            {
                                BillBoardModel.Advert adv = new BillBoardModel.Advert();

                                adv.Category = category;
                                adv.ID = Convert.ToInt32(item["ID"]);
                                adv.Title = item["Title"].ToString();
                                string imgUrl = item["DefaultImage"] != null ? item["DefaultImage"].ToString() : "";

                                if (imgUrl == string.Empty)
                                {
                                    adv.DefaultImage = BillBoardModel.defaultImageBase64;
                                }
                                else if (imgUrl.Contains("/tr-tr/ilanpanosu/") && !imgUrl.Contains("x.com"))
                                {
                                    adv.DefaultImage = HLP.RemoteImageUrlToBase64Converter(Data.DataStatics.saportHostURL + imgUrl, true);
                                }
                                else if (imgUrl.Contains("x.com"))
                                {
                                    adv.DefaultImage = HLP.RemoteImageUrlToBase64Converter(imgUrl, true);
                                }
                                else
                                {
                                    adv.DefaultImage = HLP.RemoteImageUrlToBase64Converter(string.Format(BillBoardModel.thumbnailImageRemoteUrl, Convert.ToInt32(item["ID"]), imgUrl), true);
                                }

                                if (adv.DefaultImage.Length == 0 || adv.DefaultImage == null)
                                {
                                    adv.DefaultImage = BillBoardModel.defaultImageBase64;
                                }

                                adverts.Add(adv);
                            }

                            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            jsonString = jsonSerializer.Serialize(adverts);
                            #endregion

                        }
                    }
                }
                return jsonString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetAdvertDetail(int id, string category, string secretKey)
        {
            try
            {
                string jsonString = string.Empty;

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {
                        #region Prepare Adverts
                        //List<BillBoardModel.Advert> activeAdverts = BillBoardService.QueryAdvertDetailForService(id, category);

                        List<BillBoardModel.Advert> adverts = new List<BillBoardModel.Advert>();
                        ListItemCollection coll = QueryListItems(BillBoardModel.advertsSiteUrl, BillBoardModel.sayfalarListName, string.Format(BillBoardModel.advertDetailForServiceCamlQuery, category, id));
                        if (coll.Count > 0)
                        {
                            var item = coll[0];
                            BillBoardModel.Advert adv = new BillBoardModel.Advert();
                            adv.Category = category;
                            adv.ID = id;
                            adv.Title = item["Title"].ToString();
                            adv.Price = Convert.ToDecimal(item["Price"]);
                            adv.Created = Convert.ToDateTime(item["Created"]);
                            adv.Description = item["GenericDescription"].ToString();
                            adv.Detail = item["GenericDetail"].ToString();

                            // Retrieve Images
                            List<string> imageServerUrls = DataQuery.QueryFolderFileUrls(BillBoardModel.advertsSiteUrl, BillBoardModel.goruntulerListName, id.ToString());
                            if (imageServerUrls.Count > 0)
                            {
                                List<string> urlPaths = new List<string>();

                                foreach (var imgUrl in imageServerUrls)
                                {
                                    urlPaths.Add(HLP.ResizeAndSaveRemoteImageToLocalForService(imgUrl, 200, 100, 14, adv.GetType().Name.ToString() + id));
                                }

                                adv.ImageUrls = urlPaths;
                            }

                            adverts.Add(adv);
                        }

                        #endregion

                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        jsonString = jsonSerializer.Serialize(adverts.Take(1));

                    }
                }
                return jsonString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCampaignDetail(int id, string secretKey)
        {
            try
            {
                string jsonString = string.Empty;

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {

                        ListItemCollection coll = QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignDetailCamlQuery, id));
                        ListItem item = coll[0];
                        CampaignModel.Campaign camp = new CampaignModel.Campaign();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("PublishingPageContent:SW")) camp.PublishingPageContent = HLP.TransformHtmlStringForMobile(i.Split('|')[1].Replace("\r", ""));
                            //if (i.Contains("CampaignEndDate:SW")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                            //if (i.Contains("CampaignStartDate:SW")) camp.CampaignStartDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));

                        }
                        camp.ID = Convert.ToInt32(item["ID"]);
                        camp.Title = item["Title"].ToString();

                        //#region Prepare Campaign
                        //CampaignModel.Campaign campDetail = CampaignService.QueryCampaignDetailForService(id);
                        //jsonString = campDetail.PublishingPageContent;
                        //#endregion

                        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        jsonString = jsonSerializer.Serialize(camp);

                    }
                }

                return jsonString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public string GetActiveBanners(string secretKey)
        {
            try
            {
                string jsonString = string.Empty;

                if (secretKey != null)
                {
                    byte[] data = Convert.FromBase64String(secretKey);
                    if (DataStatics.SecretKey == System.Text.Encoding.UTF8.GetString(data))
                    {

                        #region Prepare Banner List and Images

                        List<BannerModel.Banner> banners = new List<BannerModel.Banner>();


                        ListItemCollection coll = QueryListItems(BannerModel.bannersSiteUrl, BannerModel.bannersListName, BannerModel.latestBannersCamlQuery);
                        if (coll.Count > 0)
                        {

                            foreach (ListItem item in coll)
                            {
                                BannerModel.Banner bnnr = new BannerModel.Banner();

                                string[] metaInfo = item["MetaInfo"].ToString().Split('\n');

                                foreach (var i in metaInfo)
                                {
                                    if (i.Contains("TargetFrame"))
                                    {
                                        bnnr.IsExternalLink = Convert.ToBoolean(i.Split('|')[1].Replace("\r", "")); break;
                                    };
                                }


                                bnnr.Created = Convert.ToDateTime(item["Created"]);
                                bnnr.ID = Convert.ToInt32(item["ID"]);
                                bnnr.Title = item["Title"].ToString();
                                bnnr.ImageUrl = HttpUtility.UrlEncode(item["FileLeafRef"].ToString());
                                bnnr.MobileImageUrl = HttpUtility.UrlEncode(item["FileLeafRef"].ToString());
                                var urlValue = HttpUtility.UrlEncode(((FieldUrlValue)item["URL"]).Url);
                                bnnr.LinkUrlForMobile = urlValue;
                                banners.Add(bnnr);
                            }

                            //List<BannerModel.Banner> activeBanners = BannerService.QueryLatestBannersForService();

                            foreach (var banner in banners)
                            {
                                banner.MobileImageUrl = HLP.SaveFileForMobile(HLP.GetNetworkCredential(), banner.ImageUrl, "Banner");
                            }

                            #endregion

                            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            jsonString = jsonSerializer.Serialize(banners);

                        }
                    }
                }

                return jsonString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        #region Helper Statics & Methods

        public static string categoryAdvertsLimitedCamlQuery = @"<View>
        <ViewFields><FieldRef Name='ID'/><FieldRef Name='DefaultImage'/><FieldRef Name='Title'/></ViewFields>
        <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='AdCategory'/><Value Type='Text'>{0}</Value></Eq></Where></Query>
        <RowLimit>{1}</RowLimit></View>";
        public static string advertsSiteUrl = "https://saport.x.com/tr-tr/ilanpanosu/";
        public static string sayfalarListName = "Sayfalar";
        public static string goruntulerListName = "Görüntüler";
        private static string userNameForService = "x";
        private static string passwordForService = "x";
        private static string domainForService = "x";

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


        #endregion

    }
}
