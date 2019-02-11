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
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Saport2.Business.Entity
{
    public class CampaignService :  CampaignModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        public static List<Campaign> QueryLatestCampaignsLight()
        {
            try
            {
                List<Campaign> camps = new List<Campaign>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, CampaignModel.campaignsLightCamlQuery);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Campaign camp = new Campaign();

                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                        }

                        if (camp.CampaignEndDate >= DateTime.Now)
                        {
                            camp.Created = Convert.ToDateTime(item["Created"]);
                            camp.ID = Convert.ToInt32(item["ID"]);
                            camp.Title = item["Title"].ToString();
                            camps.Add(camp);
                        }
                    }
                }

                return camps;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Campaign> QueryLatestCampaigns()
        {
            try
            {
                List<Campaign> camps = new List<Campaign>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, CampaignModel.campaignsCamlQuery);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Campaign camp = new Campaign();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                            if (i.Contains("ListImage"))
                            {
                                if (camp.CampaignEndDate != null && camp.CampaignEndDate >= DateTime.Now)
                                {
                                    camp.ListImage = HLP.TransformHtmlString(i.Split('|')[1].Replace("\r", ""));
                                    break;
                                }
                            }
                        }
                        if (camp.CampaignEndDate >= DateTime.Now)
                        {
                            camp.ID = Convert.ToInt32(item["ID"]);
                            camp.Title = item["Title"].ToString();
                            camps.Add(camp);
                        }
                    }
                }

                return camps;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Campaign> QueryLatestCampaigns(int amount)
        {
            try
            {
                List<Campaign> camps = new List<Campaign>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, CampaignModel.campaignsCamlQueryForHomePage);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Campaign camp = new Campaign();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                            if (i.Contains("ListImage"))
                            {
                                if (camp.CampaignEndDate != null && camp.CampaignEndDate >= DateTime.Now)
                                {
                                    camp.ListImage = "../temp/" + HLP.TransformHtmlStringAndGetFileName(i.Split('|')[1].Replace("\r", ""));
                                    break;
                                }
                            }
                        }
                        if (camp.CampaignEndDate >= DateTime.Now)
                        {
                            camp.ID = Convert.ToInt32(item["ID"]);
                            camp.Title = item["Title"].ToString();
                            camps.Add(camp);
                        }
                    }
                }

                return camps.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Campaign> QueryLastSixCampaigns()
        {
            try
            {
                List<Campaign> camps = new List<Campaign>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, CampaignModel.campaignsCamlQuery6);
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Campaign camp = new Campaign();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                            if (i.Contains("ListImage"))
                            {
                                camp.ListImage = "../temp/" + HLP.TransformHtmlStringAndGetFileName(i.Split('|')[1].Replace("\r", ""));
                            }
                        }

                        camp.ID = Convert.ToInt32(item["ID"]);
                        camp.Title = item["Title"].ToString();
                        camps.Add(camp);

                    }
                }

                return camps;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Campaign> QueryLatestCampaignsForService(int amount)
        {
            try
            {
                List<Campaign> camps = new List<Campaign>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignsCamlQueryForService, amount*3));
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Campaign camp = new Campaign();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("PublishingPageContent:SW"))
                            {
                                camp.PublishingPageContent = HLP.TransformHtmlStringForMobile(i.Split('|')[1].Replace("\r", ""));
                            }
                            if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                            if (i.Contains("ListImage"))
                            {
                                if (camp.CampaignEndDate != null && camp.CampaignEndDate >= DateTime.Now)
                                {
                                    camp.ListImage = DAT.DataStatics.saportHostURL + HLP.TransformHtmlStringAndGetFileUrl(i.Split('|')[1].Replace("\r", ""));
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
                return camps.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static Campaign QueryCampaignDetails(int campaignId)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }; // To avouid SSL Security - Certificate Error

                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignDetailCamlQuery, campaignId));
                SP.ListItem item = coll[0];
                Campaign camp = new Campaign();
                string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                foreach (var i in metaInfo)
                {
                    if (i.Contains("vti_cachedcustomprops")) continue;
                    if (i.Contains("PublishingPageContent:SW")) camp.PublishingPageContent = HLP.TransformHtmlString(i.Split('|')[1].Replace("\r", ""));
                    if (i.Contains("CampaignEndDate:SW")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                    if (i.Contains("CampaignStartDate:SW")) camp.CampaignStartDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));

                }
                camp.ID = Convert.ToInt32(item["ID"]);
                camp.Title = item["Title"].ToString();
                camp.PublishingPageContent = HttpUtility.HtmlDecode(camp.PublishingPageContent);

                return camp;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static Campaign QueryCampaignDetailForService(int campaignId)
        {
            try
            {
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignDetailCamlQuery, campaignId));
                SP.ListItem item = coll[0];
                Campaign camp = new Campaign();
                string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                foreach (var i in metaInfo)
                {
                    if (i.Contains("vti_cachedcustomprops")) continue;
                    if (i.Contains("PublishingPageContent:SW")) camp.PublishingPageContent = HLP.TransformHtmlStringForMobile(i.Split('|')[1].Replace("\r", ""));
                    if (i.Contains("CampaignEndDate:SW")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                    if (i.Contains("CampaignStartDate:SW")) camp.CampaignStartDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));

                }
                camp.ID = Convert.ToInt32(item["ID"]);
                camp.Title = item["Title"].ToString();
                return camp;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static int? QueryCampaignDetailsForBanner(string campaignPageName)
        {
            try
            {
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignDetailForBannerCamlQuery, campaignPageName));
                SP.ListItem item = coll[0];
                Campaign camp = new Campaign();
                camp.ID = Convert.ToInt32(item["ID"]);
                return camp.ID;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Campaign> QueryLatestCampaignsForXml(int amount)
        {
            try
            {
                List<Campaign> camps = new List<Campaign>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignsLimitedCamlQuery, amount*3));
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Campaign camp = new Campaign();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                            if (i.Contains("ListImage"))
                            {
                                if(camp.CampaignEndDate != null && camp.CampaignEndDate >= DateTime.Now)
                                {
                                    camp.ListImage = HLP.TransformImgHtmlStringAndGetBase64(i.Split('|')[1].Replace("\r", ""));
                                    //camp.ListImage = "../temp/" + HLP.TransformHtmlStringAndGetFileName(i.Split('|')[1].Replace("\r", ""));
                                    break;
                                }
                            }
                        }
                        if (camp.CampaignEndDate >= DateTime.Now)
                        {
                            camp.ID = Convert.ToInt32(item["ID"]);
                            camp.Title = item["Title"].ToString();
                            camps.Add(camp);
                        } 
                        
                    }
                }

                return camps.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Campaign> QueryAllCampaignsForXml(int amount)
        {
            try
            {
                List<Campaign> camps = new List<Campaign>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(CampaignModel.campaignsSiteUrl, CampaignModel.sayfalarListName, string.Format(CampaignModel.campaignsLimitedCamlQuery, amount * 3));
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Campaign camp = new Campaign();
                        string[] metaInfo = item["MetaInfo"].ToString().Split('\n');
                        foreach (var i in metaInfo)
                        {
                            if (i.Contains("vti_cachedcustomprops")) continue;
                            if (i.Contains("CampaignEndDate")) camp.CampaignEndDate = Convert.ToDateTime(i.Split('|')[1].Replace("\r", ""));
                            if (i.Contains("ListImage"))
                            {

                                    camp.ListImage = HLP.TransformImgHtmlStringAndGetBase64(i.Split('|')[1].Replace("\r", ""));
                                    //camp.ListImage = "../temp/" + HLP.TransformHtmlStringAndGetFileName(i.Split('|')[1].Replace("\r", ""));
                                    break;
                            }
                        }
                            camp.ID = Convert.ToInt32(item["ID"]);
                            camp.Title = item["Title"].ToString();
                            camps.Add(camp);

                    }
                }

                return camps.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Campaign> DeserializeXMLToCampsLight(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Campaign> campsLight = new List<Campaign>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Campaign>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    campsLight = (List<Campaign>)writer.Deserialize(file);
                    campsLight = campsLight.Take(amount).ToList();
                    file.Close();
                }
                else
                {
                    campsLight = QueryLatestCampaignsForXml(amount*10);
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Campaign>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, campsLight);
                    file.Close();
                }

                return campsLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Campaign> DeserializeXMLToCampsLightForGloalAsax(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Campaign> campsLight = new List<Campaign>();

                campsLight = QueryLatestCampaignsForXml(amount);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Campaign>));

                HLP.DeleteFile(filefolder, fileName);

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, campsLight);
                file.Close();

                return campsLight.Take(amount).ToList();
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
