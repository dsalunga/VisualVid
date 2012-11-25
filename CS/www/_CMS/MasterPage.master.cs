using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CMS
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (!Page.IsPostBack)
            {
                if (!Roles.IsUserInRole("Administrators") && !Roles.IsUserInRole("Site Owners") &&
                    !Roles.IsUserInRole("Content Owners"))
                {
                    LoginStatusDerived lsd = new LoginStatusDerived();
                    lsd.LogOutCurrentUser();
                }
            }
            */
        }
    }
}
