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

public partial class _CMS_Members : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        MembershipUser user;
        string sUserId = e.CommandArgument.ToString();

        switch (e.CommandName)
        {
            case "View_Videos":
                Response.Redirect("Videos.aspx?UserId=" + sUserId, true);
                break;
            case "Custom_Delete":
                // Delete all videos first
                using (SqlDataReader r = SqlHelper.ExecuteReader("SELECT_Videos",
                    new SqlParameter("@UserId", new Guid(sUserId))
                ))
                {
                    while (r.Read())
                    {
                        string sVideoId = r["VideoId"].ToString();
                        SqlHelper.ExecuteNonQuery(CommandType.Text,
                            "DELETE FROM Videos WHERE VideoId=@VideoId",
                            new SqlParameter("@VideoId", new Guid(sVideoId))
                        );
                        GridView1.DataBind();

                        string sFile = MapPath("/Videos/Members/" + sUserId + "/" + sVideoId);
                        try
                        {
                            File.Delete(sFile + ".flv");
                            File.Delete(sFile + ".jpg");
                        }
                        catch { }

                        try
                        {
                            File.Delete(MapPath("/Videos/Members/" + sUserId + ".jpg"));
                        }
                        catch { }
                    }
                }

                string sMemberVideos = MapPath("/Videos/Members/" + sUserId);
                try
                {
                    Directory.Delete(sMemberVideos);
                }
                catch { }

                user = Membership.GetUser(new Guid(sUserId));
                Membership.DeleteUser(user.UserName);
                GridView1.DataBind();
                break;
            case "Toggle_Locked":
                user = Membership.GetUser(new Guid(sUserId));
                user.UnlockUser();
                Membership.UpdateUser(user);
                GridView1.DataBind();
                break;
            case "Toggle_Approved":
                user = Membership.GetUser(new Guid(sUserId));
                user.IsApproved = !user.IsApproved;
                Membership.UpdateUser(user);
                GridView1.DataBind();
                break;
        }
    }
}
