using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using SP = Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;
using System.IO;

namespace Saport2.Business.Entity
{
    public class BannerService : BannerModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        public static List<Banner> QueryLatestBanners()
        {
            try
            {
                List<Banner> banners = new List<Banner>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BannerModel.bannersSiteUrl, BannerModel.bannersListName, BannerModel.latestBannersCamlQuery);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Banner bnnr = new Banner();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("TargetFrame")) { bnnr.IsExternalLink = Convert.ToBoolean(i.Split('|')[1].Replace("\r", "")); break; };
                        }
                        bnnr.Created = Convert.ToDateTime(item["Created"]);
                        bnnr.ID = Convert.ToInt32(item["ID"]);
                        bnnr.Title = item["Title"].ToString();
                        bnnr.ImageUrl = item["FileLeafRef"].ToString();
                        var linkUrl_ = (SP.FieldUrlValue)item["URL"];
                        bnnr.LinkUrl = linkUrl_.Url ;
                        banners.Add(bnnr);
                    }
                }

                return banners;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Banner> QueryLatestBannersForService()
        {
            try
            {
                List<Banner> banners = new List<Banner>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BannerModel.bannersSiteUrl, BannerModel.bannersListName, BannerModel.latestBannersCamlQuery);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Banner bnnr = new Banner();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("TargetFrame")) { bnnr.IsExternalLink = Convert.ToBoolean(i.Split('|')[1].Replace("\r", "")); break; };
                        }
                        bnnr.Created = Convert.ToDateTime(item["Created"]);
                        bnnr.ID = Convert.ToInt32(item["ID"]);
                        bnnr.Title = item["Title"].ToString();
                        bnnr.ImageUrl = item["FileLeafRef"].ToString();
                        bnnr.MobileImageUrl = item["FileLeafRef"].ToString();
                        var linkUrl_ = (SP.FieldUrlValue)item["URL"];
                        bnnr.LinkUrl = linkUrl_.Url;
                        banners.Add(bnnr);
                    }
                }

                return banners;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        //public static string WrapHomePageBanners(string redirectPath, string bannerImageVirtualPath)
        //{
        //    try
        //    {
        //        //string wrapTextForHomePage = "<div><a href=\"{0}\"><img src=\"data:image/png:base64,{1}\" width=\"0\" height=\"0\" border=\"0\" alt=\"\"/></a></div>";
        //        string wrapText = "<div><a href=\"{0}\"><img src=\"{1}\" width=\"0\" height=\"0\" border=\"0\" alt=\"\"/></a></div>";
        //        return string.Format(wrapText, redirectPath, bannerImageVirtualPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        EXP.RedirectToErrorPage(ex.Message);
        //        return null;
        //    }
        //}

        public static List<Banner> QueryLatestBannersForXml(int amount)
        {
            try
            {
                List<Banner> banners = new List<Banner>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BannerModel.bannersSiteUrl, BannerModel.bannersListName, BannerModel.latestBannersCamlQuery);
                if (coll.Count > 0)
                {
                    

                    foreach (SP.ListItem item in coll)
                    {
                        Banner bnnr = new Banner();

                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("TargetFrame"))
                            {
                                bnnr.IsExternalLink = Convert.ToBoolean(i.Split('|')[1].Replace("\r", "")); break; };
                        }
                        bnnr.ID = Convert.ToInt32(item["ID"]);
                        bnnr.Title = item["Title"].ToString();
                        bnnr.ImageUrl = item["FileLeafRef"].ToString();
                        var linkUrl_ = (SP.FieldUrlValue)item["URL"];
                        string linkUrl__ = Convert.ToString(linkUrl_.Url);
                        if (linkUrl__.Contains(DAT.DataStatics.saportHostURL)) bnnr.IsExternalLink = false;
                        bnnr.LinkUrl = linkUrl_.Url;
                        banners.Add(bnnr);
                    }
                }

                return banners;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Banner> DeserializeXMLToBannersLight(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Banner> bannersLight = new List<Banner>();
                DateTime fileCreationDate = System.IO.File.GetCreationTime(fileLocation);
                bool isOld = fileCreationDate < DateTime.Now.AddDays(-1) ? true : false;
                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Banner>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    bannersLight = (List<Banner>)writer.Deserialize(file);
                    bannersLight = bannersLight.Take(amount).ToList();
                    file.Close();
                }
                else
                {
                    bannersLight = QueryLatestBannersForXml(amount);
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Banner>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, bannersLight);
                    file.Close();
                }

                return bannersLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Banner> DeserializeXMLToBannersLightForGlobalAsax(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Banner> bannersLight = new List<Banner>();

                bannersLight = QueryLatestBannersForXml(amount);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Banner>));

                HLP.DeleteFile(filefolder, fileName);

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, bannersLight);
                file.Close();

                return bannersLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        //public static string GetMobileImageUrl(string imageUrl, string context)
        //{
        //    try
        //    {
        //        imageUrl = bannersListUrl + imageUrl;
        //        imageUrl = "/mobile/" + HLP.SaveFileForMobile(HLP.GetNetworkCredential(), imageUrl, context );
        //        return imageUrl;
        //    }
        //    catch (Exception ex)
        //    {
        //        EXP.RedirectToErrorPage(ex.Message);
        //        return null;
        //    }
        //}

        public static string GetImageUrl(string imageUrl)
        {
            try
            {
                 //imageUrl = "/temp/" + HLP.SaveFileToTempFolderForBanners(HLP.GetNetworkCredential(), imageUrl);
                 imageUrl = HLP.RemoteImageUrlToBase64Converter(imageUrl, false);
                return imageUrl;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string GetRemoteImageAsBase64(string imageUrl)
        {
            try
            {
                string imageBase64 = HLP.RemoteImageUrlToBase64Converter(imageUrl, false);
                return imageBase64;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string SaveRemoteImageToTemp(string imageUrl)
        {
            try
            {
                string imageBase64 = HLP.RemoteImageUrlToBase64Converter(imageUrl, false);
                return imageBase64;
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
