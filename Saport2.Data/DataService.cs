using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Saport2.Data
{
    public class DataService
    {
        /* It includes methods and helpers about retreival of data */

        #region Methods and Helpers

            /// <summary>
            /// It checks if the service is up with parameters: domain, username, password, serviceURL etc.
            /// </summary>
            /// <param name="domain"></param>
            /// <param name="userName"></param>
            /// <param name="password"></param>
            /// <param name="testServiceURL"></param>
            /// <param name="useDefaultCredentials"></param>
            /// <param name="preAuthenticate"></param>
            /// <param name="contentLenght"></param>
            /// <param name="contentType"></param>
            /// <returns></returns>
        public static bool IsServiceUp(string domain, string userName, string password, string testServiceURL, bool useDefaultCredentials, bool preAuthenticate, int contentLenght, string contentType)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testServiceURL);
                request.Credentials = new NetworkCredential (userName, password, domain);
                request.PreAuthenticate = preAuthenticate;
                request.ContentLength = contentLenght;
                request.ContentType = contentType;
                HttpWebResponse testResponse = (HttpWebResponse)request.GetResponse();
                bool status = testResponse.StatusCode == HttpStatusCode.OK ? true : false;
                return status;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// It creates a HttpWebRequest with parameters: domain, userName, password, serviceURL etc.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="serviceURL"></param>
        /// <param name="useDefaultCredentials"></param>
        /// <param name="preAuthenticate"></param>
        /// <param name="contentLenght"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(string domain, string userName, string password, string serviceURL, bool useDefaultCredentials, bool preAuthenticate, string contentType, string accept, string method)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceURL);
                request.Credentials = new NetworkCredential(userName, password, domain);
                request.KeepAlive = false;
                request.PreAuthenticate = preAuthenticate;
                request.AllowAutoRedirect = false; // sonradan eklendi
                request.Method = method;
                request.Accept = accept;
                request.ContentType = contentType;
                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// It reads WebResponse and returns string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string RestfulReader(WebResponse response)
        {
            try
            {
                System.IO.Stream stream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(stream); 
                return reader.ReadToEnd();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static NetworkCredential GetNetworkCredential()
        {
            try
            {
                NetworkCredential credential = new NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                return credential;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
