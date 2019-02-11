using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web;

namespace Saport2.Business.Entity
{
    public class SaUserService : SaUserModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        // SERVİS GELDİĞİNDE DEĞİŞECEK
        public static SaUser QueryUserByGsmNumber(string gsmNumber)
        {
            try
            {
                SaUser thisUser = new SaUser();

                string queryText = string.Format(SaUserModel.userCamlQueryByGsmNumber, gsmNumber);

                ListItemCollection coll = DAT.DataQuery.QueryListItems(SaUserModel.usersListSiteUrl, SaUserModel.usersListName, string.Format(SaUserModel.userCamlQueryByGsmNumber, gsmNumber));
                if (coll.Count > 0)
                {
                    ListItem item = coll[0];
                    thisUser.Id = Convert.ToInt32(item["ID"]);
                    thisUser.Name = item["Title"].ToString();
                    thisUser.LastName = item["LastName"].ToString();
                    thisUser.GsmNumber = item["GsmNumber"] != null ? item["GsmNumber"].ToString() : string.Empty;
                    thisUser.Email = item["Email"] != null ? item["Email"].ToString() : string.Empty;
                    thisUser.Password = item["Password"].ToString();
                    thisUser.UserActive = Convert.ToBoolean(item["UserActive"]);
                    thisUser.LoginType = LoginTypeName.Phone;
                    //thisUser.UserDevices = QueryUserDevices((int)thisUser.Id);
                }
                return thisUser;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        // SERVİS GELDİĞİNDE DEĞİŞECEK
        public static SaUser QueryUserByEmail(string email)
        {
            try
            {
                SaUser thisUser = new SaUser();
                ListItemCollection coll = DAT.DataQuery.QueryListItems(SaUserModel.usersListSiteUrl, SaUserModel.usersListName, string.Format(SaUserModel.userCamlQueryByEmail, email));
                if(coll.Count > 0)
                {
                    ListItem item = coll[0];
                    thisUser.Id = Convert.ToInt32(item["ID"]);
                    thisUser.Name = item["Title"].ToString();
                    thisUser.LastName = item["LastName"].ToString();
                    thisUser.GsmNumber = item["GsmNumber"] != null ? item["GsmNumber"].ToString() : string.Empty;
                    thisUser.Email = item["Email"] != null ? item["Email"].ToString() : string.Empty;
                    thisUser.Password = item["Password"].ToString();
                    thisUser.UserActive = Convert.ToBoolean(item["UserActive"]);
                    thisUser.LoginType = LoginTypeName.Email;
                    //thisUser.UserDevices = QueryUserDevices((int)thisUser.Id);
                }
                return thisUser;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static LoginTypeName IdentifyAndValidateEmailOrPhone(string inputText)
        {
            try
            {
                LoginTypeName loginType = LoginTypeName.None;

                Regex emailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
                Regex phoneRegex = new Regex(@"^(5)[0-9][0-9][1-9]([0-9]){6}$");

                loginType = emailRegex.IsMatch(inputText) ? LoginTypeName.Email : (phoneRegex.IsMatch(inputText) ? LoginTypeName.Phone : LoginTypeName.None);
                return loginType;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return LoginTypeName.None;
            }
        }

        public static bool IdentifyAndValidatePhone(string inputText)
        {
            try
            {
                bool status = false;
                Regex phoneRegex = new Regex(@"^(5)[0-9][0-9][1-9]([0-9]){6}$");
                status = phoneRegex.IsMatch(inputText);
                return status;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return false;
            }
        }


        public static List<SaUserDevice> QueryUserDevices(int userId)
        {
            try
            {
                List<SaUserDevice> userDevices = new List<SaUserDevice>();
                ListItemCollection coll = DAT.DataQuery.QueryListItems(SaUserModel.usersListSiteUrl, SaUserModel.userDetailsListName, string.Format(SaUserModel.userDevicesCamlQuery, userId));
                if (coll.Count > 0)
                {
                    foreach (ListItem item in coll)
                    {
                        SaUserDevice device = new SaUserDevice();
                        FieldLookupValue idValue = (FieldLookupValue)item["UserId"];
                        device.UserId = Convert.ToInt32(idValue.LookupValue);
                        device.DeviceId = item["Title"].ToString();
                        userDevices.Add(device);
                    }
                }

                return userDevices;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string RenewSaUserPassword(string userGsmNumber)
        {
            try
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(DAT.DataStatics.UserNameForService, DAT.DataStatics.PasswordForService, DAT.DataStatics.DomainForService);

                ClientContext clientContext = new ClientContext(usersListSiteUrl);
                clientContext.Credentials = credentials;
                List oList = clientContext.Web.Lists.GetByTitle(usersListName);

                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = string.Format(userCamlQueryByGsmNumber, userGsmNumber);
                ListItemCollection collListItems = oList.GetItems(camlQuery);
                clientContext.Load(collListItems);
                clientContext.ExecuteQuery();

                ListItem item = collListItems[0];
                clientContext.Load(item);
                clientContext.ExecuteQuery();
                string newPassword = GeneratePassword();
                newPassword = EncodeToBase64(newPassword);
                item["Password"] = newPassword;

                // Mevcut User için Yazma Erişimi Reddedildiği için aşağıdaki kod satırları yorum satırı yapıldı
                //item.Update(); 
                //clientContext.ExecuteQuery();

                clientContext.Dispose();
                return newPassword;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string EncodeToBase64(string toBase64String)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(toBase64String);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string DecodeFromBase64(string encodedString)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(encodedString);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string GeneratePassword()
        {
            try
            {
                const string characters = "abcdefghijklmnopqrstuvwxyz1234567890";
                Random rnd = new Random();
                string password = string.Empty;

                for (int i = 0; i < 6; i++)
                {
                    password += characters[rnd.Next(characters.Length)];
                }

                //string password = System.Web.Security.Membership.GeneratePassword(6, 0);

                return password;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static bool CreateAuthenticationCookies(string userName)
        {
            try
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(15), false, "authenticated"); // expires in 2 minutes
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket1));
                HttpContext.Current.Response.Cookies.Add(cookie1);
                return true;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return false;
            }
            
        }

        // SMS ÇALIŞIYOR , SMTP GÖNDERİMİ EKLENECEK
        public static void SendSmsOrEmail(string userName, string password, DateTime sendDate, DateTime expirationDate, string[] receipents, string message )
        {
            try
            {
                com.postaguvercini.www.smsservice smsService = new com.postaguvercini.www.smsservice();
                var serviceResult = smsService.SmsInsert_1_N(userName, password, sendDate, expirationDate, receipents, message);
                smsService.Dispose();
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }

        #endregion
    }
}
