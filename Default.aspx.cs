using System;
using System.Web;
using System.Web.UI;

namespace Webwakeup
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["client"] = Request.clientHostAddress;
            txtAlias.Text = Request.clientHostAddress;
        }
    }
}
