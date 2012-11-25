using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Security;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Controls_MemberLogin : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LoginButton_Click(object sender, ImageClickEventArgs e)
    {
        
    }

    protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        /*
        if (Roles.IsUserInRole(Login1.UserName, "Administrators"))
        {
            e.Cancel = true;
        }
        */
    }
    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        if (Roles.IsUserInRole(Login1.UserName, "Administrators"))
        {
            Response.Redirect("/_CMS/Master.aspx", true);
        }
        else
        {
            Response.Redirect("/Member/", true);
        }
    }
}
