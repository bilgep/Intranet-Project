using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using EXP = Saport2.Shared.Exceptions;
using HLP = Saport2.Shared.Helpers;

namespace Saport2.UI
{
    public partial class BillBoardCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!Page.IsPostBack)
                {
                    string category = Request.QueryString["Cat"] != null ? Request.QueryString["Cat"].ToString() : string.Empty;

                    if (category != string.Empty)
                    {
                        ltrAdCat.Text = Request.QueryString["Cat"].ToString();

                        List<BillBoardModel.Advert> adverts = BillBoardService.DeserializeXMLToAdvertsLight(BillBoardModel.advertssSaveFolder, BillBoardService.GetSaveFileNameForGlobalAsax(category), 30, category);

                        foreach (var item in adverts)
                        {


                            ltrMain.Text += string.Format(BillBoardModel.advertListItemHTMLString, item.DetailPageUrl, item.DefaultImage, item.DetailPageUrl, item.Title);
                        }
 
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