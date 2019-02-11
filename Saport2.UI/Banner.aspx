<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Banner.aspx.cs" Inherits="Saport2.UI.Banner" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link href="resources/styles/style.css" rel="stylesheet" />
    <link href="resources/styles/gridsa.css" rel="stylesheet" />
    <script src="resources/scripts/jquery-1.12.2.min.js"></script>
    <script src="resources/scripts/jquery.newsTicker.min.js"></script>
    <script src="resources/scripts/knockout-3.4.0.js"></script>
    <script src="resources/scripts/pSlider.js"></script>
    <script src="resources/scripts/pSlider.min.js"></script>
    <script src="resources/scripts/saport2main.js"></script>
    <script src="resources/scripts/skdslider.js"></script>
    <script src="resources/scripts/swipe.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="row">
                <div class="grid-9 grid-tl-8 grid-t-12">

                    <div id="mySwipe" class="swipe" style="visibility: visible;">
                        <div class="swipe-wrap" style="width: 3283.28px;">
                            <asp:Literal ID="ltrMain" runat="server"></asp:Literal>

<%--                            <div data-index="0" style="width: 656.656px; left: 0px; transition-duration: 0ms; transform: translate(0px, 0px) translateZ(0px);">
                                <a href="Posts.aspx">
                                    <img src="resources/img/160216_holding_700x329.jpg" width="0" height="0" border="0" alt="" /></a>
                            </div>
                            <div data-index="0" style="width: 656.656px; left: 0px; transition-duration: 0ms; transform: translate(0px, 0px) translateZ(0px);">
                                <a href="Posts.aspx">
                                    <img src="resources/img/160216_temsa_700x329.jpg" width="0" height="0" border="0" alt="" /></a>
                            </div>--%>

                        </div>
                        <button type="button" onclick="mySwipe.prev()" class="left"></button>
                        <button type="button" onclick="mySwipe.next()" class="right"></button>
                    </div>
                    <script>

                        // pure JS
                        var elem = document.getElementById('mySwipe');
                        window.mySwipe = Swipe(elem, {
                            //startSlide: 4,
                            //auto: 3000,
                            //continuous: true,
                            disableScroll: true,
                            stopPropagation: true,
                            callback: function (index, element) { },
                            transitionEnd: function (index, element) { }
                        });

                        // with jQuery
                        // window.mySwipe = $('#mySwipe').Swipe().data('Swipe');

                    </script>

                </div>

            </div>


        </div>


    </form>
</body>
</html>
