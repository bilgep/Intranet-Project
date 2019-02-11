using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Saport2.Shared
{
    public class Exceptions
    {
        /* It stores exception types, Exception methods (redirection, showing exception messages etc.)*/
        public static string errorPageName = "~/Error.aspx";
        public string ExceptionUIMessage { get; set; } 
        public string ExceptionLogMessage { get; set; }

        public static void RedirectToErrorPage(string exceptionMessage)
        {
            try
            {
                HttpContext.Current.Session.Add("LastException", exceptionMessage);
                HttpContext.Current.Response.Redirect(errorPageName, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (System.Exception)
            {
                HttpContext.Current.Response.Redirect(errorPageName, false);
            }
        }
        
    }
}
