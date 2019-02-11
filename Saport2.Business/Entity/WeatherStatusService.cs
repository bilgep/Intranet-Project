using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using SP = Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

namespace Saport2.Business.Entity
{
    public class WeatherStatusService : WeatherStatusModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        public static WeatherStatus GetCityWeatherStatus(string cityName)
        {
            try
            {
                HttpWebRequest myReq8 = DAT.DataService.CreateRequest(DAT.DataStatics.DomainForService, DAT.DataStatics.UserNameForService, DAT.DataStatics.PasswordForService, DAT.DataStatics.saportServiceURL(DAT.DataStatics.saportServiceGetCityWeatherStatus), false, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "POST");
                string parsedContent8 =  "{\"CityName\":\"" + (string.IsNullOrEmpty(cityName) ? weatherDefaultCity : cityName) +"\"}";
                UTF8Encoding encoding8 = new UTF8Encoding();
                Byte[] bytes8 = encoding8.GetBytes(parsedContent8);
                myReq8.ContentLength = bytes8.Length;
                Stream newStream8 = myReq8.GetRequestStream();
                newStream8.Write(bytes8, 0, bytes8.Length);
                newStream8.Close();
                WebResponse response8 = myReq8.GetResponse();
                string responseString8 = DAT.DataService.RestfulReader(response8);

                var serializer = new JavaScriptSerializer();
                WeatherObject weatherObj = serializer.Deserialize<WeatherObject>(responseString8);
                WeatherStatus weatherStatus = weatherObj.Data;

                if (weatherStatus == null)
                {
                    weatherStatus = GetDefaultWeatherStatus();
                }

                return weatherStatus;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<WeatherStatus> QueryAllWeatherStatusesForXml()
        {
            try
            {
                List<WeatherStatus> statuses = new List<WeatherStatus>();

                foreach (var item in Enum.GetValues(typeof(WeatherCityName)))
                {

                    WeatherStatus status = item.ToString() == "İstanbul" ? GetCityWeatherStatus("istanbul") : GetCityWeatherStatus(item.ToString());
                    statuses.Add(status);


                }

                return statuses;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static void AddCityCookie(string city)
        {
            try
            {
                HttpCookie cityCookie = new HttpCookie("cityName");
                cityCookie["city"] = HttpContext.Current.Server.UrlEncode(city);
                cityCookie.Expires = DateTime.Now.AddDays(30);
                HttpContext.Current.Response.Cookies.Add(cityCookie);
            }
            catch (Exception ex )
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }

        public static bool CheckIfCityNameCookieExists()
        {
            try
            {
                bool exists = false;
                HttpCookie cookieCheck = HttpContext.Current.Request.Cookies["cityName"];
                if (cookieCheck != null) exists = true;
                return exists;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return false;
            }
        }

        public static string ReadCookieValue()
        {
            try
            {
                string cookieValue = string.Empty;

                if (HttpContext.Current.Request.Cookies["cityName"] != null)
                {
                    cookieValue = string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["cityName"].Values[0].ToString()) ? weatherDefaultCity : HttpContext.Current.Request.Cookies["cityName"].Values[0].ToString();
                }

                return cookieValue;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static WeatherStatus GetDefaultWeatherStatus()
        {
            try
            {
                WeatherStatus defaultWeatherStat = new WeatherStatus() { City = weatherDefaultCity, WeatherInfo = new WeatherInfo() { ObservedAt = "", Status = "Partly Cloudy", StatusImage = "116", Temperature = "20" } };
                return defaultWeatherStat;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string GetWeatherImageUrl(WeatherStatus weatherStat)
        {
            try
            {
                string periodOfDay = (DateTime.Now.Hour > 19 && DateTime.Now.Hour <= 23) || (DateTime.Now.Hour >=0 && DateTime.Now.Hour <= 5) ? "pm" : "am";
                return string.Format(weatherImgeUrl, periodOfDay == "am" ? "day" : "night", weatherStat.WeatherInfo.StatusImage, weatherStat.WeatherInfo.Status);
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static WeatherStatus DeserializeXMLToWeathers(string filefolder, string fileName, string cityName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<WeatherStatus> weatherStatuses = new List<WeatherStatus>();
                WeatherStatus cityWeatherStatus = new WeatherStatus();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<WeatherStatus>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    weatherStatuses = (List<WeatherStatus>)writer.Deserialize(file);
                    cityWeatherStatus = weatherStatuses.Single(x => x.City == cityName);
                    file.Close();
                }
                else
                {
                    weatherStatuses = QueryAllWeatherStatusesForXml();
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<WeatherStatus>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, weatherStatuses);
                    file.Close();
                }

                return weatherStatuses.Single(x => x.City == cityName);

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static void DeserializeXMLToWeathersForGlobalAsax(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<WeatherStatus> weatherStatuses = new List<WeatherStatus>();

                weatherStatuses = QueryAllWeatherStatusesForXml();
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<WeatherStatus>));
                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, weatherStatuses);
                file.Close();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }

        #endregion
    }
}
