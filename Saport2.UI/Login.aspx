<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Saport2.UI.Login" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<%@ OutputCache Duration="60" VaryByParam="*" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SAPORT Web | Login</title>
    <link rel="icon" href="resources/img/favicon.ico"/>
    <style type="text/css">
        body{background-color: #66BAEE !important;
            padding: 0;
            margin: 0;
            font-family: "Segoe UI","Segoe",Tahoma,Helvetica,Arial,sans-serif;
            font-size: 12px;
            color: #fff;
            line-height: 18px;
        }
        #loginArea{
            width: 460px;
            margin: auto;
            /*background-color: orange;*/
            background: #ffe46d; /* Old browsers */
            background: -moz-linear-gradient(top,  #ffe46d 0%, #ff7200 45%, #ff7200 100%); /* FF3.6-15 */
            background: -webkit-linear-gradient(top,  #ffe46d 0%,#ff7200 45%,#ff7200 100%); /* Chrome10-25,Safari5.1-6 */
            background: linear-gradient(to bottom,  #ffe46d 0%,#ff7200 45%,#ff7200 100%); /* W3C, IE10+, FF16+, Chrome26+, Opera12+, Safari7+ */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffe46d', endColorstr='#ff7200',GradientType=0 ); /* IE6-9 */
            padding: 20px;
            border-radius: 10px;
            min-height:250px;
        }
        .loginAll { display:block;padding:0;margin:0
        }
        .loginLeft {float: left; display: block; width: 150px; font-weight: bold; font-size: 13px; text-align: right;padding-right: 10px;}
        .loginRight { float:left;min-width:180px;}
        #loginArea ul li {
        margin-bottom:10px;}
        input{border:1px solid #ddd; padding:5px;}
        input[type="submit"] {border-style: none;
            border-color: inherit;
            border-width: medium;
            background-color:#66BAEE;color:#fff;border-radius:5px;margin-top:10px;
        }

        #divNotification {
            font-size:14px;
            background-color: rgba(54, 50, 50, 0.22);
            padding: 8px;
            text-align:center;
            text-transform: uppercase;
            color: #ffe06a;
            font-weight: bold;
            width: 100%;
            position:absolute;
            margin-top: 30px;
        }
        #captcha img {
            height: 25px;
            
        }
    </style>
    <script language="javascript" type="text/javascript">
    function clearText(field)
    {
        if (field.defaultValue == field.value) field.value = '';
        else if (field.value == '') field.value = field.defaultValue;

    }
</script>
</head>
<body>
    
    <form id="form1" runat="server">
    <div>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

         <asp:UpdatePanel ID="UpdatePanel1"  runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <div style="position:absolute; height:100%; width:100%; display: table; top: 37px; left: 0px;">

                    <div id="divNotification" runat="server" ><asp:Literal ID="ltrNotification" runat="server"></asp:Literal> </div>

  	                <div style="display: table-cell;vertical-align: middle;text-align:center;">
                          
                          <div id="loginArea">

                              <ul style="list-style:none" class="loginAll">
                                  <li><div align="center" style="padding-top:20px;padding-bottom:30px"><img src="https://saportweb.x.com/resources/img/site-logo.png" /></div></li>
                                  <li></li>

                          <asp:Panel ID="pnlLogin" runat="server">
                        


                        <li>
                            <div class="loginLeft">Eposta / Telefon :</div>
                            <div class="loginRight"><asp:TextBox ID="txtEmailOrPhone" placeholder="birisi@domain.com / 5001234567" runat="server" Height="20px" Width="230px"></asp:TextBox></div>
                            <div style="clear:both"></div>

                        </li>

                        <li>
                            <div class="loginLeft">Şifre : </div>
                            <div class="loginRight"><asp:TextBox ID="txtPassword"  onfocus="clearText(this)" onblur="clearText(this)" value=""  runat="server" TextMode="Password" Height="20px" Width="230px" placeholder="Şifreniz" ></asp:TextBox></div>
                            <div style="clear:both"></div>
                        </li>
                       <li>
                           <div class="loginLeft" id="captcha">
                           <cc1:CaptchaControl ID="CaptchaControl1" runat="server" Height="25px" 
                         CaptchaLength="4" BackColor="#ff7200"  CaptchaWidth="110" CaptchaHeight="40"
                        EnableViewState="False" CaptchaBackgroundNoise="None" Font-Size="XX-Small" Font-Bold="false" Font-Italic="false" CaptchaFontWarping="None" />
                           </div>

                            <div class="loginRight">
                                <asp:TextBox ID="txtCaptcha" runat="server" Height="20px" Width="230px" placeholder="Soldaki metni yazınız"></asp:TextBox></div>
                            <div style="clear:both"></div>
                       </li>

                        <li><div> </div> </li>

                        <li>
                            <div class="loginLeft">&nbsp;</div>
                            <div class="loginRight" style="margin-left:20px">
                                <asp:Button ID="btnSubmit" runat="server" Text="Oturum Aç" OnClick="btnSubmit_Click" />
<%--                            <a href="#" style="float:right;margin-top: 15px;color: white;text-decoration: none;">Şifremi Unuttum</a>--%>
                            <asp:LinkButton ID="btnForgetPassword" runat="server" style="float:right;margin-top: 15px;color: white;text-decoration: none;" OnClick="btnForgetPassword_Click">Şifremi Unuttum</asp:LinkButton>
                         <div style="clear:both"></div>
                         </div><div style="clear:both"></div></li>
                          </asp:Panel>


                          <asp:Panel ID="pnlForgetPassword" runat="server">
                              <li><div class="loginLeft">Eposta / Telefon :</div><div class="loginRight">
                                  <asp:TextBox ID="txtForgotPassword" runat="server" Height="20px" Width="200px" placeholder="birisi@domain.com / 50012345677"></asp:TextBox></div></li>
                              <li><div class="loginLeft" style="">&nbsp;</div>
                                  <div class="loginRight" style="padding-top:5px">
                                      <asp:Button ID="btnRemindPassword" runat="server" Text="Şifremi Gönder" OnClick="btnRemindPassword_Click"  />
                                      <asp:LinkButton ID="btnBackToLoginScreen" runat="server" style=" margin-top: 15px; margin-left:10px;color: white;text-decoration: none;" OnClick="btnBackToLoginScreen_Click">İptal</asp:LinkButton>
                                      <div style="clear:both">
                                  </div>
                              </li>
                          </asp:Panel>

                          </ul>
                         </div>
                </div>
                    <div style="display:none"><asp:Literal ID="ltrResult" runat="server"></asp:Literal></div>
              </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
