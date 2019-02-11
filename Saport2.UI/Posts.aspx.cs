using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using Saport2.Business;
using HLP = Saport2.Shared.Helpers;
using EXP = Saport2.Shared.Exceptions;
using System.Web.UI.HtmlControls;

namespace Saport2.UI
{
    public partial class Posts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string literalString = "";

                #region (!Page.IsPostBack)
                if (!Page.IsPostBack)
                {
                    #region Load Category Links
                    List<PostModel.PostCategory> allCats = PostService.DeserializeXMLToPostCategories(PostModel.postCategoriesSaveFolder, PostModel.postCategoriesSaveFileName);
                    foreach (var cat in allCats)
                    {
                        ltrBlogCategories.Text += string.Format(PostModel.htmlForPostCategories, string.Format(PostModel.postCategoryDetailUrl, cat.CategoryName),cat.CategoryTitle);
                    }
                    #endregion


                    List<PostModel.Post> allPostsLight = PostService.DeserializeXMLToPostsLight(PostModel.postsLightSaveFolder, PostModel.postsLightFileName);

                    PostModel.Post post = new PostModel.Post();

                    if (Request.QueryString["PostCat"] == null) // If there is no Category selection
                    {
                        #region First 3 Post of All Categories
                        int i = 0;

                        #region Modify Box Title
                        boxTitle.InnerHtml = "Köşe Yazıları";
                        #endregion

                        foreach (var item in allPostsLight)
                        {
                            post = PostService.QueryPostDetail(item.Id, item.CategoryName);

                            string redirectUrlCategory = string.Format(PostModel.postCategoryDetailUrl, post.CategoryName);
                            string redirectUrlCampItem = string.Format(PostModel.postDetailUrl, item.Id, post.CategoryName);
                            ltrMain.Text += string.Format(PostModel.htmlForPostsPage, redirectUrlCampItem, post.Title, post.Author, redirectUrlCategory , post.CategoryTitle, HLP.GetDateTurkishCulture(post.PublishedDate), post.Body, redirectUrlCampItem);

                            i++;
                            if (i == 3)
                            {
                                ViewState.Add("LastPostId", post.Id.ToString());
                                ViewState.Add("LastPostCat", post.CategoryName.ToString());
                                break;
                            }
                        }
                        #endregion
                    }
                    else // If there's a Category selected
                    {

                        #region First 3 Post of Specific Category
                        int i = 0;
                        string catName = Request.QueryString["PostCat"].ToString();

                        #region Modify Box Title
                        boxTitle.InnerHtml = "Köşe Yazıları - " + PostService.GetCategoryTitleByCategoryName(catName);
                        #endregion

                        var categoriedPosts = allPostsLight.Where(x => x.CategoryName == catName).Take(3).ToList();
                        foreach (var item in categoriedPosts)
                        {
                            post = PostService.QueryPostDetail(item.Id, item.CategoryName);

                            ltrMain.Text += string.Format(PostModel.htmlForCategoryPostsPage, string.Format (PostModel.postDetailUrl, post.Id,post.CategoryName), post.Title, post.Author, post.PublishedDate.ToShortDateString(), post.Body);
                            i++;
                            if (i == 3)
                            {
                                ViewState.Add("LastPostId", post.Id.ToString());
                                break;
                            }
                        }
                        #endregion
                    }

                }

                #endregion

                ltrMain.Text += literalString;

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                #region Load Mode Button'u click olursa
                int lastPostId = Convert.ToInt32(ViewState["LastPostId"]);
                string lastPostCat = ViewState["LastPostCat"] != null ? ViewState["LastPostCat"].ToString() : (Request.QueryString["PostCat"] != null ? Request.QueryString["PostCat"] : "");
                List<PostModel.Post> sessionPosts = PostService.DeserializeXMLToPostsLight(PostModel.postsLightSaveFolder, PostModel.postsLightFileName);


                if (sessionPosts != null)
                {
                    #region Disable LoadMore Button if no more posts exist
                    PostModel.Post last = null;

                    if (Request.QueryString["PostCat"] != null)
                    {
                        last = sessionPosts.Where(x => x.CategoryName == Request.QueryString["PostCat"].ToString()).Last();
                    }

                    if ((Request.QueryString["PostCat"] == null && sessionPosts[sessionPosts.Count - 1].Id == lastPostId) || (Request.QueryString["PostCat"] != null && last.Id == lastPostId))
                    {
                        Button1.Visible = false;
                        #endregion
                    }
                    else
                    {
                        var lastPost = sessionPosts.Where(x => x.Id == lastPostId && x.CategoryName == lastPostCat).ToList();

                        int index = sessionPosts.IndexOf((PostModel.Post)lastPost[0]);
                        index++;
                        if (Request.QueryString["PostCat"] == null)
                        {
                            #region Load One More Post From Any Category

                            PostModel.Post addPost = PostService.QueryPostDetail(sessionPosts[index].Id, sessionPosts[index].CategoryName);

                            string redirectUrlCategory = string.Format(PostModel.postCategoryDetailUrl, addPost.CategoryName);
                            string redirectUrlCampItem = string.Format(PostModel.postDetailUrl, addPost.Id, addPost.CategoryName);
                            ltrMain.Text += string.Format(PostModel.htmlForPostsPage, redirectUrlCampItem, addPost.Title, addPost.Author, redirectUrlCategory, addPost.CategoryTitle, HLP.GetDateTurkishCulture(addPost.PublishedDate), addPost.Body, redirectUrlCampItem);

                            ViewState.Add("LastPostId", addPost.Id);
                            ViewState.Add("LastPostCat", addPost.CategoryName);
                            #endregion
                        }
                        else
                        {
                            #region Load One More Post From Specific Category
                            string postCat = Request.QueryString["PostCat"].ToString();
                            while (sessionPosts[index].CategoryName != postCat)
                            {
                                index++;
                                if (index == sessionPosts.Count) { index = 0; break; }
                            }
                            if (index > 0)
                            {
                                PostModel.Post addPost = PostService.QueryPostDetail(sessionPosts[index].Id, sessionPosts[index].CategoryName);

                                ltrMain.Text += string.Format(PostModel.htmlForCategoryPostsPage, string.Format(PostModel.postDetailUrl, addPost.Id, addPost.CategoryName), addPost.Title, addPost.Author, HLP.GetDateTurkishCulture(addPost.PublishedDate), addPost.Body);

                                ViewState.Add("LastPostId", addPost.Id);
                            }

                            #endregion

                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

        }
    }
}