<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="Posts.aspx.cs" Inherits="Saport2.UI.Posts" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="resources/styles/style2.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel3"  runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

            <div class="blog-categories">
            <asp:Literal ID="ltrBlogCategories" runat="server"></asp:Literal>
            </div>

            <div style="clear:both;height:20px"></div>

            <div class="box">
        	<div class="green-title" id="boxTitle" runat="server">Yazılar</div>
        	
            <div class="blog-post-list" style="">

            <asp:Literal ID="ltrMain" runat="server"></asp:Literal>

        </div>
             <asp:Button ID="Button1" runat="server" Text="daha fazla köşe yazısı yükle" OnClick="Button1_Click" class="loadMore" />
        </div>


        </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID = "Button1" />
    </Triggers>
    </asp:UpdatePanel>

</asp:Content>
