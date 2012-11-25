using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DES;

public partial class Member_Profile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //DropDownList cboMonth = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboMonth");
            //DropDownList cboDay = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboDay");
            //DropDownList cboYear = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboYear");

            cboMonth.Items.Add(new ListItem("---", string.Empty));
            cboDay.Items.Add(new ListItem("---", string.Empty));
            cboYear.Items.Add(new ListItem("---", string.Empty));

            for (int i = 1; i <= 12; i++)
            {
                cboMonth.Items.Add(new ListItem(DateTimeFormatInfo.InvariantInfo.MonthNames[i - 1], i.ToString()));
            }

            for (int i = 1; i <= 31; i++)
            {
                cboDay.Items.Add(i.ToString());
            }

            for (int i = DateTime.Now.Year; i >= 1950; i--)
            {
                cboYear.Items.Add(i.ToString());
            }

            cboCountry.DataBind();

            string sUserId = Membership.GetUser().ProviderUserKey.ToString();
            imageUser.ImageUrl = "/Videos/Members/" + sUserId + ".jpg";

            using (SqlDataReader r = SqlHelper.ExecuteReader("SELECT_Members",
                new SqlParameter("@UserId", new Guid(sUserId))
            ))
            {
                if (r.Read())
                {
                    try
                    {
                        cboCountry.SelectedValue = r["CountryCode"].ToString();
                    }
                    catch { }

                    DateTime birthDate = (DateTime)r["BirthDate"];

                    cboMonth.SelectedValue = birthDate.Month.ToString();
                    cboYear.SelectedValue = birthDate.Year.ToString();
                    cboDay.SelectedValue = birthDate.Day.ToString();

                    rblGender.SelectedIndex = (bool)r["Gender"] ? 0 : 1;

                    txtUsername.Text = r["Username"].ToString();
                }
            }
        }
    }

    protected void cmdUpdate_Click(object sender, ImageClickEventArgs e)
    {
        DateTime birthDate = new DateTime(Convert.ToInt32(cboYear.SelectedValue), Convert.ToInt32(cboMonth.SelectedValue), Convert.ToInt32(cboDay.SelectedValue));
        Int32 iCountryCode = Convert.ToInt32(cboCountry.SelectedValue);
        bool isMale = (rblGender.SelectedIndex == 0);
        string sUserId = Membership.GetUser().ProviderUserKey.ToString();


        SqlHelper.ExecuteNonQuery("UPDATE_Members",
            new SqlParameter("@UserId", new Guid(sUserId)),
            new SqlParameter("@CountryCode", iCountryCode),
            new SqlParameter("@Gender", isMale),
            new SqlParameter("@BirthDate", birthDate)
        );

        // Upload Photo
        if (FileUpload1.HasFile)
        {
            string sTempDir = "/_Uploads/Temp/";

            if (!Directory.Exists(MapPath(sTempDir)))
            {
                Directory.CreateDirectory(MapPath(sTempDir));
            }

            string sUploadFile = FileUpload1.FileName; // original file
            string sDesFilename = sUserId + ".jpg"; // create a jpeg version of filename
            string sTempFilename = "TempImage" + DateTime.Now.Second + Path.GetExtension(sUploadFile); // create a temporary filename
            FileUpload1.SaveAs(MapPath(sTempDir + sTempFilename)); // upload to temp folder

            // GET SIZE RATIO
            System.Drawing.Image imageSource = ImageUtil.GetImage(MapPath(sTempDir + sTempFilename));
            int iWidth = 118;
            int iHeight = (imageSource.Height * iWidth) / imageSource.Width;

            // resize the image / create thumbnail
            ImageUtil.GenerateThumbnail(
                imageSource,
                MapPath("/Videos/Members/" + sDesFilename),
                iWidth,
                iHeight,
                System.Drawing.Imaging.ImageFormat.Jpeg
            );
            imageSource.Dispose(); // FREE UP MEMORY

            // DELETE TEMPORARY FILE
            try
            {
                File.Delete(MapPath(sTempDir + sTempFilename));
            }
            catch { }

            this.imageUser.ImageUrl = "/Videos/Members/" + sDesFilename; // show preview
            //imagePhoto.ImageUrl = imagePreview.ImageUrl;
            //hiddenImageFilename.Value = sDesFilename;
        }


        lblStatus.Text = "Update successful.";
    }

    protected void cmdCancel_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("/Member/", true);
    }
}
