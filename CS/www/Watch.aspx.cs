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
using DES.VisualVid;

public partial class Watch : System.Web.UI.Page
{
    protected string sVideo;
    protected string sDateAdded;
    protected string sUserName;
    protected string sViews;
    protected string sTags;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string sVideoID = Request.QueryString["VideoId"];

            if (!string.IsNullOrEmpty(sVideoID))
            {
                using (SqlDataReader r = SqlHelper.ExecuteReader("SELECT_Videos",
                    new SqlParameter("@VideoId", new Guid(sVideoID)),
                    new SqlParameter("@UpdateViews", true)
                ))
                {
                    if (r.Read())
                    {
                        string sUserId = r["UserId"].ToString();

                        spanTitle.InnerHtml = r["Title"].ToString();
                        spanDescription.InnerHtml = r["Description"].ToString();
                        sVideo = sUserId + "/" + sVideoID;
                        spanDateAdded.InnerHtml = Convert.ToDateTime(r["DateAdded"]).ToString("MMMM d, yyyy");
                        aUserName.InnerHtml = r["UserName"].ToString();
                        aUserName.HRef = "/Profile.aspx?UserId=" + sUserId;
                        spanViews.InnerHtml = r["Views"].ToString();
                        spanTags.InnerHtml = VideoHelper.FormatTags(r["Tags"].ToString());
                    }
                }
            }
            else
            {
                Response.Redirect(".", true);
                return;
            }

            if (!Context.User.Identity.IsAuthenticated)
            {
                aComment1.Attributes["onclick"] = "alert('Please login to post a comment'); return false;";
                aComment2.Attributes["onclick"] = "alert('Please login to post a comment'); return false;";
            }
            else
            {
                SqlHelper.ExecuteNonQuery(CommandType.Text,
                    "UPDATE Members SET Watched=Watched + 1 WHERE UserId=@UserId",
                    new SqlParameter("@UserId", Membership.GetUser().ProviderUserKey)
                );
            }
        }
    }

    protected void cmdComment_Click(object sender, EventArgs e)
    {
        Guid guidVideoId = new Guid(Request.QueryString["VideoId"]);
        object userId = Membership.GetUser().ProviderUserKey;
        string sContent = txtComment.Text.Trim();

        SqlHelper.ExecuteNonQuery("UPDATE_Comments",
            new SqlParameter("@UserId", userId),
            new SqlParameter("@VideoId", guidVideoId),
            new SqlParameter("@Content", sContent)
        );

        Response.Redirect(Request.RawUrl, true);
    }
}