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

public partial class Member_Video : System.Web.UI.Page
{
    protected string sVideoId;
    protected void Page_Load(object sender, EventArgs e)
    {
        sVideoId = Request.QueryString["VideoId"];

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(sVideoId))
            {
                string sUserId = Membership.GetUser().ProviderUserKey.ToString();

                aVideo.HRef = "/Watch.aspx?VideoId=" + sVideoId;

                using (SqlDataReader r = SqlHelper.ExecuteReader("SELECT_Videos",
                    new SqlParameter("@VideoId", new Guid(sVideoId))
                ))
                {
                    if (r.Read())
                    {
                        imageVideo.Alt = r["Title"].ToString();
                        imageVideo.Src = "/Videos/Members/" + sUserId + "/" + sVideoId + ".jpg";

                        txtTitle.Text = r["Title"].ToString();
                        txtDescription.Text = r["Description"].ToString();
                        txtTags.Text = r["Tags"].ToString();

                        rblCategories.DataBind();

                        try
                        {
                            rblCategories.SelectedValue = r["CategoryID"].ToString();
                        }
                        catch { }
                    }
                }
            }
            else
            {
                Response.Redirect("/Member/", true);
            }
        }
    }

    protected void cmdCancel_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("/Member/", true);
    }

    protected void cmdUpload_Click(object sender, ImageClickEventArgs e)
    {
        string sTitle = txtTitle.Text.Trim();
        string sDescription = txtDescription.Text.Trim();
        string sTags = txtTags.Text.Trim();
        int iCategoryID = Convert.ToInt32(rblCategories.SelectedValue);

        SqlHelper.ExecuteNonQuery("UPDATE_Videos",
            new SqlParameter("@Title", sTitle),
            new SqlParameter("@Description", sDescription),
            new SqlParameter("@Tags", sTags),
            new SqlParameter("@CategoryID", iCategoryID),
            new SqlParameter("@VideoId", new Guid(sVideoId))
        );

        Response.Redirect("/Member/", true);
    }
}
