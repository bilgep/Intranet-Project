using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;
using System.Net;
using Microsoft.SharePoint.Client.Search.Query;

namespace Saport2.Data
{
    public class DataQuery
    {
        /* It helps to query SP Lists, SP Query etc with basic query methods. */

        public static ListItemCollection QueryListItems(string siteUrl, string listName, string queryText)
        {
            try
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                ClientContext clientContext = new ClientContext(siteUrl);
                clientContext.Credentials = credentials;
                //clientContext.ExecuteQuery();
                List oList = clientContext.Web.Lists.GetByTitle(listName);
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = queryText;
                ListItemCollection collListItems = oList.GetItems(camlQuery);
                clientContext.Load(collListItems);
                clientContext.ExecuteQuery();
                clientContext.Dispose();

                return collListItems;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        //public static SP.ListItemCollection UpdateListItem(string siteUrl, string listName, int id)
        //{
        //    try
        //    {
        //        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
        //        SP.ClientContext clientContext = new SP.ClientContext(siteUrl);
        //        clientContext.Credentials = credentials;
        //        SP.List oList = clientContext.Web.Lists.GetByTitle(listName);
        //        SP.CamlQuery camlQuery = new SP.CamlQuery();
        //        camlQuery.ViewXml = queryText;
        //        SP.ListItemCollection collListItems = oList.GetItems(camlQuery);
        //        clientContext.Load(collListItems);
        //        clientContext.ExecuteQuery();
        //        clientContext.Dispose();

        //        return collListItems;

        //    }
        //    catch (Exception ex)
        //    {
        //        EXP.RedirectToErrorPage(ex.Message);
        //        return null;
        //    }
        //}


        public static List<string> QueryFolderFileUrls(string siteUrl, string listName, string folderName)
        {
            try
            {
                List<string> folderFileUrls = new List<string>();
                ClientContext clientContext = new ClientContext(siteUrl);
                clientContext.Credentials = new NetworkCredential(DataStatics.UserNameForService, DataStatics.PasswordForService, DataStatics.DomainForService);
                List oList = clientContext.Web.Lists.GetByTitle(listName);
                clientContext.Load(oList);
                clientContext.ExecuteQuery();
                clientContext.Load(oList.RootFolder.Folders);
                clientContext.ExecuteQuery();

                for (int i = 0; i < oList.RootFolder.Folders.Count; i++)
                {
                    if (oList.RootFolder.Folders[i].Name == folderName)
                    {
                        Folder myFolder = oList.RootFolder.Folders[i];
                        clientContext.Load(myFolder.Files);
                        clientContext.ExecuteQuery();
                        foreach (Microsoft.SharePoint.Client.File file in myFolder.Files)
                        {
                            if (file.Name.Contains(".jpg") || file.Name.Contains(".png") || file.Name.Contains(".gif") || file.Name.Contains(".bmp") || file.Name.Contains(".JPG") || file.Name.Contains(".PNG") || file.Name.Contains(".GIF") || file.Name.Contains(".BMP"))
                            {
                                folderFileUrls.Add(file.ServerRelativeUrl);
                            }
                        }
                        clientContext.Dispose();
                        break;
                    }
                }
               
                return folderFileUrls;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

    }
}
