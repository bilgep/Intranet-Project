<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="Announcements.aspx.cs" Inherits="Saport2.UI.Announcements" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="clear:both;height:20px"></div>

    <!-- Announcements Top BEGIN -->
    <div class="well well-small well-site-panel">
        <!-- Title -->
		<div class="well-title clearfix">
		  <div class="pull-left">
            <h3>Duyurular</h3>
          </div>
        </div>

        <!-- Body -->
        <div class="content">
            <div class="pull-right site-panel-logo">
                <img src="../resources/img/duyuru-icon.png"/>
            </div>
            <br/>
            <p style="font-size:14px">Sabancı Topluluğu’na ilişkin tüm duyurular, atama haberleri, yeni uygulama bilgileri... Hepsi burada.<br/><br/></p>
        </div>
    </div>
    <!-- Announcements Top END -->

    <div style="clear:both;height:20px"></div>

    <!-- Announcements Body BEGIN -->
    <div class="box">
        <div class="green-title">Duyurular</div>
        	
            <div class="blog-post-list" style="">
                <!-- Announcement Items BEGIN -->

                <asp:Literal ID="ltrMain" runat="server"></asp:Literal>

                <!-- Announcement Items END -->

            </div>
            
    </div>

    <!-- Announcements Body END -->

    <div style="clear:both;height:20px"></div>


    
</asp:Content>
