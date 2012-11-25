using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DES;

public partial class Member_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string sUserId = Membership.GetUser().ProviderUserKey.ToString();

            hiddenUserId.Value = sUserId;
            imageUser.Src = "/Videos/Members/" + sUserId + ".jpg";

            if (!string.IsNullOrEmpty(sUserId))
            {
                using (SqlDataReader r = SqlHelper.ExecuteReader("SELECT_Members",
                    new SqlParameter("@UserId", new Guid(sUserId))
                ))
                {
                    if (r.Read())
                    {
                        DateTime birthDate = (DateTime)r["BirthDate"];
                        spanAge.InnerHtml = ((int)(DateTime.Now.Subtract(birthDate).Ticks / (TimeSpan.TicksPerDay * 365))).ToString();

                        spanGender.InnerHtml = (bool)r["Gender"] ? "Male" : "Female";
                        spanUsername.InnerHtml = r["Username"].ToString();
                        spanUsername2.InnerHtml = spanUsername.InnerHtml;
                        spanCountry.InnerHtml = r["CountryName"].ToString();
                        spanMemberSince.InnerHtml = ((DateTime)r["CreateDate"]).ToString("MMMM d, yyyy");
                        //spanLastLogin.InnerHtml = r["LastLoginDate"].ToString();
                        spanWatched.InnerHtml = r["Watched"].ToString();
                        spanEmail.InnerHtml = r["Email"].ToString();
                    }
                }
            }


        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sID = e.CommandArgument.ToString();
        switch (e.CommandName)
        {
            case "Custom_Delete":
                SqlHelper.ExecuteNonQuery(CommandType.Text,
                    "DELETE FROM Videos WHERE VideoId=@VideoId",
                    new SqlParameter("@VideoId", new Guid(sID))
                );
                GridView1.DataBind();

                string sFile = MapPath("/Videos/Members/" + Membership.GetUser().ProviderUserKey + "/" + sID);
                try
                {
                    File.Delete(sFile + ".flv");
                    File.Delete(sFile + ".jpg");
                }
                catch { }
                break;

            case "Custom_Edit":
                Response.Redirect("/Member/Video.aspx?VideoId=" + sID, true);
                break;
        }
    }
}
