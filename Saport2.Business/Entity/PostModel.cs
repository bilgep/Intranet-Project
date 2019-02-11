using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using SP = Microsoft.SharePoint.Client;

namespace Saport2.Business.Entity
{
    public class PostModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Statics & Constants
        public const string categoryListName = "Blog Yönetimi";
        public const string postsListName = "Postalar";
        public const string postsMainSiteUrl = "https://saport.x.com/tr-tr/koseyazilari/";
        public const string postsSiteUrl = "https://saport.x.com/tr-tr/koseyazilari/{0}/";
        public const string postCategoriesSaveFolder = "temp";
        public const string postCategoriesSaveFileName = "PostCategories.xml";
        public const string postsLightSaveFolder = "temp";
        public const string postsLightFileName = "PostsLight.xml";
        public const string postCategoryDetailUrl = "../Posts.aspx?PostCat={0}";
        public const string postDetailUrl = "../PostDetail.aspx?PostId={0}&PostCat={1}";
        public static string postCategoriesSaveLocation = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + "PostCategories.xml";
        public static string postsLightSaveLocation = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + "PostsLight.xml";

        public const string getCategoriesCamlQuery = "<View><ViewFields><FieldRef Name='ID'/><FieldRef Name='BlogUrl'/><FieldRef Name='Title'/></ViewFields><Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query><RowLimit>50</RowLimit></View>";
        public const string getPostsCamlQueryLight = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='PublishedDate'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='PublishedDate' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>100</RowLimit>
                                  </View>";
        public const string getPostsCamlQueryLightLimited = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='PublishedDate'/><FieldRef Name='Title'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='PublishedDate' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>{0}</RowLimit>
                                  </View>";
        public const string getPostsCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='Body'/><FieldRef Name='PublishedDate'/><FieldRef Name='Author'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query>
                                    <RowLimit>3</RowLimit>
                                  </View>";
        public const string postDetailCamlQuery = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='PublishedDate'/><FieldRef Name='Author'/><FieldRef Name='Body'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='ID'/><Value Type='Number'>{0}</Value></Eq></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string postDetailCamlQueryByTitle = @"<View>
                                    <ViewFields><FieldRef Name='ID'/><FieldRef Name='Title'/><FieldRef Name='PublishedDate'/><FieldRef Name='Author'/><FieldRef Name='Body'/></ViewFields>
                                    <Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>{0}</Value></Eq></Where></Query>
                                    <RowLimit>1</RowLimit>
                                  </View>";
        public const string htmlDivPostsForHomePage = "<div class=\"media\"><a class=\"pull-right\" href=\"{0}\"><img src=\"resources/img/ikSelfServisUygulamalari.jpg\" class=\"media-object\"/></a><div class=\"media-body\"><a href =\"{1}\">{2} <b>»</b></a></div></div>";
        public const string htmlForPostCategories = "<a href=\"{0}\" class=\"btn btn-link\"><span>{1}</span></a>";
        public const string htmlForPostsPage = "<div class=\"media\"><a class=\"pull-left post-icon\" href=\"#\"><img  class=\"media-object\" src=\"resources/img/ikSelfServisUygulamalari.jpg\"></a><div class=\"media-body\"><h4 class=\"media-heading\"><!-- Redirect Url + Title --><a href=\"{0}\">{1}</a></h4><div class=\"media\"><div><b>Yazar: </b><!-- Author --><span >{2}</span> | <b>Kategori : </b><!-- Category URL + Category Name --><a href=\"{3}\">{4}</a> | <b>Tarih: </b><!-- Date --><span >{5}</span></div><p></p><!-- Content BEGIN -->{6}<!-- Content END --><p></p><div class=\"navbar nav-actions\"><div class=\"navbar-inner\"><ul class=\"nav pull-right\"><li><!-- Redirect Url --><a href=\"{7}\">Yazının Devamı<i class=\"icon-angle-double-right\"></i></a></li></ul></div></div></div></div></div>";
        public const string htmlForCategoryPostsPage = "<div class=\"media\"><a class=\"pull-left post-icon\" href=\"#\"><img  class=\"media-object\" src=\"resources/img/ikSelfServisUygulamalari.jpg\"></a><div class=\"media-body\"><h4 class=\"media-heading\"><!-- Redirect URL + Title --><a href=\"{0}\">{1}</a></h4><div class=\"media\"><div><b>Yazar: </b><!-- Author --><span >{2}</span> | <b>Tarih: </b><!-- Date --><span >{3}</span></div><p></p><!-- Content BEGIN -->{4}<!-- Content END --><p></p><div class=\"navbar nav-actions\"><div class=\"navbar-inner\"><ul class=\"nav pull-right\"><li></li></ul></div></div></div></div></div>";
        public const string htmlForPostDetailPage = "<div class=\"media\"><a class=\"pull-left post-icon\" href=\"#\"><img  class=\"media-object\" src=\"resources/img/ikSelfServisUygulamalari.jpg\"></a><div class=\"media-body\"><h4 class=\"media-heading\"><!-- Title -->{0}</h4><div class=\"media\"><div><b>Yazar: </b><!-- Author --><span >{1}</span> | <b>Tarih: </b><!-- Date --><span >{2}</span></div><p></p><!-- Content BEGIN -->{3}<!-- Content END --><p></p><div class=\"navbar nav-actions\"><div class=\"navbar-inner\"><ul class=\"nav pull-right\"><li></li></ul></div></div></div></div></div>";
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        [Serializable, XmlRoot("PostCategory")]
        public class PostCategory
        {
            [XmlElement("CategoryName")]
            public string CategoryName { get; set; }

            [XmlElement("CategoryTitle")]
            public string CategoryTitle { get; set; }
        }



        [Serializable, XmlRoot("Post")]
        public class Post
        {
            private string _categoryName;
            private string _author;

            [XmlElement("CategoryName")]
            public string CategoryName {
                get
                {
                    return _categoryName;
                }
                set { _categoryName = value.ToString(); }
            }

            [XmlElement("CategoryTitle")]
            public string CategoryTitle
            {
                get
                {
                    return PostService.GetCategoryTitleByCategoryName(CategoryName);
                }
            }

            [XmlElement("Id")]
            public int Id { get; set; }

            [XmlElement("Body")]
            public string Body { get; set; }

            [XmlElement("Title")]
            public string Title { get; set; }

            [XmlElement("PublishedDate")]
            public DateTime PublishedDate { get; set; }

            [XmlElement("Author")]
            public string Author
            {
                get { return _author; }
                set
                {
                    if (value.Contains("stajyer"))
                    { _author = "Saport Admin"; }
                    else
                    { _author = value; }

                }
            }

        }
        #endregion
    }
}
