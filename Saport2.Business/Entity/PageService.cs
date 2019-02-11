using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.Business.Entity
{
    public class PageService : PageModel
    {
        /*
        Reset page controls
        Set Page Title
        */
        
        public PageService(Page thisPage)
        {
            //PageModel.ThisPage = thisPage;
        }

        //public string GetPageTitle(string localPath)
        //{
        //    try
        //    {
        //        //string pageName = localPath.Replace("/", "").Replace(".aspx", "").ToUpper();
        //        //var y  = pageNamePageTitle.Where(x => pageNamePageTitle.Keys.Contains(pageName)).Select(x => x.Value).ToString();
        //        //return y;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public static string GetPageTitle(string localPath)
        {
            try
            {
                string pageName = localPath.Replace("/", "").Replace(".aspx", "").ToLower();
                var title = PageModel.pageNamePageTitle.Single(x => x.Key.ToLower() == pageName).Value.ToString();
                return title;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return null;
            }
        }

        public static bool SetPageTitle(Page thisPage, string pageTitle)
        {
            try
            {
                thisPage.Title = pageTitle;
                return true;
            }
            catch (Exception ex)
            {
                EXP.RedirectToErrorPage(ex.Message);
                return false;
            }
        }

    }
}
