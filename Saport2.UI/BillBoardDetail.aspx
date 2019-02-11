<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="BillBoardDetail.aspx.cs" Inherits="Saport2.UI.BillBoardDetail" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link rel="stylesheet" href="resources/styles/demo.css" type="text/css" media="screen" />
	<link rel="stylesheet" href="resources/styles/flexslider.css" type="text/css" media="screen" />

	<!-- Modernizr -->
  <script src="resources/scripts/modernizr.js"></script>

  <script defer src="resources/scripts/jquery.flexslider.js"></script>
  <!-- Syntax Highlighter -->
  <script type="text/javascript" src="resources/scripts/shCore.js"></script>
  <script type="text/javascript" src="resources/scripts/shBrushXml.js"></script>
  <script type="text/javascript" src="resources/scripts/shBrushJScript.js"></script>

  <!-- Optional FlexSlider Additions -->
  <script src="resources/scripts/jquery.easing.js"></script>
  <script src="resources/scripts/jquery.mousewheel.js"></script>
  <script defer src="resources/scripts/demo.js"></script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


        <div class="box">
        	<div class="green-title">İlan Detayı</div>

            <asp:Panel ID="pnlImages" runat="server">
               <div class="grid-6" style="padding:20px;">
				<div id="slider" class="flexslider">
					<ul class="slides">
                        <asp:Literal ID="ltrAnnouncementImages" runat="server"></asp:Literal>
					</ul>
				</div>
                <div id="carousel" class="flexslider">
					<ul class="slides">
                        <asp:Literal ID="ltrAnnouncementImages2" runat="server"></asp:Literal>
					</ul>
				</div>
                   <script type="text/javascript">
					$(function(){
					  SyntaxHighlighter.all();
					});
					$(window).load(function(){
					  $('#carousel').flexslider({
						animation: "slide",
						controlNav: false,
						animationLoop: false,
						slideshow: false,
						itemWidth: 110,
						itemMargin: 5,
						asNavFor: '#slider'
					  });

					  $('#slider').flexslider({
						animation: "slide",
						controlNav: false,
						animationLoop: false,
						slideshow: false,
						sync: "#carousel",
						start: function(slider){
						  $('body').removeClass('loading');
						}
					  });
					});
				  </script>
			</div>
            </asp:Panel>



			<div class="grid-6" style="padding:20px;">
			
				<table class="table table-striped">
										<tbody><tr>
											<th><span>Ürün Adı</span></th>
											<td>
												<!--Title-->
												<asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
											</td>
										</tr>
										
										<tr>
											<th><span>İlan Tarihi: </span></th>
											<td>
					                        	<!--Oluşturulma Tarihi-->
                                                <asp:Literal ID="ltrDateCreated" runat="server"></asp:Literal>
											</td>
										</tr>
										<tr>
											<th><span>Fiyatı: </span></th>
											<td>
												<!--Fiyat-->
												<asp:Literal ID="ltrPrice" runat="server"></asp:Literal>
												 TL
											</td>
										</tr>
										<tr>
									        <th>
									        	<span>Kategori: </span>
									        </th>
									        <td>
									        	<!--İlan Kategorisi-->
									        	<strong class="colorBlue">
									        	<span title="Diğer"><asp:Literal ID="ltrCategory" runat="server"></asp:Literal></span>

												</strong>
									        </td>
									    </tr>
									    <tr>
									    	<th><span>Açıklama</span></th>
									    	<td>
												<!--HTML Detay - Page Content-->
					                            <asp:Literal ID="ltrDetails" runat="server"></asp:Literal>
									    	</td>
									    </tr>
									    <tr>
									    	<th>
									    		<span>Kişisel Bilgiler</span>
									    	</th>
									    	<td>
										    	<!--Kişisel Bilgiler-->
									    		<!--Spot Metin-->
											    <asp:Literal ID="ltrSpotText" runat="server"></asp:Literal>
									    	</td>
									    </tr>
									</tbody></table>
			
			</div>
		
		<div style="clear:both"></div>
			
			
        </div>
</asp:Content>
