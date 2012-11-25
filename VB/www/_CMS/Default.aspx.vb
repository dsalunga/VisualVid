
Partial Class _CMS_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        Response.Redirect("Master.aspx", True)
    End Sub
End Class
