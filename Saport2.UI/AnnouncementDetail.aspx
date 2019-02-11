<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="AnnouncementDetail.aspx.cs" Inherits="Saport2.UI.AnnouncementDetail" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="clear:both;height:20px"></div>


    <div class="well well-small">
	                <div class="well-title well-style2 clearfix">
		                <div class="pull-left">
			                <h3>Duyuru Detayı</h3>
		                </div>
		                <div class="pull-right">
			                <a href="/Announcements.aspx" class="btn btn-link">
							Tüm Duyurular</a>
		                </div>
	                </div>
	                <div class="announcement-detail">
	                	<div class="page-header">
                            <!--Title--><h3><asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h3>
                        </div>
                        
                        <!-- <p class="lead"> -->
	                        <!--Spot Metin-->
						<!-- </p> -->

						<div class="navbar nav-actions">
                            <div class="navbar-inner">
                                <asp:Literal ID="ltrAnnContent" runat="server"></asp:Literal>
                            </div>
                        </div>						
                        

	                </div>
                </div>


        

</asp:Content>
