using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using EXP = Saport2.Shared.Exceptions;
using System.Text.RegularExpressions;

namespace Saport2.Business.Entity
{
    public class NewsService : NewsModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        public static List<News> QueryLatestNewsLight()
        {
            try
            {
                List<News> newss = new List<News>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, NewsModel.newsLightCamlQuery);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        News news = new News();
                        news.ID = Convert.ToInt32(item["ID"]);
                        news.Created = Convert.ToDateTime(item["Created"]);
                        news.Title = item["Title"].ToString();
                        newss.Add(news);
                    }
                }

                return newss;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<News> QueryLatestNews()
        {
            try
            {
                List<News> newss = new List<News>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, NewsModel.newsCamlQuery);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        News news = new News();
                        news.ID = Convert.ToInt32(item["ID"]);
                        news.Created = Convert.ToDateTime(item["Created"]);
                        news.Title = item["Title"].ToString();
                        news.ListImage = HLP.TransformHtmlString(item["ListImage"].ToString());
                        newss.Add(news);
                    }
                }

                return newss;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static News QueryNewsDetails(int newsId)
        {
            try
            {
                News news = new News();
                Microsoft.SharePoint.Client.ListItemCollection coll = DAT.DataQuery.QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, string.Format(NewsModel.newsDetailCamlQuery, newsId));
                SP.ListItem item = coll[0];
                news.ID = newsId;
                news.Modified = Convert.ToDateTime(item["Modified"]);
                news.Title = item["Title"].ToString();
                string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                foreach (var i in metaInfo)
                {
                    if (i.Contains("vti_cachedcustomprops")) continue;
                    if (i.Contains("PublishingPageContent:SW")) news.PublishingPageContent = HLP.TransformHtmlString(i.Split('|')[1].Replace("\r", ""));

                }
                return news;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static int? QueryNewsDetailsForBanner(string newsPageName)
        {
            try
            {
                News news = new News();
                Microsoft.SharePoint.Client.ListItemCollection coll = DAT.DataQuery.QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, string.Format(NewsModel.newsDetailForBannerCamlQuery, newsPageName));
                SP.ListItem item = coll[0];
                news.ID = Convert.ToInt32(item["ID"]);
                return news.ID;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<News> QueryLatestNewsForXml(int amount)
        {
            try
            {
                List<News> newss = new List<News>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(NewsModel.newsSiteUrl, NewsModel.sayfalarListName, string.Format(NewsModel.newsLimitedCamlQuery, amount));
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        News news = new News();
                        news.ID = Convert.ToInt32(item["ID"]);
                        news.Created = Convert.ToDateTime(item["Created"]);
                        news.Title = item["Title"].ToString().Replace("\u0003"," ");
                        news.ListImage = HLP.TransformImgHtmlStringAndGetBase64(item["ListImage"].ToString());
                        newss.Add(news);
                    }
                }

                return newss.Take(amount).ToList();
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<News> DeserializeXMLToNewsLight(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<News> newsLight = new List<News>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<News>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    newsLight = (List<News>)writer.Deserialize(file);
                    newsLight = newsLight.Take(amount).ToList();
                    file.Close();
                }
                else
                {
                    newsLight = QueryLatestNewsForXml(50); 
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<News>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, newsLight);
                    file.Close();
                }

                return newsLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<News> DeserializeXMLToNewsLightForGlobalAsax(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<News> newsLight = new List<News>();

                newsLight = QueryLatestNewsForXml(amount);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<News>));

                HLP.DeleteFile(filefolder, fileName);

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, newsLight);
                file.Close();

                return newsLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
