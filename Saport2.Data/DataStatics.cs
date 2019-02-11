using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saport2.Data
{
    public class DataStatics
    {
        /* It includes: URL addresses of Service locations, Access informations etc. */

        #region Statics and Enums
        // SAPORT Service Connection Info

        private static string userNameForService = "x";
        private static string passwordForService = "x";
        private static string domainForService = "x";
        public static string UserNameForService { get { return userNameForService; } }
        public static string PasswordForService { get { return passwordForService; } }
        public static string DomainForService { get { return domainForService; } }

        // LDAP Info
        //private static string ldapHost = "";
        //private static string ldapPort = "";
        //private static string userNameForLdap = "";
        //private static string passwordForLdap = "";


        // Secret Key For Web - Mobile Integration
        private static string secretKey = "x";
        public static string SecretKey { get { return secretKey; } }

        
        // Service and Host URLs
        public static string saportHostURL = "https://X.com";
        public static string saporttestHostURL = "http://X.com";
        static string saportServicePath = "/_vti_bin/XPortal/PortalService.svc/";
        public static string saportServiceURL(string serviceName) => saportHostURL + saportServicePath + serviceName;
        public static string saporttestServiceURL(string serviceName) => saporttestHostURL + saportServicePath + serviceName;


        // Service Names
        public static string saportServiceTest = "Test";
        public static string saportServiceGetLatestExchangeRates = "GetLatestExchangeRates";
        public static string saportServiceGetMenuItems = "GetMenuItems";
        public static string saportServiceGetCityWeatherStatus = "GetCityWeatherStatus";
        public static string saportServiceGetFeeds = "GetFeeds";
        public static string saportServiceGetLatestBlogPosts = "GetLatestBlogPosts";
        public static string saportServiceGetLatestNews = "GetLatestNews";
        public static string saportServiceGetLatestAnnouncements = "GetLatestAnnouncements";
        public static string saportServiceGetEventList = "GetEventList";
        public static string saportServiceGetSocialFeeds = "GetSocialFeeds";
        public static string saportServiceGetCommunityCompaniesList = "GetCommunityCompaniesList";



        #endregion

    }
}
