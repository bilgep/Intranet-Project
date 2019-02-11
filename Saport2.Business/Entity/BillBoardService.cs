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

namespace Saport2.Business.Entity
{
    public class BillBoardService : BillBoardModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        public static List<Advert> GetCategoryAdverts(string categoryName)
        {
            try
            {
                List<Advert> ads = new List<Advert>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BillBoardModel.advertsSiteUrl, BillBoardModel.sayfalarListName, string.Format(BillBoardModel.categoryAdvertsCamlQuery, categoryName));
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Advert adv = new Advert();
                        adv.ID = Convert.ToInt32(item["ID"]);
                        adv.Category = categoryName;
                        adv.Created = Convert.ToDateTime(item["Created"]);
                        adv.DefaultImage = item["DefaultImage"] != null ? item["DefaultImage"].ToString() : "";
                        adv.Title = item["Title"].ToString();
                        ads.Add(adv);
                    }
                }

                return ads;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Advert> QueryLatestCategoryAdvertsForXml(int amount, string category)
        {
            try
            {
                List<Advert> adverts = new List<Advert>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BillBoardModel.advertsSiteUrl, BillBoardModel.sayfalarListName, string.Format(BillBoardModel.categoryAdvertsLimitedCamlQuery, category, amount));
                if (coll.Count > 0)
                { 
                    foreach (SP.ListItem item in coll)
                    {
                        Advert adv = new Advert();

                        adv.Category = category;
                        adv.ID = Convert.ToInt32(item["ID"]);
                        adv.Title = item["Title"].ToString();
                        string imgUrl = item["DefaultImage"] != null ?  item["DefaultImage"].ToString() : "";

                        if (imgUrl == string.Empty)
                        {
                            adv.DefaultImage = BillBoardModel.defaultImageBase64;
                        }
                        else if (imgUrl.Contains("/tr-tr/ilanpanosu/") && !imgUrl.Contains("x.com"))
                        {
                            adv.DefaultImage =  HLP.RemoteImageUrlToBase64Converter(Data.DataStatics.saportHostURL + imgUrl, true);
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
                }

                return adverts;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Advert> QueryCategoryAdvertDetailsForService(int id, string category)
        {
            try
            {
                List<Advert> adverts = new List<Advert>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BillBoardModel.advertsSiteUrl, BillBoardModel.sayfalarListName, string.Format(BillBoardModel.advertDetailForServiceCamlQuery, category, id));
                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Advert adv = new Advert();

                        adv.Category = category;
                        adv.ID = Convert.ToInt32(item["ID"]);
                        adv.Title = item["Title"].ToString();
                        adv.Price = Convert.ToDecimal(item["Price"]);
                        adv.Created = Convert.ToDateTime(item["Created"]);
                        adv.Description = item["GenericDescription"].ToString();
                        adv.Detail = item["GenericDetail"].ToString();
                        adverts.Add(adv);
                    }
                }

                return adverts;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }




        public static List<Advert> QueryAdvertDetails(int id)
        {
            try
            {
                List<Advert> adverts = new List<Advert>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BillBoardModel.advertsSiteUrl, BillBoardModel.sayfalarListName,string.Format( BillBoardModel.advertDetailCamlQuery, id));
                if (coll.Count > 0)
                {
                    var item = coll[0];
                    Advert adv = new Advert();
                    adv.Category = item["AdCategory"].ToString();
                    adv.ID = id;
                    adv.Title = item["Title"].ToString();
                    adv.Price = Convert.ToDecimal(item["Price"]);
                    adv.Created = Convert.ToDateTime(item["Created"]);
                    adv.Description = item["GenericDescription"].ToString();
                    adv.Detail = item["GenericDetail"].ToString();

                    adverts.Add(adv);
                }

                return adverts;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Advert> QueryAdvertDetailForService(int id, string category)
        {
            try
            {
                List<Advert> adverts = new List<Advert>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(BillBoardModel.advertsSiteUrl, BillBoardModel.sayfalarListName, string.Format(BillBoardModel.advertDetailForServiceCamlQuery, category, id));
                if (coll.Count > 0)
                {
                    var item = coll[0];
                    Advert adv = new Advert();
                    adv.Category = category;
                    adv.ID = id;
                    adv.Title = item["Title"].ToString();
                    adv.Price = Convert.ToDecimal(item["Price"]);
                    adv.Created = Convert.ToDateTime(item["Created"]);
                    adv.Description = item["GenericDescription"].ToString();
                    adv.Detail = item["GenericDetail"].ToString();

                    // Retrieve Images
                    List<string> imageServerUrls = DAT.DataQuery.QueryFolderFileUrls(BillBoardModel.advertsSiteUrl, BillBoardModel.goruntulerListName, id.ToString());
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

                return adverts;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Advert> DeserializeXMLToAdvertsLight(string filefolder, string fileName, int amount, string category) 
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Advert> advsLight = new List<Advert>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Advert>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    advsLight = (List<Advert>)writer.Deserialize(file);
                    file.Close();
                }
                else
                {
                    advsLight = QueryLatestCategoryAdvertsForXml(amount, category);
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Advert>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, advsLight);
                    file.Close();
                }

                return advsLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static void DeserializeXMLToAdvertsForGlobalAsax(string filefolder, string fileName, int amount, string category)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Advert> advsLight = new List<Advert>();
                advsLight = QueryLatestCategoryAdvertsForXml(amount, category);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Advert>));

                if (System.IO.File.Exists(fileLocation))
                {
                    HLP.DeleteFile(filefolder, fileName);
                }

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, advsLight);
                file.Close();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }


        public static string GetSaveFileNameForGlobalAsax(string category)
        {
            try
            {
                string fileName = string.Empty;
                switch (category)
                {
                    case "Diğer":
                        fileName = advertsSaveFileNameForCategoryDiger;
                        break;
                    case "Elektronik":
                        fileName = advertsSaveFileNameForCategoryElektronik;
                        break;
                    case "Emlak":
                        fileName = advertsSaveFileNameForCategoryEmlak;
                        break;
                    case "Kişisel Ürünler":
                        fileName = advertsSaveFileNameForCategoryKisiselUrunler;
                        break;
                    case "Vasıta":
                        fileName = advertsSaveFileNameForCategoryVasita;
                        break;
                }

                return fileName;
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
