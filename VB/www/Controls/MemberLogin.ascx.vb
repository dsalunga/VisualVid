
Partial Class Controls_MemberLogin
    Inherits System.Web.UI.UserControl

    Protected Sub Login1_LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Login1.LoggedIn
        If Roles.IsUserInRole(Login1.UserName, "Administrators") Then
            Response.Redirect("/_CMS/Master.aspx", True)
        Else
            Response.Redirect("/Member/", True)
        End If
    End Sub
End Class
