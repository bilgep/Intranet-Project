using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saport2.Business.Entity;
using System.Threading.Tasks;
using System.Threading;

namespace Saport2.UI
{
    public partial class Error : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            errorMessage.Value = Session["LastException"] != null ? Session["LastException"].ToString() : "Bir problem oluştu.";

        }
    }
}