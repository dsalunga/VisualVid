
Partial Class Controls_HeaderSearch
    Inherits System.Web.UI.UserControl

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdSearch.Click
        Response.Redirect("/Results.aspx?Search=" + HttpUtility.UrlEncode(txtSearch.Text.Trim()), True)
    End Sub
End Class
