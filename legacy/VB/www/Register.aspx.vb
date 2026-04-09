Imports System.IO
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Globalization

Imports DES

Partial Class Register
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles Me.Load
        If Not Page.IsPostBack Then
            Dim cboMonth As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboMonth"), DropDownList)
            Dim cboDay As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboDay"), DropDownList)
            Dim cboYear As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboYear"), DropDownList)

            cboMonth.Items.Add(New ListItem("---", String.Empty))
            cboDay.Items.Add(New ListItem("---", String.Empty))
            cboYear.Items.Add(New ListItem("---", String.Empty))

            For i As Integer = 1 To 12
                cboMonth.Items.Add(New ListItem(DateTimeFormatInfo.InvariantInfo.MonthNames(i - 1), i.ToString()))
            Next

            For i As Integer = 1 To 31
                cboDay.Items.Add(i.ToString())
            Next

            For i As Integer = DateTime.Now.Year To 1930 Step -1
                cboYear.Items.Add(i.ToString())
            Next
        End If
    End Sub

    Protected Sub CreateUserWizard1_CreatedUser(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles CreateUserWizard1.CreatedUser
        Dim cboMonth As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboMonth"), DropDownList)
        Dim cboDay As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboDay"), DropDownList)
        Dim cboYear As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboYear"), DropDownList)
        Dim rblGender As RadioButtonList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("rblGender"), RadioButtonList)
        Dim cboCountry As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboCountry"), DropDownList)

        Dim sUsername As String = CreateUserWizard1.UserName
        Dim user As MembershipUser = Membership.GetUser(sUsername)

        Dim birthDate As DateTime = New DateTime(Convert.ToInt32(cboYear.SelectedValue), Convert.ToInt32(cboMonth.SelectedValue), Convert.ToInt32(cboDay.SelectedValue))

        Dim iCountryCode As Integer = Convert.ToInt32(cboCountry.SelectedValue)
        Dim isMale As Boolean = rblGender.SelectedIndex = 0
        'SqlGuid userId = SqlGuid.Parse(user.ProviderUserKey.ToString())

        Dim sUserId As String = user.ProviderUserKey.ToString()

        Roles.AddUserToRole(sUsername, "Members")

        SqlHelper.ExecuteNonQuery("UPDATE_Members", New SqlParameter("@UserId", New Guid(sUserId)), New SqlParameter("@CountryCode", iCountryCode), New SqlParameter("@Gender", isMale), New SqlParameter("@BirthDate", birthDate))

        If Not File.Exists(MapPath("/Videos/Members/" + sUserId + ".jpg")) Then
            File.Copy(MapPath("/Images/img_userpic.jpg"), MapPath("/Videos/Members/" + sUserId + ".jpg"))
        End If
    End Sub

    Protected Sub CreateUserWizard1_CreatingUser(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.LoginCancelEventArgs) ' Handles CreateUserWizard1.CreatingUser
        Dim cboMonth As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboMonth"), DropDownList)
        Dim cboDay As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboDay"), DropDownList)
        Dim cboYear As DropDownList = CType(Me.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cboYear"), DropDownList)
        Dim chkAgree As CheckBox = CType(Me.CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("chkAgree"), CheckBox)

        If Not chkAgree.Checked Then
            lblMessage.Text = "You must agree to the terms of use and privacy policy before proceeding."
            e.Cancel = True
        End If

        Try
            Dim birthDate As DateTime = New DateTime(Convert.ToInt32(cboYear.SelectedValue), Convert.ToInt32(cboMonth.SelectedValue), Convert.ToInt32(cboDay.SelectedValue))
        Catch ex As Exception
            lblMessage.Text = "Please select a valid date."
            e.Cancel = True
        End Try

        ' Check if User already exists.
        Dim sUsername As String = CreateUserWizard1.UserName
        If Roles.IsUserInRole(sUsername, "Members") Then
            lblMessage.Text = "Username already taken."
            e.Cancel = True
        End If
    End Sub

    Protected Sub CreateUserWizard1_SendingMail(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MailMessageEventArgs) ' Handles CreateUserWizard1.SendingMail
        Dim sBody As String = HttpUtility.HtmlDecode(ConfigurationManager.AppSettings("Email_Reg_Body")) ' "<span style=""font-family:Tahoma;font-size:10pt"">Username: <% Username %><br />Password: <% Password %><br /><br /><a href=""http://visualvid.s2.softrigger.com.ph/Confirm.aspx?User=<% Username %>&ConfirmCode=<% ConfirmCode %>"">Click here to activate your account</a></span>"
        Dim user As MembershipUser = Membership.GetUser(CreateUserWizard1.UserName)

        e.Cancel = True

        Dim hash As Hashtable = New Hashtable()
        hash.Add("<% Username %>", CreateUserWizard1.UserName)
        hash.Add("<% Password %>", CreateUserWizard1.Password)
        hash.Add("<% ConfirmCode %>", user.ProviderUserKey.ToString())

        Dim md As MailMessage = CreateUserWizard1.MailDefinition.CreateMailMessage(user.Email, hash, sBody, CreateUserWizard1)
        'md.CC.Add("daniel@softrigger.com");
        'md.From = new MailAddress("daniel@softrigger.com");
        md.IsBodyHtml = True
        md.Subject = "Your VisualVid Registration"

        Dim client As SmtpClient = New SmtpClient()
        client.Send(md)
    End Sub
End Class
