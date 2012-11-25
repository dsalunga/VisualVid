Imports DES

Partial Class _CMS_Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            Dim sMode As String = Request.QueryString("Mode")
            If sMode = "Forgot" Then
                MultiView1.SetActiveView(viewForgot)
            ElseIf sMode = "Activate" Then
                Dim sUsername As String = Request.QueryString("User")
                Dim sGuid As String = Request.QueryString("ConfirmCode")
                Dim User As MembershipUser = Membership.GetUser(sUsername)

                If User.ProviderUserKey.ToString() = sGuid Then
                    ' OK
                    If Not User.IsApproved Then
                        MultiView2.SetActiveView(viewSuccess)
                        User.IsApproved = True
                        Membership.UpdateUser(User)
                    Else
                        MultiView2.SetActiveView(viewNoNeed)
                    End If
                Else
                    MultiView2.SetActiveView(viewFailed)
                End If
            Else
                MultiView1.SetActiveView(viewLogin)
            End If
        End If
    End Sub

    Protected Sub Login1_LoggingIn(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.LoginCancelEventArgs) 'Handles Login1.LoggingIn
        If Not Roles.IsUserInRole(Login1.UserName, "Administrators") Then
            e.Cancel = True
        End If
    End Sub
End Class
