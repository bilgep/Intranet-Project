using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.UI
{
    public partial class Banner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string htmlBannerItem = "<div data-index=\"0\" style=\"width: 656.656px; left: 0px; transition-duration: 0ms; transform: translate(0px, 0px) translateZ(0px);\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"0\" height=\"0\" border=\"0\" alt=\"\" /></a></div>";
            List<BannerModel.Banner> banners = BannerService.QueryLatestBanners();
            foreach (var item in banners)
            {
                ltrMain.Text += string.Format(htmlBannerItem, item.RedirectPage,BannerService.GetImageUrl(item.ImageUrl));
            }

        }
    }
}