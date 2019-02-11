using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Saport2.Business.Entity
{
    public class PageModel
    {

        #region About
        /* It includes fields and properties of PageModel */
        #endregion

        #region Enums and Statics
        // Enums
        internal static string master = "Saport2.Master";
        public static Dictionary<string,string> pageNamePageTitle= new Dictionary<string,string> {
            { "Home","Ana Sayfa" },
            { "Announcements", "Duyurular" },
            { "AnnouncementDetail", "Duyuru - Detay" },
            { "Billboard", "İlan Panosu" },
            { "BillboardCategory", "İlan - Kategori" },
            { "BillboardDetail", "İlan - Detay" },
            { "Campaigns", "Kampanyalar" },
            { "CampaignDetail", "Kampanya- Detay" },
            { "Error", "Hata" },
            { "Login", "Oturum Aç" },
            { "News", "Haberler" },
            { "NewsDetail", "Haber - Detay" },
            { "PortalFeeds", "SAPORT'ta Neler Oluyor" },
            { "Posts", "Köşe Yazıları" },
            { "PostDetail", "Köşe Yazısı - Detay" },
            { "Saport2Users", "Saport Mavi Yaka - Yönetim Ekranı" },
            { "Search", "Arama" },
        };

        public static Dictionary<string, string> menuItems = new Dictionary<string, string> {
            { "Announcements", "Duyurular" },
            { "Billboard", "İlan Panosu" },
            { "Campaigns", "Kampanyalar" },
            { "News", "Haberler" },
            { "PortalFeeds", "SAPORT'ta Neler Oluyor" },
            { "Posts", "Köşe Yazıları" }
        };



        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        // Properties
        public static Page ThisPage { get; set; }
        #endregion

    }
}
