<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Saport2.UI.Home" Async="true"  asyncTimeout="30" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="resources/scripts/swipe.js"></script>
    <script src="resources/scripts/jquery-1.12.2.min.js"></script>
    <script src="resources/scripts/jquery.newsTicker.min.js"></script>
    <script src="resources/scripts/knockout-3.4.0.js"></script>
    <script src="resources/scripts/pSlider.js"></script>
    <script src="resources/scripts/pSlider.min.js"></script>
    <script src="resources/scripts/saport2main.js"></script>
    <script src="resources/scripts/skdslider.js"></script>

    <script src="resources/scripts/jquery.newsTicker.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <%--<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>

    <div style="clear:both;height:20px"></div>
        <div class="row">
        <div class="grid-8 grid-tl-8 grid-t-12">

				<div id='mySwipe' class='swipe'>
				  <div class='swipe-wrap'>
                      <asp:Literal ID="ltrBanners" runat="server"></asp:Literal>
					
				  </div>
				    <input type="button" onclick='mySwipe.prev()' class="left"/>
					<input type="button" onclick='mySwipe.next()' class="right"/>
				</div>

				<script src="resources/scripts/swipe.js"></script>
				<script>

				// pure JS
				var elem = document.getElementById('mySwipe');
				window.mySwipe = Swipe(elem, {
				  // startSlide: 4,
				   auto: 9000,
				   continuous: true,
				  // disableScroll: true,
				  // stopPropagation: true,
				  // callback: function(index, element) {},
				  // transitionEnd: function(index, element) {}
				});

				// with jQuery
				// window.mySwipe = $('#mySwipe').Swipe().data('Swipe');

				</script>

        </div>
        <div class="grid-4 grid-tl-4 grid-t-12">
        	<div class="well" id="PortalFeeds">
            	<div class="feed-list">
                    <div style="padding-left:10px;">
                        <h3>SAPORT'ta Neler Oluyor?</h3>
                    </div>
        		<ul class="unstyled" style="height: 190px; overflow: hidden;">
                    <asp:Literal ID="ltrPortalFeeds" runat="server"></asp:Literal>
                </ul>
                <a href="PortalFeeds.aspx" class="all-link"><b>»</b> Tümünü görüntüle</a>
    		</div>
            </div>
            <script type="text/javascript" src="js/jquery.newsTicker.min.js"></script>
            <script type="text/javascript">
				$('.feed-list ul.unstyled').newsTicker({
					row_height: 43,
					max_rows: 5,
					speed: 600,
					direction: 'up',
					duration: 4000,
					autostart: 1,
					pauseOnHover: 0
				});
			</script>
        </div>
        </div>

        <!--<div style="clear:both;height:20px"></div>-->

        <div style="clear:both; height:20px;"></div>

        <div class="row">
        <div class="grid-6"><div class="well well-none">
            <div id="authorsControl">
                <h5>Köşe Yazıları</h5>
                <asp:Literal ID="ltrPosts" runat="server"></asp:Literal>
                    <div class="clearfix">
                    <a href="/Posts.aspx" class="btn btn-link pull-right" style="padding-top:10px">Tümünü Görüntüle</a>
                </div>
            </div>
        </div>
		</div>
        <div class="grid-6">
        	<div class="kampanyalar box3" runat="server" id="divCampaigns">
                <h5>Kampanyalar</h5>
        	<ul>
                <asp:Literal ID="ltrCampaigns" runat="server"></asp:Literal>
                 <div class="clearfix">
                    <a href="/Campaigns.aspx" class="btn btn-link pull-right">Tümünü Görüntüle</a>
                <div style="clear:both; height:10px;"></div>
            </ul>
                
        </div>
        </div>
        </div>
</asp:Content>
