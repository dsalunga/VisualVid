using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DES;

namespace Gela.VisualVid
{
    public partial class MemberProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string sUserId = Request.QueryString["UserId"];

                imageUser.Src = "/Videos/Members/" + sUserId + ".jpg";

                if (!string.IsNullOrEmpty(sUserId))
                {
                    hiddenUserId.Value = sUserId;

                    using (SqlDataReader r = SqlHelper.ExecuteReader("SELECT_Members",
                        new SqlParameter("@UserId", new Guid(sUserId))
                    ))
                    {
                        if (r.Read())
                        {
                            DateTime birthDate = (DateTime)r["BirthDate"];
                            spanAge.InnerHtml = ((int)(DateTime.Now.Subtract(birthDate).Ticks / (TimeSpan.TicksPerDay * 365))).ToString();
                            spanGender.InnerHtml = (bool)r["Gender"] ? "Male" : "Female";
                            spanEmail.InnerHtml = r["Email"].ToString();

                            spanUsername.InnerHtml = r["Username"].ToString();
                            spanCountry.InnerHtml = r["CountryName"].ToString();
                            spanMemberSince.InnerHtml = ((DateTime)r["CreateDate"]).ToString("MMMM d, yyyy");
                            spanLastLogin.InnerHtml = r["LastLoginDate"].ToString();
                            spanWatched.InnerHtml = r["Watched"].ToString();
                        }
                        else
                        {
                            Response.Redirect(".", true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("/", true);
                }
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
}
}