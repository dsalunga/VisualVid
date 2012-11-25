using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using DES;

namespace DES.Web.CMS
{
    /// <summary>
    /// Summary description for _default.
    /// </summary>
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here

            if (!Page.IsPostBack)
            {
                string sMode = Request.QueryString["Mode"];
                if (sMode == "Forgot")
                {
                    MultiView1.SetActiveView(viewForgot);
                }
                else if (sMode == "Activate")
                {
                    string sUsername = Request.QueryString["User"];
                    string sGuid = Request.QueryString["ConfirmCode"];

                    MembershipUser user = Membership.GetUser(sUsername);
                    if (user.ProviderUserKey.ToString() == sGuid)
                    {
                        // OK
                        if (!user.IsApproved)
                        {
                            MultiView2.SetActiveView(viewSuccess);
                            user.IsApproved = true;
                            Membership.UpdateUser(user);
                        }
                        else
                        {
                            MultiView2.SetActiveView(viewNoNeed);
                        }
                    }
                    else
                    {
                        MultiView2.SetActiveView(viewFailed);
                    }
                }
                else
                {
                    MultiView1.SetActiveView(viewLogin);
                }
            }
        }

        protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            if (!Roles.IsUserInRole(Login1.UserName, "Administrators"))
            {
                e.Cancel = true;
            }
        }
}
}
