using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Saport2.Business.Entity;
using System.ServiceModel.Web;


namespace Saport2.Service
{

    [ServiceContract]
    public interface ISaport2Service
    {

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/GetNavigationJson",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        List<NavModel> GetNavigationJson();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/CheckRemoteService",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string CheckRemoteService(string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetCityWeatherStatus",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetCityWeatherStatus(string cityName);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/GetLatestExchangeRates",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetLatestExchangeRates();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetLatestBlogPosts",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetLatestBlogPosts(string lastPostDate, int itemCount, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetSocialFeeds",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetSocialFeeds(string newestDate, int itemCount, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetLatestAnnouncements",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetLatestAnnouncements(string newestDate, int itemCount, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetLatestNews",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetLatestNews(string newestDate, int itemCount, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetNewsDetail",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetNewsDetail(int id, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetPortalFeeds",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetPortalFeeds(string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetEventList",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        string GetEventList(string includeDate, int count, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetActiveBanners",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
        )]
        string GetActiveBanners(string secretKey);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetLatestCampaigns",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
        )]
        string GetLatestCampaigns(int amount, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetLatestAdverts",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
        )]
        string GetLatestAdverts(int amount, string category, string secretKey);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetAdvertDetail",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
        )]
        string GetAdvertDetail(int id, string category, string secretKey);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/GetCampaignDetail",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
        )]
        string GetCampaignDetail(int id, string secretKey);

    }
}
