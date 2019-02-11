using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Saport2.Data;
using System.IO;
using HtmlAgilityPack;
using System.Web;
using EXP = Saport2.Shared.Exceptions;
using System.Globalization;

namespace Saport2.Shared
{
    public class Helpers
    {
        public static object HLP { get; private set; }

        public static NetworkCredential GetNetworkCredential()
        {
            try
            {
                NetworkCredential credential = new NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                return credential;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public DateTime JsonToDateTime(string jsonDate)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                jsonDate = "[\"" + jsonDate.Replace("/", "\\/") + "\"]";
                DateTime dt = ser.Deserialize<DateTime[]>(jsonDate).Single();
                return dt;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return DateTime.MinValue;
            }
        }

        public string DateTimeToJson(string dateTime)
        {
            try
            {
                var jsonDate = new JavaScriptSerializer().Serialize(dateTime);
                return jsonDate;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string GetImageUrlFromImgTag(string imgTagString)
        {
            try
            {
                string matchString = Regex.Match(imgTagString, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                return matchString;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string CreateImgTagWithBase64(string base64String, string imageType, string styleString)
        {
            try
            {
                string imgTag = "<img src=\"data:image/" + imageType + ";base64," + base64String + "\" style=\"" + styleString + "\"/>";
                return imgTag;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string RemoteImageUrlToBase64Converter(string imagePath, bool isThumbnail)
        {
            try
            {
                string base64ImageRepresentation = string.Empty;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    imagePath = imagePath.Split('?')[0];
                    if (!imagePath.Contains("x.com")) imagePath = DataStatics.saportHostURL + "/" + imagePath;

                    var webClient = new WebClient();
                    NetworkCredential credential = new NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                    webClient.Credentials = credential;

                    bool exists = false;
                    HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(imagePath);
                    request.Credentials = credential;


                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode == HttpStatusCode.OK || response != null) exists = true;
                    }
                    catch (WebException ex)
                    {
                        exists = false;
                    }

                    if (exists)
                    { 
                        byte[] imageBytes = webClient.DownloadData(imagePath);
                        if (isThumbnail)
                        {
                            System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(imageBytes);
                            System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                            System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(140, 140, null, IntPtr.Zero);
                            System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                            newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);
                            imageBytes = myResult.ToArray();
                        }
                        base64ImageRepresentation = Convert.ToBase64String(imageBytes);
                    }
                    
                    webClient.Dispose();
                    request.Abort();
                }
                
                return base64ImageRepresentation;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }

        }

        public static string ResizeAndSaveRemoteImageToLocal(string imagePath, string sessionId, bool deleteAfterSessionEnds, int width, int height, int renditionId)
        {
            try
            {
                if (!imagePath.Contains("x.com")) imagePath = DataStatics.saportHostURL + "/" + imagePath;

                imagePath = imagePath.Contains("?") ? HttpUtility.HtmlDecode(imagePath) : imagePath + "?RenditionID=" + renditionId;
                Uri uri = new Uri(imagePath);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);

                fileName = imagePath.Contains("?") ? "REN" + DateTime.Now.ToLongTimeString().Replace(":", "") + DateTime.Now.Millisecond + sessionId + fileName : sessionId + fileName;

                var webClient = new WebClient();
                NetworkCredential credential = new NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                webClient.Credentials = credential;
                byte[] imageBytes = webClient.DownloadData(imagePath);

                System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(imageBytes);
                System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);
                imageBytes = myResult.ToArray();

                string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + fileName;

                if (!File.Exists(fileSavedPath))
                {
                    using (webClient)
                    {
                        byte[] fileBytes = webClient.DownloadData(imagePath);

                        File.WriteAllBytes(fileSavedPath, fileBytes);

                        if (deleteAfterSessionEnds) System.Web.HttpContext.Current.Session[fileName] = fileName; 
                    }
                }
                webClient.Dispose();
                return "../temp/" + fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string ResizeAndSaveRemoteImageToLocalForService(string imagePath, int width, int height, int renditionId, string context)
        {
            try
            {
                if (!imagePath.Contains("x.com")) imagePath = DataStatics.saportHostURL + "/" + imagePath;

                imagePath = imagePath.Contains("?") ? HttpUtility.HtmlDecode(imagePath) : imagePath + "?RenditionID=" + renditionId;
                Uri uri = new Uri(imagePath);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);

                fileName = imagePath.Contains("?") ? "REN" + context + fileName : context + fileName;


                var webClient = new WebClient();
                NetworkCredential credential = new NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                webClient.Credentials = credential;
                byte[] imageBytes = webClient.DownloadData(imagePath);

                System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(imageBytes);
                System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);
                imageBytes = myResult.ToArray();

                string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/mobile/") + fileName;

                byte[] fileBytes = webClient.DownloadData(imagePath);

                File.WriteAllBytes(fileSavedPath, imageBytes);

                webClient.Dispose();
                return "../mobile/" + fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static string CreateObjectTagWithBase64(string base64String, string fileType)
        {
            try
            {
                string objectTag = "<object data=\"data: application / pdf; base64," + base64String + "\" type=\"application / pdf\" width=\"100 % \" height=\"100 % \"></object>";
                return objectTag;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string RemotePdfUrlToBase64Converter(string pdfPath)
        {
            try
            {
                pdfPath = DataStatics.saportHostURL + "/" + pdfPath;
                var webClient = new WebClient();
                NetworkCredential credential = new NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                webClient.Credentials = credential;
                byte[] imageBytes = webClient.DownloadData(pdfPath);
                string base64ImageRepresentation = Convert.ToBase64String(imageBytes);
                webClient.Dispose();
                return base64ImageRepresentation;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string AddRootWebToSrc(string urlString)
        {
            try
            {
                string edited = "";
                edited = urlString.Replace("src=\"/tr-tr/", "src=\"https://saport.x.com/tr-tr/");
                edited = edited.Replace("src=\"/_layouts/", "src=\"https://saport.x.com/_layouts/");
                return edited;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        //public static string SaveFileToTempFile(NetworkCredential credential, string fileName, string remoteFileUrl)
        //{
        //    try
        //    {
        //        // TODO Eğer file varsa üzerine yazılacak !!!! kontrol yapıalcak
        //        string savedFilePath = System.IO.Path.GetTempPath() + fileName;
        //        WebClient client = new WebClient();
        //        client.Credentials = credential;
        //        using (client)
        //        {
        //            byte[] fileBytes = client.DownloadData(remoteFileUrl);
        //            File.WriteAllBytes(savedFilePath, fileBytes);
        //        }

        //        return savedFilePath;
        //    }
        //    catch (System.Exception)
        //    {

        //        throw;
        //    }
        //}

        public static string SaveFileToTempFolder(NetworkCredential credential, string remoteFileUrl, string sessionId, bool deleteAfterSessionEnds)
        {
            try
            {
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);

                fileName = remoteFileUrl.Contains("?") ? "REN" + DateTime.Now.ToLongTimeString().Replace(":","") + DateTime.Now.Millisecond + sessionId + fileName : sessionId + fileName;


                WebClient client = new WebClient();
                client.Credentials = credential;
                using (client)
                {
                    //string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + fileName;
                    string fileSavedPath = AppDomain.CurrentDomain.BaseDirectory + "\\temp\\" + fileName;

                    if (!File.Exists(fileSavedPath))
                    {

                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        File.WriteAllBytes(fileSavedPath, fileBytes);

                        if (deleteAfterSessionEnds) System.Web.HttpContext.Current.Session[fileName] = fileName; // Write filename to Session to be deleted later

                    }
                    else
                    {

                            byte[] fileBytes = client.DownloadData(remoteFileUrl);

                            File.Delete(fileSavedPath);
                            File.WriteAllBytes(fileSavedPath, fileBytes);

                            if (deleteAfterSessionEnds) System.Web.HttpContext.Current.Session[fileName] = fileName; // Write filename to Session to be deleted later
                    }
                }

                return fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string SaveFileToTempFolderForGlobalAsax(NetworkCredential credential, string remoteFileUrl, string sessionId, bool deleteAfterSessionEnds)
        {
            try
            {
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);

                fileName = remoteFileUrl.Contains("?") ? "REN" + fileName : fileName;


                WebClient client = new WebClient();
                client.Credentials = credential;
                using (client)
                {
                    //string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + fileName;
                    string fileSavedPath = AppDomain.CurrentDomain.BaseDirectory + "\\temp\\" + fileName;

                    if (!File.Exists(fileSavedPath))
                    {

                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        File.WriteAllBytes(fileSavedPath, fileBytes);

                    }
                    else
                    {

                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        File.Delete(fileSavedPath);
                        File.WriteAllBytes(fileSavedPath, fileBytes);

                    }
                }

                return fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string SaveFileToTempFolderForMobile(NetworkCredential credential, string remoteFileUrl)
        {
            try
            {
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);

                fileName = remoteFileUrl.Contains("?") ? "REN" + fileName : fileName;


                WebClient client = new WebClient();
                client.Credentials = credential;
                using (client)
                {
                    //string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/mobile/") + fileName;
                    string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + fileName;

                    if (!File.Exists(fileSavedPath))
                    {

                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        File.WriteAllBytes(fileSavedPath, fileBytes);
                    }
                    else
                    {
                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        File.Delete(fileSavedPath);
                        File.WriteAllBytes(fileSavedPath, fileBytes);
                    }
                }

                return fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string SaveFileToTempFolderForBanners(NetworkCredential credential, string remoteFileUrl)
        {
            try
            {
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);

                fileName = remoteFileUrl.Contains("?") ? "Banner_" + "REN" + fileName : "Banner_" + fileName;

                string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + fileName;

                WebClient client = new WebClient();
                client.Credentials = credential;
                using (client)
                {
                    if (!File.Exists(fileSavedPath))
                    {

                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        File.WriteAllBytes(fileSavedPath, fileBytes);

                    }
                }

                return "../temp/" + fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string SaveFileForMobile(NetworkCredential credential, string remoteFileUrl, string context)
        {
            try
            {
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = "";

                fileName = System.IO.Path.GetFileName(uri.LocalPath);

                fileName = remoteFileUrl.Contains("?") ? "REN" + context + fileName : context + fileName;

                //string fileSavedPath = HttpContext.Current.Server.MapPath("~/mobile/") + fileName;
                string fileSavedPath = AppDomain.CurrentDomain.BaseDirectory + "\\mobile\\" + fileName;

                WebClient client = new WebClient();
                client.Credentials = credential;
                using (client)
                {
                    byte[] fileBytes = client.DownloadData(remoteFileUrl);

                    File.WriteAllBytes(fileSavedPath, fileBytes);
                }

                return "../mobile/" + fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string ResizeAndSaveFileToTempFolder(NetworkCredential credential, string remoteFileUrl, string sessionId, bool deleteAfterSessionEnds)
        {
            try
            {
                // HTML Decode/Encode işlemi gerekebilir
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                fileName = remoteFileUrl.Contains("?") ? "REN" + sessionId + fileName : sessionId + fileName;

                string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + fileName;

                if (!File.Exists(fileSavedPath))
                {
                    WebClient client = new WebClient();
                    client.Credentials = credential;
                    using (client)
                    {
                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(fileBytes);
                        System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                        System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(60, 60, null, IntPtr.Zero);
                        System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                        newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);
                        fileBytes = myResult.ToArray();

                        File.WriteAllBytes(fileSavedPath, fileBytes);

                        if (deleteAfterSessionEnds) System.Web.HttpContext.Current.Session[fileName] = fileName; // Write filename to Session to be deleted later
                    }
                }

                return fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string ResizeAndSaveFileToTempFolderForGlobalAsax(NetworkCredential credential, string remoteFileUrl, string sessionId, bool deleteAfterSessionEnds)
        {
            try
            {
                // HTML Decode/Encode işlemi gerekebilir
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                fileName = remoteFileUrl.Contains("?") ? "REN" + fileName :  fileName;

                string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + fileName;

                if (!File.Exists(fileSavedPath))
                {
                    WebClient client = new WebClient();
                    client.Credentials = credential;
                    using (client)
                    {
                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(fileBytes);
                        System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                        System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(60, 60, null, IntPtr.Zero);
                        System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                        newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);
                        fileBytes = myResult.ToArray();

                        File.WriteAllBytes(fileSavedPath, fileBytes);

                    }
                }

                return fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        // Smaller (Thumbnail) Image
        public static string ResizeAndSaveFileToMobileFolder(NetworkCredential credential, string remoteFileUrl)
        {
            try
            {
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                fileName = remoteFileUrl.Contains("?") ? "REN" + fileName : fileName;

                string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/mobile/") + fileName;

                if (!File.Exists(fileSavedPath))
                {
                    WebClient client = new WebClient();
                    client.Credentials = credential;
                    using (client)
                    {
                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(fileBytes);
                        System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                        System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(60, 60, null, IntPtr.Zero);
                        System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                        newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);
                        fileBytes = myResult.ToArray();

                        File.WriteAllBytes(fileSavedPath, fileBytes);
                    }
                }

                return fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }
        // Bigger Image
        public static string ResizeAndSaveFileToMobileFolder2(NetworkCredential credential, string remoteFileUrl)
        {
            try
            {
                remoteFileUrl = HttpUtility.HtmlDecode(remoteFileUrl);
                Uri uri = new Uri(remoteFileUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                fileName = remoteFileUrl.Contains("?") ? "REN" + fileName : fileName;

                string fileSavedPath = System.Web.HttpContext.Current.Server.MapPath("~/mobile/") + fileName;

                if (!File.Exists(fileSavedPath))
                {
                    WebClient client = new WebClient();
                    client.Credentials = credential;
                    using (client)
                    {
                        byte[] fileBytes = client.DownloadData(remoteFileUrl);

                        System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(fileBytes);
                        System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                        //System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(60, 60, null, IntPtr.Zero);
                        System.Drawing.Image newImage = fullsizeImage;
                        System.IO.MemoryStream myResult = new System.IO.MemoryStream();
                        newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);
                        fileBytes = myResult.ToArray();

                        File.WriteAllBytes(fileSavedPath, fileBytes);
                    }
                }

                return fileName;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        //public static bool FileExistsInTemp(string fileName)
        //{
        //    try
        //    {
        //        bool status = false;
        //        string filePath = System.IO.Path.GetTempPath() + fileName;
        //        if (File.Exists(filePath)) status = true;
        //        return status;
        //    }
        //    catch (System.Exception)
        //    {

        //        throw;
        //    }
        //}

        //public static bool DeleteFileFromTempFile(string fileName)
        //{
        //    try
        //    {
        //        string filePath = System.IO.Path.GetTempPath() + fileName;
        //        if (File.Exists(filePath)) File.Delete(filePath);
        //        return true;
        //    }
        //    catch (System.Exception)
        //    {

        //        throw;
        //    }
        //}

        public static string CreateImgTag(string srcString)
        {
            try
            {
                srcString = "<img src=\"../temp/" + srcString + "\"/>";
                return srcString;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string TransformHtmlString(string htmlString)
        {
            try
            {
                string pageContentStringTemp = "<html><body>" + HttpUtility.HtmlDecode(htmlString) + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContentStringTemp);

                #region control a tag
                if(htmlString.Contains("<a"))
                { 
                    var linkNodesA = doc.DocumentNode.SelectNodes("//a");
                    string oldLinkA = "";
                    string newLinkA = "";

                    // href control
                    foreach (var node in linkNodesA)
                    {
                        string fileName = "";

                        oldLinkA = node.GetAttributeValue("href", "not found");
                        if (oldLinkA != "not found")
                        {
                            if (oldLinkA.Contains("http"))
                            {
                                if (!oldLinkA.Contains("x"))
                                {
                                    newLinkA = oldLinkA;
                                }
                                else if (oldLinkA.Contains("https://www.x.com"))
                                {
                                    newLinkA = oldLinkA;
                                    fileName = ResizeAndSaveFileToTempFolder(GetNetworkCredential(), newLinkA, HttpContext.Current.Session.SessionID, true);
                                }
                            }
                            else
                            {
                                newLinkA = (DataStatics.saportHostURL + oldLinkA);
                                fileName = SaveFileToTempFolder(GetNetworkCredential(), newLinkA, HttpContext.Current.Session.SessionID, true);
                                htmlString = htmlString.Replace(oldLinkA, "../temp/" + fileName);
                            }
                        }
                        
                    }
                }
                #endregion


                #region control img tag
                if (htmlString.Contains("<img"))
                {
                    var linkNodesImg = doc.DocumentNode.SelectNodes("//img");
                    string oldLinkImg = "";
                    string newLinkImg = "";

                    // src control
                    foreach (var node in linkNodesImg)
                    {
                        string fileName = "";

                        oldLinkImg = node.GetAttributeValue("src", "not found");

                        if (oldLinkImg != "not found")
                        {
                            if (oldLinkImg.Contains("http"))
                            {
                                if (!oldLinkImg.Contains("x"))
                                {
                                    newLinkImg = oldLinkImg;
                                }
                                else if (oldLinkImg.Contains("https://www.x.com"))
                                {
                                    newLinkImg = oldLinkImg;
                                    fileName = ResizeAndSaveFileToTempFolder(GetNetworkCredential(), newLinkImg, HttpContext.Current.Session.SessionID, true);
                                }
                            }
                            else
                            {
                                newLinkImg = (DataStatics.saportHostURL + oldLinkImg);
                                fileName = SaveFileToTempFolder(GetNetworkCredential(), newLinkImg,
                                    HttpContext.Current.Session.SessionID, true); // SESSION ID'siz bir method ile test edilecek
                            }
                        }
                        htmlString = htmlString.Replace(oldLinkImg, "../temp/" + fileName);

                    }
                }
                #endregion

                return htmlString;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string TransformHtmlStringForGlobalAsax(string htmlString)
        {
            try
            {

                Random rnd = new Random();
                string uniqueId = rnd.Next(1000, 100000).ToString();

                string pageContentStringTemp = "<html><body>" + HttpUtility.HtmlDecode(htmlString) + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContentStringTemp);

                #region control a tag
                if (htmlString.Contains("<a"))
                {
                    var linkNodesA = doc.DocumentNode.SelectNodes("//a");
                    string oldLinkA = "";
                    string newLinkA = "";

                    // href control
                    foreach (var node in linkNodesA)
                    {
                        string fileName = "";

                        oldLinkA = node.GetAttributeValue("href", "not found");
                        if (oldLinkA != "not found")
                        {
                            if (oldLinkA.Contains("http"))
                            {
                                if (!oldLinkA.Contains("x"))
                                {
                                    newLinkA = oldLinkA;
                                }
                                else if (oldLinkA.Contains("https://www.x.com"))
                                {
                                    newLinkA = oldLinkA;
                                    fileName = ResizeAndSaveFileToTempFolderForGlobalAsax(GetNetworkCredential(), newLinkA, uniqueId, true);
                                }
                            }
                            else
                            {
                                newLinkA = (DataStatics.saportHostURL + oldLinkA);
                                fileName = SaveFileToTempFolderForGlobalAsax(GetNetworkCredential(), newLinkA, uniqueId, true);
                                htmlString = htmlString.Replace(oldLinkA, "../temp/" + fileName);
                            }
                        }

                    }
                }
                #endregion


                #region control img tag
                if (htmlString.Contains("<img"))
                {
                    var linkNodesImg = doc.DocumentNode.SelectNodes("//img");
                    string oldLinkImg = "";
                    string newLinkImg = "";

                    // src control
                    foreach (var node in linkNodesImg)
                    {
                        string fileName = "";

                        oldLinkImg = node.GetAttributeValue("src", "not found");

                        if (oldLinkImg != "not found")
                        {
                            if (oldLinkImg.Contains("http"))
                            {
                                if (!oldLinkImg.Contains("x"))
                                {
                                    newLinkImg = oldLinkImg;
                                }
                                else if (oldLinkImg.Contains("https://www.x.com"))
                                {
                                    newLinkImg = oldLinkImg;
                                    fileName = ResizeAndSaveFileToTempFolderForGlobalAsax(GetNetworkCredential(), newLinkImg, uniqueId, true);
                                }
                            }
                            else
                            {
                                newLinkImg = (DataStatics.saportHostURL + oldLinkImg);
                                fileName = SaveFileToTempFolderForGlobalAsax(GetNetworkCredential(), newLinkImg,
                                    uniqueId, true); 
                            }
                        }
                        htmlString = htmlString.Replace(oldLinkImg, "../temp/" + fileName);

                    }
                }
                #endregion

                return htmlString;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string TransformHtmlStringForMobile(string htmlString) 
        {
            try
            {
                string pageContentStringTemp = "<html><body>" + HttpUtility.HtmlDecode(htmlString) + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContentStringTemp);

                #region control a tag
                if (htmlString.Contains("<a"))
                {
                    var linkNodesA = doc.DocumentNode.SelectNodes("//a");
                    string oldLinkA = "";
                    string newLinkA = "";

                    // href control
                    foreach (var node in linkNodesA)
                    {
                        string fileName = "";

                        oldLinkA = node.GetAttributeValue("href", "not found");
                        if (oldLinkA != "not found")
                        {
                            if (oldLinkA.Contains("http"))
                            {
                                if (!oldLinkA.Contains("x"))
                                {
                                    newLinkA = oldLinkA;
                                }
                                else if (oldLinkA.Contains("https://www.x.com"))
                                {
                                    newLinkA = oldLinkA;
                                    fileName = ResizeAndSaveFileToMobileFolder(GetNetworkCredential(), newLinkA);
                                }
                            }
                            else
                            {
                                newLinkA = (DataStatics.saportHostURL + oldLinkA);
                                fileName = ResizeAndSaveFileToMobileFolder(GetNetworkCredential(), newLinkA);
                                htmlString = htmlString.Replace(oldLinkA, "../mobile/" + fileName);
                            }
                        }

                    }
                }
                #endregion


                #region control img tag
                if (htmlString.Contains("<img"))
                {
                    var linkNodesImg = doc.DocumentNode.SelectNodes("//img");
                    string oldLinkImg = "";
                    string newLinkImg = "";

                    // src control
                    foreach (var node in linkNodesImg)
                    {
                        string fileName = "";

                        oldLinkImg = node.GetAttributeValue("src", "not found");

                        if (oldLinkImg != "not found")
                        {
                            if (oldLinkImg.Contains("http"))
                            {
                                if (!oldLinkImg.Contains("x"))
                                {
                                    newLinkImg = oldLinkImg;
                                }
                                else if (oldLinkImg.Contains("https://www.x.com"))
                                {
                                    newLinkImg = oldLinkImg;
                                    fileName = ResizeAndSaveFileToMobileFolder2(GetNetworkCredential(), newLinkImg);
                                }
                            }
                            else
                            {

                                newLinkImg = (DataStatics.saportHostURL + oldLinkImg);
                                fileName = ResizeAndSaveFileToMobileFolder2(GetNetworkCredential(), newLinkImg);
                            }
                        }
                        htmlString = htmlString.Replace(oldLinkImg, "../mobile/" + fileName);

                    }
                }
                #endregion

                return htmlString;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string TransformHtmlStringAndGetFileName(string htmlString)
        {
            try
            {
                string pageContentStringTemp = "<html><body>" + HttpUtility.HtmlDecode(htmlString) + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContentStringTemp);

                #region control a tag
                if (htmlString.Contains("<a"))
                {
                    var linkNodesA = doc.DocumentNode.SelectNodes("//a");
                    string oldLinkA = "";
                    string newLinkA = "";

                    // href control
                    foreach (var node in linkNodesA)
                    {
                        string fileName = "";

                        oldLinkA = node.GetAttributeValue("href", "not found");
                        if (oldLinkA != "not found")
                        {
                            if (oldLinkA.Contains("http"))
                            {
                                if (!oldLinkA.Contains("x"))
                                {
                                    newLinkA = oldLinkA;
                                }
                                else if (oldLinkA.Contains("https://www.x.com"))
                                {
                                    newLinkA = oldLinkA;
                                    fileName = ResizeAndSaveFileToTempFolder(GetNetworkCredential(), newLinkA, HttpContext.Current.Session.SessionID, true);
                                }
                            }
                            else
                            {
                                newLinkA = (DataStatics.saportHostURL + oldLinkA);
                                fileName = SaveFileToTempFolder(GetNetworkCredential(), newLinkA, HttpContext.Current.Session.SessionID, true);
                                htmlString = fileName;
                            }
                        }

                    }
                }
                #endregion

                #region control img tag
                if (htmlString.Contains("<img"))
                {
                    var linkNodesImg = doc.DocumentNode.SelectNodes("//img");
                    string oldLinkImg = "";
                    string newLinkImg = "";

                    // src control
                    foreach (var node in linkNodesImg)
                    {
                        string fileName = "";

                        oldLinkImg = node.GetAttributeValue("src", "not found");

                        if (oldLinkImg != "not found")
                        {
                            if (oldLinkImg.Contains("http"))
                            {
                                if (!oldLinkImg.Contains("x"))
                                {
                                    newLinkImg = oldLinkImg;
                                }
                                else if (oldLinkImg.Contains("https://www.x.com"))
                                {
                                    newLinkImg = oldLinkImg;
                                    fileName = ResizeAndSaveFileToTempFolder(GetNetworkCredential(), newLinkImg, HttpContext.Current.Session.SessionID, true);
                                }
                            }
                            else
                            {

                                newLinkImg = (DataStatics.saportHostURL + oldLinkImg);
                                fileName = SaveFileToTempFolder(GetNetworkCredential(), newLinkImg,
                                    HttpContext.Current.Session.SessionID, true);
                            }
                        }
                        htmlString = fileName;

                    }
                }
                #endregion

                return htmlString;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string TransformHtmlStringAndGetFileUrl(string htmlString)
        {
            try
            {
                string pageContentStringTemp = "<html><body>" + HttpUtility.HtmlDecode(htmlString) + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContentStringTemp);

                #region control a tag
                if (htmlString.Contains("<a"))
                {
                    var linkNodesA = doc.DocumentNode.SelectNodes("//a");
                    string oldLinkA = "";
                    string newLinkA = "";

                    // href control
                    foreach (var node in linkNodesA)
                    {
                        oldLinkA = node.GetAttributeValue("href", "not found");
                        if (oldLinkA != "not found")
                        {
                            if (oldLinkA.Contains("http"))
                            {
                                if (!oldLinkA.Contains("x"))
                                {
                                    newLinkA = oldLinkA;
                                }
                                else if (oldLinkA.Contains("https://www.x.com"))
                                {
                                    newLinkA = oldLinkA;
                                }
                            }
                            else
                            {
                                newLinkA =  oldLinkA;
                            }
                        }
                        htmlString = newLinkA;
                    }
                }
                #endregion

                #region control img tag
                if (htmlString.Contains("<img"))
                {
                    var linkNodesImg = doc.DocumentNode.SelectNodes("//img");
                    string oldLinkImg = "";
                    string newLinkImg = "";

                    // src control
                    foreach (var node in linkNodesImg)
                    {

                        oldLinkImg = node.GetAttributeValue("src", "not found");

                        if (oldLinkImg != "not found")
                        {
                            if (oldLinkImg.Contains("http"))
                            {
                                if (!oldLinkImg.Contains("x"))
                                {
                                    newLinkImg = oldLinkImg;
                                }
                                else if (oldLinkImg.Contains("https://www.x.com"))
                                {
                                    newLinkImg = oldLinkImg;
                                }
                            }
                            else
                            {

                                newLinkImg = oldLinkImg;
                                
                            }
                        }

                        htmlString = newLinkImg;

                    }
                }
                #endregion

                return htmlString;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string TransformImgHtmlStringAndGetBase64(string htmlString)
        {
            try
            {
                string pageContentStringTemp = "<html><body>" + HttpUtility.HtmlDecode(htmlString) + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContentStringTemp);

                #region control img tag
                if (htmlString.Contains("<img"))
                {
                    var linkNodesImg = doc.DocumentNode.SelectNodes("//img");
                    string oldLinkImg = "";
                    string newLinkImg = "";

                    // src control
                    foreach (var node in linkNodesImg)
                    {
                        string fileName = "";

                        oldLinkImg = node.GetAttributeValue("src", "not found");

                        if (oldLinkImg != "not found")
                        {
                            if (oldLinkImg.Contains("http"))
                            {
                                if (!oldLinkImg.Contains("x"))
                                {
                                    newLinkImg = oldLinkImg;
                                }
                                else if (oldLinkImg.Contains("https://www.x.com"))
                                {
                                    newLinkImg = oldLinkImg;
                                    fileName = RemoteImageUrlToBase64Converter(newLinkImg,true);
                                }
                            }
                            else
                            {

                                newLinkImg = (DataStatics.saportHostURL + oldLinkImg);
                                fileName = RemoteImageUrlToBase64Converter(newLinkImg, true);

                            }
                        }
                        htmlString = fileName;
                    }
                }
                #endregion

                return htmlString;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string ReplaceImagesAndCutHtmlString(string htmlString)
        {
            try
            {
                string pageContentStringTemp = "<html><body>" + HttpUtility.HtmlDecode(htmlString) + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContentStringTemp);


                #region delete img tags

                while (htmlString.Contains("<p"))
                {
                    doc.DocumentNode.SelectNodes("//img").Clear();
                }

                htmlString = doc.ToString().Replace("<html><body>", "").Replace("</body></html>", "");
                
                #endregion

                return htmlString;

            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static bool CheckIfFileExists(string fileLocation)
        {
            try
            {
                bool status = false;
                status = System.IO.File.Exists(fileLocation);
                return status;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return false;
            }
        }

        public static string GetDateTurkishCulture(DateTime date)
        {
            try
            {
                CultureInfo culture = new CultureInfo("tr-TR");
                return date.ToString("d", culture);
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static void DeleteFile(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                if (File.Exists(fileLocation))
                {
                    WebClient client = new WebClient();
                    client.Credentials = GetNetworkCredential();
                    using (client)
                    {
                        File.Delete(fileLocation);
                    }

                }
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }


    }
}
