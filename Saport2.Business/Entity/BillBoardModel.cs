using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Saport2.Business.Entity
{
    public class BillBoardModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums

        #endregion

        #region Statics & Constants
        public const string sayfalarListName = "Sayfalar";
        public const string goruntulerListName = "Görüntüler";
        public const string advertsSiteUrl =   "https://saport.x.com/tr-tr/ilanpanosu/";
        public const string defaultImageBase64 = "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";
        public static string thumbnailImageRemoteUrl = Data.DataStatics.saportHostURL + "/tr-tr/ilanpanosu/PublishingImages/{0}/{1}"; // 0 = Id, 1 = filename
        public const string thumbnailImageLocalUrl = "../temp/{0}";
        public const string advertsSaveFileNameForCategoryDiger = "AdvertsDiger.xml";
        public const string advertsSaveFileNameForCategoryElektronik = "AdvertsElektronik.xml";
        public const string advertsSaveFileNameForCategoryEmlak = "AdvertsEmlak.xml";
        public const string advertsSaveFileNameForCategoryKisiselUrunler = "AdvertsKisiselUrunler.xml";
        public const string advertsSaveFileNameForCategoryVasita = "AdvertsVasita.xml";
        public const string advertssSaveFolder = "temp";

        public const string categoryAdvertsCamlQuery = @"<View>
        <ViewFields><FieldRef Name='ID'/><FieldRef Name='DefaultImage'/><FieldRef Name='Title'/><FieldRef Name='Created'/></ViewFields>
        <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy>
        <Where><Eq><FieldRef Name='AdCategory'/><Value Type='Text'>{0}</Value></Eq></Where></Query>
        <RowLimit>5</RowLimit></View>";
        public const string categoryAdvertsLimitedCamlQuery = @"<View>
        <ViewFields><FieldRef Name='ID'/><FieldRef Name='DefaultImage'/><FieldRef Name='Title'/></ViewFields>
        <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='AdCategory'/><Value Type='Text'>{0}</Value></Eq></Where></Query>
        <RowLimit>{1}</RowLimit></View>";
        public const string advertDetailCamlQuery = @"<View>
        <ViewFields><FieldRef Name='Created'/><FieldRef Name='GenericDescription'/><FieldRef Name='GenericDetail'/><FieldRef Name='AdCategory'/><FieldRef Name='Title'/><FieldRef Name='Price'/></ViewFields>
        <Query><OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='ID'/><Value Type='Number'>{0}</Value></Eq></Where></Query>
        <RowLimit>1</RowLimit>
        </View>";
        public const string advertDetailForServiceCamlQuery = @"<View>
        <ViewFields><FieldRef Name='Created'/><FieldRef Name='GenericDescription'/><FieldRef Name='GenericDetail'/><FieldRef Name='AdCategory'/><FieldRef Name='Title'/><FieldRef Name='Price'/></ViewFields>
        <Query><OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy><Where><And><Eq><FieldRef Name='AdCategory'/><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='ID'/><Value Type='Number'>{1}</Value></Eq></And></Where></Query>
        <RowLimit>1</RowLimit>
        </View>";


        public const string advertListItemHTMLString = "<li class=\"grid-6\"><div class=\"thumbnail\"><a href=\"{0}\" class=\"mask\"><img src=\"data:image/png;base64,{1}\"></a><div class=\"caption\"><h3><a href=\"{2}\">{3}</a></h3></div></div></li>";

        //public static string catAdvertItemHtml = "<a class=\"btn btn-link diger {0}\" href=\"{1}\" style=\"background-image:url('resources/img/billboard/{2}.png'); \">_____________</a>";

        public const string catAdvertItemHtml = "<a href=\"{0}\" \"><img src=\"resources/img/billboard/{1}.png\"/></a>";
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        public class AdvertImage
        {
            public int AdvertId { get; set; }
            public string Image { get; set; }
        }


        [XmlRoot("Advert")]
        public class Advert
        {
            public static List<string> adCategories = new List<string>() { "Diğer", "Elektronik", "Emlak", "Kişisel Ürünler", "Vasıta" };

            [XmlElement("ID")]
            public int? ID { get; set; }

            [XmlElement("Category")]
            public string Category { get; set; } 

            public string Description { get; set; } 
            public string Detail { get; set; } 
            public decimal Price { get; set; }

            [XmlElement("DefaultImage")]
            public string DefaultImage { get; set; }

            public List<string> ImageUrls { get; set; }

            public DateTime Created { get; set; }

            [XmlElement("Title")]
            public string Title { get; set; }
            public string DetailPageUrl { get { return string.Format(DetailRedirectPage, ID, Category); } }
            public static string CategoryRedirectPage { get { return "/BillBoardCategory.aspx?Cat={0}"; } }
            public static string DetailRedirectPage { get { return "/BillBoardDetail.aspx?AdvertId={0}&Cat={1}"; } }
        }
        #endregion
    }
}
