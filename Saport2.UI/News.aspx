<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="Saport2.UI.News" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>
    <div style="clear:both;height:20px"></div>

    <asp:UpdatePanel ID="UpdatePanel1"  runat="server" UpdateMode="Conditional" >
        <ContentTemplate>



    <!-- News Top BGEIN -->
    <div class="well well-small well-site-panel">
    <div id="ctl00_ctl41_g_72a7be68_10e6_466f_a97b_326743556427_ctl00_pnlSite" class="well-title clearfix well-style1">		
        <div class="pull-left">
            <h3>Haberler</h3>
        </div>
    </div>

    <div class="content">
        <div class="pull-right site-panel-logo">
            <img id="ctl00_ctl41_g_72a7be68_10e6_466f_a97b_326743556427_ctl00_imgSiteImage" src="resources/img/i_haberler.png"/>
        </div>
        <p>
            Sabancı Topluluğu’nda neler oluyor, gündeme ilişkin ayrıntılar neler, şirketlerden haberler, güncel gelişmeler, hepsi artık tek bir yerde toplanıyor. Haberler bölümü, SAPORT’un en fazla ilgi çekecek bölümlerinden olacak.
        </p>
        </div>
    </div>
    <!-- News Top END -->

    <div style="clear:both;height:20px"></div>

    <!-- News Body BEGIN -->
    <div class="box">
       <div class="green-title">Haberler</div>
        	
       <div class="blog-post-list"  style="">
            <asp:Literal ID="ltrMain" runat="server"></asp:Literal>
            <asp:Literal ID="ltrAddition" runat="server"></asp:Literal>
       </div>
       <br />
       <asp:Button ID="Button1" runat="server" Text="daha fazla haber yükle" OnClick="Button1_Click" class="loadMore"/>
</div>

</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Button1" />
    </Triggers>
</asp:UpdatePanel>


</asp:Content>
