<%@ Page Title="" Language="C#" MasterPageFile="~/Saport2.Master" AutoEventWireup="true" CodeBehind="Campaigns.aspx.cs" Inherits="Saport2.UI.Campaigns" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>
    <div style="clear:both;height:20px"></div>

        <div class="well well-small well-site-panel">
		        <div class="well-title clearfix well-style3">
                    <i class="arrow"></i>
                    <div class="pull-left">
                    <h3>Kampanyalar</h3>
                    </div>
				</div>

                <div class="content">
                    <div class="pull-right site-panel-logo">
                        <img src="../resources/img/campaign-site-logo.png"/>
                    </div>
                    <p>
                        Sabancı Topluluğu çalışanları için özel kampanyalar, indirimler ve avantajlı alışveriş fırsatları burada. Gözünü Kampanyalar bölümünden ayırmayın, sürekli güncellenen fırsatlardan habersiz kalmayın...
                    <br/><br/>                    
                    <font size="1">
                    ÖNEMLİ AÇIKLAMA:
                    “İşbu portalda yer alan ve ilgili firmalarca sunulan hizmetler ve/veya ürünlere  ilişkin olarak  tüketici mevzuatı başta olmak üzere ilgili kanunlar çerçevesinde ileri sürülecek her türlü talep ve/veya şikayetin doğrudan ürünü ve/veya hizmeti sunan, kampanyayı düzenleyen firmaya iletileceği hususu işbu bu portalden bilgilenmek yoluyla hizmet ve/veya ürünü tedarik eden kişilerce kabul edilmektedir.”</font>
                    </p>
                </div>
                </div>

        <div style="clear:both;height:20px"></div>

        <!-- <div class="grid-12 grid-tl-12 grid-t-12">

				<div id='mySwipe' class='swipe'>
				  <div class='swipe-wrap'>
                    <asp:Literal ID="ltrBanner" runat="server"></asp:Literal> !!! BANNER'Lar query ile alınacak ve literal'e yazdırılacak

					<div><img src="img/asd1.jpg" width="0" height="0" border="0" alt=""/></div>
					<div><img src="img/asd2.jpg" width="0" height="0" border="0" alt=""/></div>
				  </div>
                    <%--<button onclick='mySwipe.prev()' class="left"></button> 
					<button onclick='mySwipe.next()' class="right"></button>--%>
				</div>
                <script src="resources/scripts/swipe.js"></script>

				<script>

				// pure JS
				var elem = document.getElementById('mySwipe');
				window.mySwipe = Swipe(elem, {
				  // startSlide: 4,
				   auto: 3000,
				  // continuous: true,
				  // disableScroll: true,
				  // stopPropagation: true,
				  // callback: function(index, element) {},
				  // transitionEnd: function(index, element) {}
				});
				// with jQuery
				// window.mySwipe = $('#mySwipe').Swipe().data('Swipe');

				</script>
				<style style="text/css">
					.swipe-wrap img {width:100%;height:100% !important;min-height:200px !important;border: 0;object-fit: cover;border-radius:5px;}
				</style>

        </div> -->

        <div style="clear:both;height:20px"></div>

    <asp:UpdatePanel ID="UpdatePanel1"  runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <div class="kampanyalar box2">
        	<div class="green-title">Kampanyalar</div>
        	<ul>
            	<asp:Literal ID="ltrMain" runat="server"></asp:Literal>
                <asp:Literal ID="ltrAddition" runat="server"></asp:Literal>
            </ul>
            <br />
            <br />




            <asp:Button ID="Button1" runat="server" Text="daha fazla kampanya yükle" OnClick="Button1_Click" class="loadMore"/>
            <div style="clear:both;"></div>
        </div>
        </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Button1" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>
