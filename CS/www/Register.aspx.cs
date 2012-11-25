using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DES;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DropDownList cboMonth = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboMonth");
            DropDownList cboDay = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboDay");
            DropDownList cboYear = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboYear");

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
        }
    }

    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        DropDownList cboMonth = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboMonth");
        DropDownList cboDay = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboDay");
        DropDownList cboYear = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboYear");
        RadioButtonList rblGender = (RadioButtonList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("rblGender");
        DropDownList cboCountry = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboCountry");

        string sUsername = CreateUserWizard1.UserName;
        MembershipUser user = Membership.GetUser(sUsername);

        DateTime birthDate = new DateTime(Convert.ToInt32(cboYear.SelectedValue), Convert.ToInt32(cboMonth.SelectedValue), Convert.ToInt32(cboDay.SelectedValue));

        Int32 iCountryCode = Convert.ToInt32(cboCountry.SelectedValue);
        bool isMale = (rblGender.SelectedIndex == 0);
        //SqlGuid userId = SqlGuid.Parse(user.ProviderUserKey.ToString());

        string sUserId = user.ProviderUserKey.ToString();

        Roles.AddUserToRole(sUsername, "Members");

        SqlHelper.ExecuteNonQuery("UPDATE_Members",
            new SqlParameter("@UserId", new Guid(sUserId)),
            new SqlParameter("@CountryCode", iCountryCode),
            new SqlParameter("@Gender", isMale),
            new SqlParameter("@BirthDate", birthDate)
        );

        File.Copy(MapPath("/Images/img_userpic.jpg"), MapPath("/Videos/Members/" + sUserId + ".jpg"));

        // Create Folder
        //string sPath = MapPath("/Videos/Members/" + user.ProviderUserKey);
        //if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
    }

    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        DropDownList cboMonth = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboMonth");
        DropDownList cboDay = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboDay");
        DropDownList cboYear = (DropDownList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboYear");

        CheckBox chkAgree = (CheckBox)this.CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("chkAgree");

        if (!chkAgree.Checked)
        {
            lblMessage.Text = "You must agree to the terms of use and privacy policy before proceeding.";
            e.Cancel = true;
        }

        try
        {
            DateTime birthDate = new DateTime(Convert.ToInt32(cboYear.SelectedValue), Convert.ToInt32(cboMonth.SelectedValue), Convert.ToInt32(cboDay.SelectedValue));
        }
        catch
        {
            lblMessage.Text = "Please select a valid date.";
            e.Cancel = true;
        }
    }

    protected void CreateUserWizard1_SendingMail(object sender, MailMessageEventArgs e)
    {
        string sBody = ConfigurationManager.AppSettings["Email_Reg_Body"]; //"<span style=\"font-family:Tahoma;font-size:10pt\">Username: <% Username %><br />Password: <% Password %><br /><br /><a href=\"http://visualvid.s2.softrigger.com.ph/Confirm.aspx?User=<% Username %>&ConfirmCode=<% ConfirmCode %>\">Click here to activate your account</a></span>";
        MembershipUser user = Membership.GetUser(CreateUserWizard1.UserName);

        e.Cancel = true;

        Hashtable hash = new Hashtable();
        hash.Add("<% Username %>", CreateUserWizard1.UserName);
        hash.Add("<% Password %>", CreateUserWizard1.Password);
        hash.Add("<% ConfirmCode %>", user.ProviderUserKey.ToString());

        MailMessage md = CreateUserWizard1.MailDefinition.CreateMailMessage(user.Email, hash, sBody, CreateUserWizard1);
        //md.CC.Add("daniel@softrigger.com");
        //md.From = new MailAddress("daniel@softrigger.com");
        md.IsBodyHtml = true;
        md.Subject = "Your VisualVid Registration";

        try
        {
            SmtpClient client = new SmtpClient();
            client.Send(md);
        }
        catch { }
    }
}
