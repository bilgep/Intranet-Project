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
    public partial class PostDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int postId = Request.QueryString["PostId"] != null ? Convert.ToInt32(Request.QueryString["PostId"].ToString()) : 0;
                string postCat = Request.QueryString["PostCat"] != null ? Request.QueryString["PostCat"].ToString() : "";
                if (postId > 0 && !string.IsNullOrEmpty(postCat))
                {
                    PostModel.Post addPost = PostService.QueryPostDetail(postId, postCat);
                    ltrMain.Text = string.Format(PostModel.htmlForPostDetailPage, addPost.Title, addPost.Author, Shared.Helpers.GetDateTurkishCulture(addPost.PublishedDate), addPost.Body);
                }

            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

        }
    }
}