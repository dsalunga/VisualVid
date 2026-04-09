namespace DES.Web._CMS
{
	using System;
	//using System.Configuration;
	using System.Data;
    using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
    using System.Web.Security;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

    using DES;

	/// <summary>
	///		Summary description for LoginInfo.
	/// </summary>
	public partial class LoginInfo : System.Web.UI.UserControl
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			if(!Page.IsPostBack)
			{
                MembershipUser user = Membership.GetUser();
                lUsername.Text = user.UserName;
                lLastLogin.Text = user.LastLoginDate.ToString();

                /*
                using (SqlDataReader r = SqlHelper.ExecuteReader("CMS.SELECT_Users",
                    new SqlParameter("@UserId", user.ProviderUserKey)
                ))
                {
                    if (r.Read())
                    {
                        lName.Text = string.Format("{0} {1} {2}", r["FirstName"], r["MiddleName"], r["LastName"]);
                    }
                }
                */
			}
		}
	}
}
