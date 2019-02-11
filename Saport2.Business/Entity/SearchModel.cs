using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saport2.Business.Entity
{
    public class SearchModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        public enum ContentTypeNames
        {
            Announcement,
            Campaign,
            News,
            Post,
            // Advert
        }

        //public static List<string> contentTypeIds = new List<string>() { "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39007A570E0F151646A9A11AC91712157BF900711007995E05534B94F2021C7A6D7324;0;duyuru;/AnnouncementDetail.aspx?AnnId={0}", "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39007FFDEB9A82364488971B0F14FF2FBC3300311BD56D845B0243AABADC1916DAE357;0;kampanya;/CampaignDetail.aspx?CampId={0}", "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF3900631F350EEC9E4D43A3652A78FA49C16F0051B551467F958C48BAC19C36502EC960;0;haber;/NewsDetail.aspx?NewsId={0}", "0x011000F5BC65C656DB2B4DB941574D7377A512;1;saglik;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000A0B47F912CBFC149B98FA05181E7617D;1;hobiyazilari;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000E397E50CF77CEC48819B07B0B6883F99;1;kisiselgelisim;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000495D284BCED3764EB9F36614E7364E20;1;kultursanat;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000B5702A52D7F5F84089A32495E32FAEE4;1;teknoloji;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000657599D92B6A0F42A4318B44EB1C6754;1;gezi;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39001325BAC5EA5A451585457AE50009F0C6008997E48048DAE24B9986E4024E2164B0;0;ilan;/BillBoardDetail.aspx?AdvertId={0}" };
        // NOT: İlanlar aramalarda hariç tutuldu.

        public static List<string> contentTypeIds = new List<string>() { "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39007A570E0F151646A9A11AC91712157BF900711007995E05534B94F2021C7A6D7324;0;duyuru;/AnnouncementDetail.aspx?AnnId={0}", "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39007FFDEB9A82364488971B0F14FF2FBC3300311BD56D845B0243AABADC1916DAE357;0;kampanya;/CampaignDetail.aspx?CampId={0}", "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF3900631F350EEC9E4D43A3652A78FA49C16F0051B551467F958C48BAC19C36502EC960;0;haber;/NewsDetail.aspx?NewsId={0}", "0x011000F5BC65C656DB2B4DB941574D7377A512;1;saglik;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000A0B47F912CBFC149B98FA05181E7617D;1;hobiyazilari;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000E397E50CF77CEC48819B07B0B6883F99;1;kisiselgelisim;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000495D284BCED3764EB9F36614E7364E20;1;kultursanat;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000B5702A52D7F5F84089A32495E32FAEE4;1;teknoloji;/PostDetail.aspx?PostId={0}&PostCat={1}", "0x011000657599D92B6A0F42A4318B44EB1C6754;1;gezi;/PostDetail.aspx?PostId={0}&PostCat={1}" };


        #endregion

        #region Statics
        // Query Texts
        public static string RestfulKqlQuery = "https://saport.x.com/_api/search/query?querytext='{0}'&rowlimit=20&selectproperties='ContentTypeId%2c+Created%2c+Title%2c+ListItemID% 2cFileExtension'&sortlist='Created:descending'&refinementfilters='fileextension:equals(#aspx#)'clienttype='ContentSearchRegular'";

        public static string resultItemHtmlTemplate = "<div style='font-size:16px'><a href='{0}'>{1}</a></div></br>";
        #endregion

        #region Properties & Fields

        public string ContentTypeName { get; set; }
        public int ContentTypeCode { get; set; }
        public string ContentTypeId { get; set; }
        public string ListItemId { get; set; }
        public string Title { get; set; }
        public string RedirectUrl { get; set; }
        public string RedirectUrlComplete
        {
            get
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(this.RedirectUrl))
                {
                    if (ContentTypeCode == 0)
                    {
                        result = string.Format(this.RedirectUrl, ListItemId);
                    }
                    else if (ContentTypeCode == 1)
                    {
                        result = string.Format(this.RedirectUrl, ListItemId, ContentTypeName);
                    }
                }

                return result;
            }
        }
        #endregion
    }
}
