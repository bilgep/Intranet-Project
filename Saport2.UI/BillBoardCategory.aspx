<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="BillBoardCategory.aspx.cs" Inherits="Saport2.UI.BillBoardCategory" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="well well-small well-site-panel">
        <div class="well-title well-style1 clearfix">
            <div class="pull-left">
                <h3><asp:Literal ID="ltrAdCat" runat="server"></asp:Literal></h3>
            </div>
        </div>
        <div class="content">
            <div class="all-advert-list">
                <div id="ctl00_ctl41_g_b707dd10_694d_47e4_8be4_620278ceb39e_ctl00_pnlFollow" class="clearfix">
                        <ul class="thumbnails">
                            <asp:Literal ID="ltrMain" runat="server"></asp:Literal>
                        </ul>
                 </div>
            </div>
        </div>
    </div>


</asp:Content>
