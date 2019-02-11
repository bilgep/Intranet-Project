<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="PostDetail.aspx.cs" Inherits="Saport2.UI.PostDetail" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="resources/styles/style2.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box">
    <div class="green-title" id="boxTitle" runat="server">Köşe Yazısı: Detay</div>
    <div class="blog-post-list" style="">
    <asp:Literal ID="ltrMain" runat="server"></asp:Literal>
    </div>
    </div>
    
</asp:Content>
