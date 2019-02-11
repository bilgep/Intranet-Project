using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EXP = Saport2.Shared.Exceptions;
using Saport2.Business.Entity;
using LOGSTAT = Saport2.Business.Entity.SaUserModel;
using LOGSERV = Saport2.Business.Entity.SaUserService;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Saport2.UI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Captcha


                #endregion

                if (!Page.IsPostBack)
                {
                    pnlLogin.Visible = true;
                    pnlForgetPassword.Visible = false;

                    divNotification.Visible = false;
                    ltrNotification.Text = string.Empty;

                }

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                #region Encoding
                string inputEmailOrPhone = HttpUtility.HtmlEncode(txtEmailOrPhone.Text);
                string inputPassword = HttpUtility.HtmlEncode(txtPassword.Text);
                string captchaText = HttpUtility.HtmlEncode(txtCaptcha.Text);
                #endregion

                #region Validations, Notifications & Authenticaiton Process
                divNotification.Visible = false;
                ltrNotification.Text = string.Empty;

                LOGSTAT.LoginTypeName logType = LOGSERV.IdentifyAndValidateEmailOrPhone(inputEmailOrPhone);
                SaUserModel.SaUser thisUser = new LOGSTAT.SaUser();


                if (inputEmailOrPhone == string.Empty)
                {
                    divNotification.Visible = true;
                    ltrNotification.Text = LOGSTAT.msgPhoneEmailEmpty;
                }
                else if (inputPassword == string.Empty)
                {
                    divNotification.Visible = true;
                    ltrNotification.Text = LOGSTAT.msgPasswordEmpty;
                }
                else if (txtCaptcha.Text == string.Empty)
                {
                    divNotification.Visible = true;
                    ltrNotification.Text = LOGSTAT.msgCaptchaEmpty;
                }
                else if (logType == LOGSTAT.LoginTypeName.None)
                {
                    divNotification.Visible = true;
                    ltrNotification.Text = LOGSTAT.msgInvalidPhoneEmailFormat;
                }
                else if (txtCaptcha.Text != string.Empty)
                {
                    CaptchaControl1.ValidateCaptcha(captchaText);

                    if (!CaptchaControl1.UserValidated)
                    {
                        divNotification.Visible = true;
                        ltrNotification.Text = LOGSTAT.msgCaptchaEmpty;
                    }
                    else
                    {

                        thisUser = logType == SaUserModel.LoginTypeName.Email ? SaUserService.QueryUserByEmail(inputEmailOrPhone) : (logType == SaUserModel.LoginTypeName.Phone ? SaUserService.QueryUserByGsmNumber(inputEmailOrPhone) : null);

                        if (thisUser.Password != inputPassword)
                        {
                            divNotification.Visible = true;
                            ltrNotification.Text = LOGSTAT.msgInvalidPassword;
                        }
                        else if (thisUser != null && thisUser.UserActive == true)
                        {

                            SaUserService.CreateAuthenticationCookies(inputEmailOrPhone);

                            HttpCookie userCookie = new HttpCookie("userName", thisUser.Name + " " + thisUser.LastName);
                            HttpContext.Current.Response.Cookies.Add(userCookie);

                            string returnUrlString = Request.QueryString["ReturnUrl"] != null ? Request.QueryString["ReturnUrl"].ToString() : string.Empty;

                            string redirectionUrl = !string.IsNullOrEmpty(returnUrlString) ? returnUrlString : "~/Home.aspx";
                            Response.Redirect(redirectionUrl, false);

                            //if (!string.IsNullOrEmpty(returnUrlString))
                            //{
                            //    Response.Redirect(returnUrlString, false);
                            //}
                            //else
                            //{
                            //    Response.Redirect("~/Home.aspx", false);

                            //}
                        }
                        else
                        {
                            divNotification.Visible = true;
                            ltrNotification.Text = LOGSTAT.msgUserDoesntExist;
                        }
                    }
                }

                #endregion


                txtCaptcha.Text = string.Empty;
                txtEmailOrPhone.Text = string.Empty;
                txtPassword.Text = string.Empty;


 

            }
            catch (Exception ex)
            {
                ltrResult.Text = ex.Message + " - " + ex.StackTrace;
                //EXP.RedirectToErrorPage(ex.Message);
            }
        }

        protected void btnForgetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                pnlLogin.Visible = false;
                pnlForgetPassword.Visible = true;
                divNotification.Visible = false;

                txtForgotPassword.Text = string.Empty;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }

        protected void btnRemindPassword_Click(object sender, EventArgs e)
        {
            try
            {

                SaUserModel.SaUser thisUser = new LOGSTAT.SaUser();

                string strForgotPassword = HttpUtility.HtmlEncode(txtForgotPassword.Text);

                if (strForgotPassword == string.Empty)
                {
                    divNotification.Visible = true;
                    ltrNotification.Text = LOGSTAT.msgPhoneEmpty;
                    return;
                }
                else
                {
                    LOGSTAT.LoginTypeName logType = LOGSERV.IdentifyAndValidateEmailOrPhone(strForgotPassword);

                    if (logType == SaUserModel.LoginTypeName.Email)
                    {
                        // SERVİS GELİNCE GÜNCELLENECEK
                        thisUser = LOGSERV.QueryUserByEmail(strForgotPassword);
                    }
                    else if (logType == SaUserModel.LoginTypeName.Phone)
                    {
                        // GEÇİCİ SİLİNECEK
                        strForgotPassword = "00000000";

                        // SERVİS GELİNCE GÜNCELLENECEK
                        thisUser = LOGSERV.QueryUserByGsmNumber(strForgotPassword);
                    }
                    else
                    {
                        divNotification.Visible = true;
                        ltrNotification.Text = LOGSTAT.msgInvalidPhoneOrEmailFormat;
                        return;
                    }
                }

               
                thisUser.UserActive = true;

                if ((!string.IsNullOrEmpty(thisUser.GsmNumber) || !string.IsNullOrEmpty(thisUser.Email)) && thisUser.UserActive == true)
                {

                    //thisUser.Password = LOGSERV.RenewSaUserPassword(thisUser.GsmNumber);  


                    thisUser.Password = LOGSERV.GeneratePassword();
                    thisUser.Password = LOGSERV.EncodeToBase64(thisUser.Password);

                    if (thisUser.LoginType == SaUserModel.LoginTypeName.Phone)
                    {
                        // 

                    }
                    else if (thisUser.LoginType == SaUserModel.LoginTypeName.Email)
                    {
                        // Send Email İşlemleri (SMTP)
                        SmtpClient smtpClient = new SmtpClient("192.168.0.0", 25);
                        smtpClient.UseDefaultCredentials = true;
                        //smtpClient.EnableSsl = true;
                        MailMessage mail = new MailMessage("saportwebadmin@x.com", thisUser.Email);
                        mail.IsBodyHtml = true;
                        mail.Subject = "SaportWeb | Erişim";
                        mail.Body = "SaportWeb Portal'a erişim için şifreniz: <b> XXXXX </b>'dir.<br/><br/><a href=\"https://saportweb.x.com/Login.aspx\" >>> SaportWeb Portal'a git</a>";
                        smtpClient.Send(mail);
                    }

                    // Go Back To Login Panel
                    pnlForgetPassword.Visible = false;
                    pnlLogin.Visible = true;
                    divNotification.Visible = true;
                    ltrNotification.Text = LOGSTAT.msgAfterSmsOrEmail;
                }
                else
                {
                    divNotification.Visible = true;
                    ltrNotification.Text = LOGSTAT.msgUserDoesntExist;
                }
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }

        protected void btnBackToLoginScreen_Click(object sender, EventArgs e)
        {
            try
            {
                pnlLogin.Visible = true;
                pnlForgetPassword.Visible = false;
                divNotification.Visible = false;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }
        }
    }
}