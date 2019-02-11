<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="BillBoard.aspx.cs" Inherits="Saport2.UI.BillBoard" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box">
        	<div class="green-title">İlan Panosu</div>	
			<div class="ads-categories">
                <asp:Literal ID="ltrAdCategoryLinks" runat="server"></asp:Literal>
			</div>
        </div>
</asp:Content>
