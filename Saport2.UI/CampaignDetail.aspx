<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="CampaignDetail.aspx.cs" Inherits="Saport2.UI.CampaignDetail" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="box">
    <div class="green-title">Kampanya Detayı</div>
        	
    <div class="news-detail">
		                <div>

	                	<div class="page-header">
	                        <h3>
		                        <!--Title-->
                                <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
	                        </h3>
                        </div>
                        <br />
                        <%--<p class="lead">
	                        <!--Spot Metin-->
						</p>--%>
						
						<div class="info-bar">
						    <i class="atr"></i>
						    <ul class="unstyled inline">
						        <li>
						        	<span>Bitiş Tarihi</span>
						        	<strong>
						            	<!--Haber Bitiş Tarihi-->
                                        <asp:Literal ID="ltrEndDate" runat="server"></asp:Literal>
						            </strong>
						        </li>
						    </ul>
						</div>
                        
                        <%--<div class="thumbnail">
	                        <!--Listeleme Resmi-->
	                        <div id="ctl00_PlaceHolderMain_ctl02_ctl05_label" style="display:none">Toplama Resmi</div><div id="ctl00_PlaceHolderMain_ctl02_ctl05__ControlWrapper_RichImageField" class="ms-rtestate-field" style="display:inline" aria-labelledby="ctl00_PlaceHolderMain_ctl02_ctl05_label"><div class="ms-rtestate-field"></div></div></div>--%>

                        <p>
	                        <!--HTML Detay - Page Content-->         
                            <asp:Literal ID="ltrPageContent" runat="server"></asp:Literal>                   
	                    </p>
	    </div>                   
    </div>
</div>
</asp:Content>
