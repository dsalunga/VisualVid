using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
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

public partial class Member_Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MultiView1.SetActiveView(viewStep1);
        }
    }

    protected void cmdFinish_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Member/", true);
    }

    protected void cmdCancel_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("/Member/", true);
    }

    protected void cmdGoToUpload_Click(object sender, ImageClickEventArgs e)
    {
        MultiView1.SetActiveView(viewStep2);
    }
    protected void cmdUpload_Click(object sender, ImageClickEventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            int iMaxVideoSize = Convert.ToInt32(ConfigurationManager.AppSettings["System.MaxVideoSize"]);
            int iFileSize = (FileUpload1.PostedFile.ContentLength / 1024) / 1024;
            if (iFileSize > iMaxVideoSize)
            {
                lblMessage.Text = string.Format("[ Sorry, the maximum video size is {0}, your file is {1} ]", iMaxVideoSize, iFileSize);
                return;
            }

            string sTitle = txtTitle.Text.Trim();
            string sDescription = txtDescription.Text.Trim();
            string sTags = txtTags.Text.Trim();
            int iCategoryID = Convert.ToInt32(rblCategories.SelectedValue);
            string sOriginalExtension = Path.GetExtension(FileUpload1.FileName);
            object userKey = Membership.GetUser().ProviderUserKey;

            //string sGuid = SqlHelper.ExecuteScalar(CommandType.Text, "SELECT NEWID()").ToString();

            object obj = SqlHelper.ExecuteScalar("UPDATE_Videos",
                new SqlParameter("@Title", sTitle),
                new SqlParameter("@Description", sDescription),
                new SqlParameter("@Tags", sTags),
                new SqlParameter("@CategoryID", iCategoryID),
                new SqlParameter("@OriginalExtension", sOriginalExtension),
                new SqlParameter("@UserId", userKey)
            );

            string sGuid = obj.ToString();
            string sNewFilename = sGuid + sOriginalExtension;

            FileUpload1.SaveAs(MapPath("/Videos/Pending/" + sNewFilename));

            string sMemberDir = MapPath("/Videos/Members/" + userKey);
            if (!Directory.Exists(sMemberDir))
            {
                Directory.CreateDirectory(sMemberDir);
            }

            Process[] ms = Process.GetProcessesByName("VisualVid");
            if (ms.Length == 0)
            {
                // foreach (Process m in ms){}
                //Thread.Sleep(30000); // Wait for 30 seconds
                //Console.WriteLine("Encoder running. Wait for 30 seconds...");

                Process p = new Process();
                p.StartInfo.FileName = MapPath("/Videos/bin/VisualVid.exe");
                p.StartInfo.WorkingDirectory = MapPath("/Videos/bin");
                p.Start();
                p.PriorityClass = ProcessPriorityClass.BelowNormal;
            }

            MultiView1.SetActiveView(viewDone);
        }
        else
        {
            lblMessage.Text = "[ Invalid upload file. ]";
        }
    }
}