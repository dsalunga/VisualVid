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

public partial class Confirm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string sUsername = Request.QueryString["User"];
            string sGuid = Request.QueryString["ConfirmCode"];

            if (string.IsNullOrEmpty(sUsername) || string.IsNullOrEmpty(sGuid))
                Response.Redirect(".");

            MembershipUser user = Membership.GetUser(sUsername);
            if (user.ProviderUserKey.ToString() == sGuid)
            {
                // OK
                if (!user.IsApproved)
                {
                    MultiView1.SetActiveView(viewSuccess);
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                }
                else
                {
                    MultiView1.SetActiveView(viewNoNeed);
                }
            }
            else
            {
                MultiView1.SetActiveView(viewFailed);
            }
        }
    }
}
