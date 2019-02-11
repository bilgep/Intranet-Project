<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="NewsDetail.aspx.cs" Inherits="Saport2.UI.NewsDetail" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box">
    <div class="green-title">Haber Detayı</div>
        	
    <div class="news-detail">
		                <div>

	                	<div class="page-header">
	                        <h3>
		                        <!--Title-->
                                <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
	                        </h3>
                        </div>
                        <br />
						
						<div class="info-bar">
						    <i class="atr"></i>
						    <ul class="unstyled inline">
						        <li>
						        	<span>Güncelleme Tarihi</span>
						        	<strong>
						            	<!--Haber Güncelleme Tarihi-->
                                        <asp:Literal ID="ltrModified" runat="server"></asp:Literal>
						            </strong>
						        </li>
						    </ul>
						</div>

                        <p>
	                        <!--HTML Detay - Page Content-->         
                            <asp:Literal ID="ltrPageContent" runat="server"></asp:Literal>                   
	                    </p>
	    </div>                   
    </div>
</div>
</asp:Content>
