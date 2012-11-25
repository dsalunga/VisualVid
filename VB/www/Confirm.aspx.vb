
Partial Class Confirm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            Dim sUsername As String = Request.QueryString("User")
            Dim sGuid As String = Request.QueryString("ConfirmCode")

            If (String.IsNullOrEmpty(sUsername) Or String.IsNullOrEmpty(sGuid)) Then
                Response.Redirect(".")
            End If

            Dim user As MembershipUser = Membership.GetUser(sUsername)
            If (user.ProviderUserKey.ToString() = sGuid) Then

                ' OK
                If (Not user.IsApproved) Then

                    MultiView1.SetActiveView(viewSuccess)
                    user.IsApproved = True
                    Membership.UpdateUser(user)
                Else
                    MultiView1.SetActiveView(viewNoNeed)
                End If
            Else
                MultiView1.SetActiveView(viewFailed)
            End If
        End If
    End Sub
End Class
