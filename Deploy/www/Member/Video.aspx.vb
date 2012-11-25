Imports System.Data
Imports System.Data.SqlClient

Imports DES

Partial Class Member_Video
    Inherits System.Web.UI.Page

    Protected sVideoId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        sVideoId = Request.QueryString("VideoId")

        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(sVideoId) Then
                Dim sUserId As String = Membership.GetUser().ProviderUserKey.ToString()

                aVideo.HRef = "/Watch.aspx?VideoId=" + sVideoId

                Using r As SqlDataReader = SqlHelper.ExecuteReader("SELECT_Videos", New SqlParameter("@VideoId", New Guid(sVideoId)))
                    If r.Read() Then
                        imageVideo.Alt = r("Title").ToString()
                        imageVideo.Src = "/Videos/Members/" + sUserId + "/" + sVideoId + ".jpg"

                        txtTitle.Text = r("Title").ToString()
                        txtDescription.Text = r("Description").ToString()
                        txtTags.Text = r("Tags").ToString()

                        rblCategories.DataBind()

                        Try
                            rblCategories.SelectedValue = r("CategoryID").ToString()
                        Catch ex As Exception

                        End Try
                    End If
                End Using
            Else
                Response.Redirect("/Member/", True)
            End If
        End If
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdCancel.Click
        Response.Redirect("/Member/", True)
    End Sub

    Protected Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdUpload.Click
        Dim sTitle As String = txtTitle.Text.Trim()
        Dim sDescription As String = txtDescription.Text.Trim()
        Dim sTags As String = txtTags.Text.Trim()
        Dim iCategoryID As Integer = Convert.ToInt32(rblCategories.SelectedValue)

        SqlHelper.ExecuteNonQuery("UPDATE_Videos", New SqlParameter("@Title", sTitle), New SqlParameter("@Description", sDescription), New SqlParameter("@Tags", sTags), New SqlParameter("@CategoryID", iCategoryID), New SqlParameter("@VideoId", New Guid(sVideoId)))

        Response.Redirect("/Member/", True)
    End Sub
End Class
