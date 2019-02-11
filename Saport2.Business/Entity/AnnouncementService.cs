using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using EXP = Saport2.Shared.Exceptions;
using SP = Microsoft.SharePoint.Client;
using System.Web;

namespace Saport2.Business.Entity
{
    public class AnnouncementService : AnnouncementModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        // Retrieve Announcements
        //public static List<Announcement> QueryLatestAnnouncements()
        //{
        //    try
        //    {
        //        HttpWebRequest request = DataService.CreateRequest(DataStatics.DomainForService, DataStatics.UserNameForService, DataStatics.PasswordForService, AnnouncementModel.queryUrl , true, true, "application/json; charset=UTF-8", "application/json; odata=verbose", "GET");
        //        request.ContentLength = 0;

        //        WebResponse response = request.GetResponse();
        //        string returnString = DataService.RestfulReader(response);
        //        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Root));
        //        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(returnString));
        //        Root obj = (Root)ser.ReadObject(stream);
        //        List<Announcement> announcements = (List<Announcement>)obj.Data.Results;
        //        return announcements;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        //public static List<Announcement> QueryLatestAnnouncements()
        //{
        //    try
        //    {
        //        List<Announcement> anns = new List<Announcement>();
        //        Microsoft.SharePoint.Client.ListItemCollection coll = DAT.DataQuery.QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, AnnouncementModel.announcementsCamlQuery);
        //        if (coll.Count > 0)
        //        {
        //            foreach (SP.ListItem item in coll)
        //            {
        //                Announcement ann = new Announcement();
        //                ann.ID = Convert.ToInt32(item["ID"]);
        //                ann.Created = Convert.ToDateTime(item["Created"]);
        //                ann.Title = item["Title"].ToString();
        //                string imgUrl = HLP.GetImageUrlFromImgTag(item["ListImage"].ToString());
        //                string imagePath = imgUrl.Split('?')[0];
        //                string imageExtension = imagePath.Substring(imagePath.Length - 3);
        //                string base64 = HLP.RemoteImageUrlToBase64Converter(imgUrl);
        //                ann.ListImage = HLP.CreateImgTagWithBase64(base64,imageExtension,"width:100px;height:auto; border: 0px solid");
        //                anns.Add(ann);
        //            }
        //        }

        //        return anns;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public static List<Announcement> QueryAnnouncementsLight()
        {
            try
            {
                List<Announcement> anns = new List<Announcement>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, AnnouncementModel.announcementsLightCamlQuery);
                if (coll.Count > 0)
                {


                    foreach (SP.ListItem item in coll)
                    {
                        Announcement ann = new Announcement();
                        ann.ID = Convert.ToInt32(item["ID"]);
                        ann.Created = Convert.ToDateTime(item["Created"]);
                        ann.Title = item["Title"].ToString();
                        anns.Add(ann);
                    }
                }

                return anns;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Announcement> QueryLatestAnnouncements(int amount)
        {
            try
            {
                List<Announcement> anns = new List<Announcement>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, string.Format(AnnouncementModel.announcementsCamlQuery, amount));
                if (coll.Count > 0)
                {
                    

                    foreach (SP.ListItem item in coll)
                    {
                        Announcement ann = new Announcement();
                        ann.ID = Convert.ToInt32(item["ID"]);
                        ann.Created = Convert.ToDateTime(item["Created"]);
                        ann.Title = item["Title"].ToString();
                        //string imgUrl = HLP.GetImageUrlFromImgTag(item["ListImage"].ToString());
                        //string imgFileName = HLP.SaveFileToTempFolder(HLP.GetNetworkCredential(), DAT.DataStatics.saportHostURL+ imgUrl,  System.Web.HttpContext.Current.Session.SessionID);
                        //ann.ListImage = HLP.CreateImgTag(imgFileName);
                        ann.ListImage = HLP.TransformHtmlString(item["ListImage"].ToString());
                        anns.Add(ann);
                    }
                }

                return anns;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Announcement> QueryLatestAnnouncementsForXml(int amount)
        {
            try
            {
                List<Announcement> anns = new List<Announcement>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, string.Format(AnnouncementModel.announcementsCamlQuery, amount));
                if (coll.Count > 0)
                {


                    foreach (SP.ListItem item in coll)
                    {
                        Announcement ann = new Announcement();
                        ann.ID = Convert.ToInt32(item["ID"]);
                        ann.Created = Convert.ToDateTime(item["Created"]);
                        ann.Title = item["Title"].ToString();
                        string imgUrl = HLP.GetImageUrlFromImgTag(item["ListImage"].ToString());
                        //string imgFileName = HLP.SaveFileToTempFolder(HLP.GetNetworkCredential(), DAT.DataStatics.saportHostURL+ imgUrl,  System.Web.HttpContext.Current.Session.SessionID);
                        //ann.ListImage = HLP.CreateImgTag(imgFileName);
                        //ann.ListImage = HLP.TransformHtmlString(item["ListImage"].ToString());
                        ann.ListImage = HLP.RemoteImageUrlToBase64Converter(imgUrl, true);
                        anns.Add(ann);
                    }
                }

                return anns;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static Announcement QueryAnnouncementDetails(int announcementId)
        {
            try
            {
                Announcement ann = new Announcement();
                Microsoft.SharePoint.Client.ListItemCollection coll = DAT.DataQuery.QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, string.Format(AnnouncementModel.announcementDetailCamlQuery, announcementId));
                SP.ListItem item = coll[0];
                ann.ID = announcementId;
                ann.Created = Convert.ToDateTime(item["Created"]);
                ann.Title = item["Title"].ToString();
                ann.PublishingPageContent = HLP.TransformHtmlString(item["PublishingPageContent"].ToString());
                return ann;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static int? QueryAnnouncementDetailsForBanner(string announcementPage)
        {
            try
            {
                Announcement ann = new Announcement();
                Microsoft.SharePoint.Client.ListItemCollection coll = DAT.DataQuery.QueryListItems(AnnouncementModel.announcementsSiteUrl, AnnouncementModel.sayfalarListName, string.Format(AnnouncementModel.announcementDetailForBannerCamlQuery, announcementPage));
                SP.ListItem item = coll[0];
                ann.ID = Convert.ToInt32(item["ID"]);
                return ann.ID;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Announcement> DeserializeXMLToAnnsLight(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Announcement> annsLight = new List<Announcement>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Announcement>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    annsLight = (List<Announcement>)writer.Deserialize(file);
                    annsLight = annsLight.Take(amount).ToList();
                    file.Close();
                }
                else
                {
                    annsLight = QueryLatestAnnouncementsForXml(amount);
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Announcement>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, annsLight);
                    file.Close();
                }

                return annsLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static void DeserializeXMLToAnnouncementsForGlobalAsax(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Announcement> anns = new List<Announcement>();
                anns = QueryLatestAnnouncementsForXml(100);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Announcement>));

                HLP.DeleteFile(filefolder, fileName);

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, anns);
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
