<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="PortalFeeds.aspx.cs" Inherits="Saport2.UI.PortalFeeds" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="clear:both;height:20px"></div>

    <asp:UpdatePanel ID="UpdatePanel3"  runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <div class="box">
        	    <div class="green-title">SAPORT'ta Neler Oluyor?</div>
        	    <div class="feed-list">
                    <ul class="unstyled" style="">
                        <asp:Literal ID="ltrMain" runat="server"></asp:Literal>
                        <asp:Literal ID="ltrAddition" runat="server"></asp:Literal>
                    </ul>
                </div>
                <asp:Button ID="Button1" runat="server" Text="daha fazla gelişme yükle" OnClick="Button1_Click" class="loadMore"/>
            </div>

        </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Button1" />
    </Triggers>
    </asp:UpdatePanel>

</asp:Content>
