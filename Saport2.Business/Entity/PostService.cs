using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT = Saport2.Data;
using HLP = Saport2.Shared.Helpers;
using SP = Microsoft.SharePoint.Client;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.Business.Entity
{
    public class PostService : PostModel
    {
        #region Exceptional 

        //public static SP.ListItemCollection QueryListItems(string siteUrl, string listName, string queryText)
        //{
        //    try
        //    {
        //        SP.ClientContext clientContext = new SP.ClientContext(siteUrl);
        //        SP.List oList = clientContext.Web.Lists.GetByTitle(listName);
        //        SP.CamlQuery camlQuery = new SP.CamlQuery();
        //        camlQuery.ViewXml = queryText;
        //        SP.ListItemCollection collListItems = oList.GetItems(camlQuery);
        //        clientContext.Load(collListItems);
        //        clientContext.ExecuteQuery();
        //        clientContext.Dispose();
        //        return collListItems;

        //    }
        //    catch (Exception ex)
        //    {
        //        EXP.RedirectToErrorPage(ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region Methods
        public static List<PostCategory> QueryPostCategories()
        {
            try
            {
                List<Saport2.Business.Entity.PostModel.PostCategory> categories = new List<Saport2.Business.Entity.PostModel.PostCategory>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(postsMainSiteUrl, categoryListName, getCategoriesCamlQuery);

                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Saport2.Business.Entity.PostModel.PostCategory catgry = new Saport2.Business.Entity.PostModel.PostCategory();
                        catgry.CategoryName = item["BlogUrl"].ToString();
                        catgry.CategoryTitle = item["Title"].ToString();
                        categories.Add(catgry);
                    }
                }

                return categories;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<PostCategory> QueryPostCategories(string postsMainSiteUrl, string categoryListName, string getCategoriesCamlQuery)
        {
            try
            {
                List<Saport2.Business.Entity.PostModel.PostCategory> categories = new List<Saport2.Business.Entity.PostModel.PostCategory>();
                SP.ListItemCollection coll = DAT.DataQuery.QueryListItems(postsMainSiteUrl, categoryListName, getCategoriesCamlQuery);

                if (coll.Count > 0)
                {
                    foreach (SP.ListItem item in coll)
                    {
                        Saport2.Business.Entity.PostModel.PostCategory catgry = new Saport2.Business.Entity.PostModel.PostCategory();
                        catgry.CategoryName = item["BlogUrl"].ToString();
                        catgry.CategoryTitle = item["Title"].ToString();
                        categories.Add(catgry);
                    }
                }

                return categories;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Post> QueryAllLatestPostsLight()
        {
            try
            {
                List<Post> posts = new List<Post>();
                //List<PostCategory> categories = QueryPostCategories();
                List<PostModel.PostCategory> categories = PostService.DeserializeXMLToPostCategories(PostModel.postCategoriesSaveFolder, PostModel.postCategoriesSaveFileName);
                var postsOrdered = posts;
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        SP.ListItemCollection catPosts = DAT.DataQuery.QueryListItems(string.Format(postsSiteUrl, category.CategoryName), postsListName, getPostsCamlQueryLight);
                        if (catPosts.Count > 0)
                        {
                            foreach (SP.ListItem item in catPosts)
                            {
                                Post newPost = new Post() { Id = Convert.ToInt32(item["ID"]), PublishedDate = Convert.ToDateTime(item["PublishedDate"]), CategoryName = category.CategoryName , Title = item["Title"].ToString() };
                                posts.Add(newPost);
                            }
                        }
                    }

                    postsOrdered = (from x in posts orderby x.PublishedDate descending select x).ToList();
                }

                return postsOrdered;

            }
            catch (Exception ex)
            { 
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Post> QueryAllLatestPostsLight(string postCategoriesSaveFolder, string postsSaveFileName)
        {
            try
            {
                List<Post> posts = new List<Post>();
                //List<PostCategory> categories = QueryPostCategories();
                List<PostModel.PostCategory> categories = PostService.DeserializeXMLToPostCategories(postCategoriesSaveFolder, postCategoriesSaveFileName);
                var postsOrdered = posts;
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        SP.ListItemCollection catPosts = DAT.DataQuery.QueryListItems(string.Format(postsSiteUrl, category.CategoryName), postsListName, getPostsCamlQueryLight);
                        if (catPosts.Count > 0)
                        {
                            foreach (SP.ListItem item in catPosts)
                            {
                                Post newPost = new Post() { Id = Convert.ToInt32(item["ID"]), PublishedDate = Convert.ToDateTime(item["PublishedDate"]), CategoryName = category.CategoryName, Title = item["Title"].ToString() };
                                posts.Add(newPost);
                            }
                        }
                    }

                    postsOrdered = (from x in posts orderby x.PublishedDate descending select x).ToList();
                }

                return postsOrdered;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Post> QueryAllLatestPostsLight(int amount)
        {
            try
            {
                List<Post> posts = new List<Post>();
                List<PostCategory> categories = DeserializeXMLToPostCategories(postCategoriesSaveFolder, postCategoriesSaveFileName);
                var postsOrdered = posts;
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        SP.ListItemCollection catPosts = DAT.DataQuery.QueryListItems(string.Format(postsSiteUrl, category.CategoryName), postsListName, string.Format(getPostsCamlQueryLightLimited, amount));
                        if (catPosts.Count > 0)
                        {
                            foreach (SP.ListItem item in catPosts)
                            {
                                Post newPost = new Post() { Id = Convert.ToInt32(item["ID"]), PublishedDate = Convert.ToDateTime(item["PublishedDate"]), CategoryName = category.CategoryName , Title = item["Title"].ToString()};
                                posts.Add(newPost);
                            }
                        }
                    }

                    postsOrdered = (from x in posts orderby x.PublishedDate descending select x).ToList();
                    posts.Clear();
                    for (int i = 0; i < amount; i++)
                    {
                        posts.Add(postsOrdered[i]);
                    }
                }

                return posts;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static Post QueryPostDetail(int postId, string category)
        {
            try
            {
                Post post = new Post();
                Microsoft.SharePoint.Client.ListItemCollection coll = DAT.DataQuery.QueryListItems(string.Format(PostModel.postsSiteUrl, category), PostModel.postsListName, string.Format(PostModel.postDetailCamlQuery, postId));
                if (coll.Count > 0 && coll[0] != null)
                {
                    SP.ListItem item = coll[0];
                    post.Id = postId;
                    SP.FieldUserValue author = (SP.FieldUserValue)item["Author"];
                    post.Author = author.LookupValue.ToString();
                    post.Title = item["Title"].ToString().Replace("\\", "");
                    post.Body = HLP.TransformHtmlString(item["Body"].ToString());
                    post.PublishedDate = Convert.ToDateTime(item["PublishedDate"]);
                    post.CategoryName = category;
                }
                return post;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Post QueryPostDetail(string postTitle, string category)
        {
            try
            {
                Post post = new Post();
                Microsoft.SharePoint.Client.ListItemCollection coll = DAT.DataQuery.QueryListItems(string.Format(PostModel.postsSiteUrl, category), PostModel.postsListName, string.Format(PostModel.postDetailCamlQueryByTitle, postTitle));
                if (coll.Count > 0 && coll[0] != null)
                {
                    SP.ListItem item = coll[0];
                    post.Id = Convert.ToInt32(item["ID"]);
                    SP.FieldUserValue author = (SP.FieldUserValue)item["Author"];
                    post.Author = author.LookupValue.ToString();
                    post.Title = postTitle;
                    post.Body = HLP.TransformHtmlStringForGlobalAsax(item["Body"].ToString());
                    post.PublishedDate = Convert.ToDateTime(item["PublishedDate"]);
                    post.CategoryName = category;
                }
                return post;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int GetNextIndexOfPostList(List<Post> posts, int lastPostId)
        {
            try
            {
                var index = posts.FindIndex(a => a.Id == lastPostId);
                index++;
                return index;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return 0;
            }
        }

        public bool CreateOrUpdateCategoriesXML(string fileLocation)
        {
            try
            {
                List<PostCategory> categories = QueryPostCategories();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PostCategory>));
                    System.IO.FileStream file = System.IO.File.OpenWrite(fileLocation);
                    writer.Serialize(file, categories);
                    file.Close();
                }
                else
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PostCategory>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, categories);
                    file.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return false;
            }
        }

        public static List<PostCategory> DeserializeXMLToPostCategories(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<PostCategory> categories = new List<PostCategory>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PostCategory>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    categories = (List<PostCategory>)writer.Deserialize(file);
                    file.Close();
                }
                else
                {
                    categories = QueryPostCategories(PostModel.postsMainSiteUrl, PostModel.categoryListName, PostModel.getCategoriesCamlQuery);


                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PostCategory>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, categories);
                    file.Close();
                }

                return categories;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<PostCategory> DeserializeXMLToPostCategoriesForGlobalAsax(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<PostCategory> categories = new List<PostCategory>();
                categories = QueryPostCategories();
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<PostCategory>));

                HLP.DeleteFile(filefolder, fileName);

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, categories);
                file.Close();

                return categories;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Post> DeserializeXMLToPostsLight(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Post> postsLight = new List<Post>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Post>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    postsLight = (List<Post>)writer.Deserialize(file);
                    file.Close();
                }
                else
                {
                    postsLight = QueryAllLatestPostsLight();
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Post>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, postsLight);
                    file.Close();
                }

                return postsLight;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }


        public static List<Post> DeserializeXMLToPostsLight(string filefolder, string fileName, int amount)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Post> postsLight = new List<Post>();

                if (HLP.CheckIfFileExists(fileLocation))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Post>));
                    System.IO.FileStream file = System.IO.File.OpenRead(fileLocation);
                    postsLight = (List<Post>)writer.Deserialize(file);
                    file.Close();
                }
                else
                {
                    postsLight = QueryAllLatestPostsLight();
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Post>));
                    System.IO.FileStream file = System.IO.File.Create(fileLocation);
                    writer.Serialize(file, postsLight);
                    file.Close();
                }

                return postsLight.Take(amount).ToList();

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static List<Post> DeserializeXMLToPostsLightForGlobalAsax(string filefolder, string fileName)
        {
            try
            {
                string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + filefolder + "\\" + fileName;

                List<Post> postsLight = new List<Post>();

                postsLight = QueryAllLatestPostsLight(filefolder, fileName);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Post>));

                HLP.DeleteFile(filefolder, fileName);

                System.IO.FileStream file = System.IO.File.Create(fileLocation);
                writer.Serialize(file, postsLight);
                file.Close();

                return postsLight;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string WrapPostItem(Post post)
        {
            try
            {
                string wrappedString = "<a href=\"" + string.Format(PostModel.postDetailUrl, post.Id.ToString(), post.CategoryName.ToString()) + "\">" + post.Title + "</a>" + " | " + post.PublishedDate.ToShortDateString() + " | " + post.CategoryName + " | " + post.Author + " | " + post.Body;
                return wrappedString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string WrapPostItemNoLink(Post post)
        {
            try
            {
                string wrappedString = post.Title + " | " + post.PublishedDate.ToShortDateString() + " | " + post.CategoryName + " | " + post.Author + " | " + post.Body;
                return wrappedString;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string CreateCircleLinkDiv(List<PostModel.PostCategory> categories)
        {
            try
            {
                string redirectUrl = "../Posts.aspx?PostCat={0}";
                string itemHtmlString = "<a href=\"{0}\" class=\"btn btn-link\"><span>{1}</span></a>";
                string resultString = string.Empty;
                foreach (var item in categories)
                {
                    resultString += string.Format(itemHtmlString, string.Format(redirectUrl, item.CategoryName), item.CategoryName.ToUpperInvariant());
                }

                return resultString;
            }
            catch (System.Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static string GetCategoryTitleByCategoryName(string categoryName)
        {
            try
            {
                List<PostCategory> categories = PostService.DeserializeXMLToPostCategories(PostModel.postCategoriesSaveFolder, PostModel.postCategoriesSaveFileName);
                categories = categories.Where(x => x.CategoryName == categoryName).Take(1).ToList();
                return categories[0].CategoryTitle;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
