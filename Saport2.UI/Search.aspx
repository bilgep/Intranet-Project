<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Saport2.UI.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box">
    <div class="green-title">Arama Sonucu</div>
        	
    <div class="news-detail">

<%--	                	<div class="page-header">
	                        <h3>
		                        <!--Title-->
                                <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
	                        </h3>
                        </div>--%>
                        <br/>

                        <asp:Literal ID="ltrResult" runat="server"></asp:Literal> 

        </div>
     </div>
</asp:Content>
