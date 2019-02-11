<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Saport2.UI.Error" Async="true"  asyncTimeout="30" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box">
    <div class="green-title">Hata</div>
        	
    <div class="news-detail">
           Bir sorun oluştu.
        <input type="hidden" id="errorMessage" runat="server" />
    </div>
    </div>

</asp:Content>
