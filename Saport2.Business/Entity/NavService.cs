using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using EXP = Saport2.Shared.Exceptions;

namespace Saport2.Business.Entity
{
    public class NavService : NavModel
    {
        #region About
        /* It includes methods related to SampleEntity */
        #endregion

        #region Methods
        // Methods

        public static List<NavModel> GetNavigationList()
        {
            try
            {
                List<NavModel> navList = new List<NavModel>();
                foreach (var pageModelInfo in PageModel.pageNamePageTitle)
                {
                    navList.Add(new NavModel() { NavTitle = pageModelInfo.Value, NavUrl = "/" + pageModelInfo.Key + ".aspx" });
                }

                return navList;
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
