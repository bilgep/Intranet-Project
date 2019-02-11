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
    public partial class BillBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    foreach (var cat in BillBoardModel.Advert.adCategories)
                    {
                        ltrAdCategoryLinks.Text += string.Format(BillBoardModel.catAdvertItemHtml,  string.Format(BillBoardModel.Advert.CategoryRedirectPage, cat), cat);
                    }
                }
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
            }

        }
    }
}