Imports System.Data
Imports System.Data.SqlClient

Imports DES
Imports DES.VisualVid

Partial Class Watch
    Inherits System.Web.UI.Page

    Protected sVideo As String
    Protected sDateAdded As String
    Protected sUserName As String
    Protected sViews As String
    Protected sTags As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            Dim sVideoID As String = Request.QueryString("VideoId")

            If Not String.IsNullOrEmpty(sVideoID) Then
                Using r As SqlDataReader = SqlHelper.ExecuteReader("SELECT_Videos", New SqlParameter("@VideoId", New Guid(sVideoID)), New SqlParameter("@UpdateViews", True))
                    If r.Read() Then
                        Dim sUserId As String = r("UserId").ToString()

                        spanTitle.InnerHtml = r("Title").ToString()
                        spanDescription.InnerHtml = r("Description").ToString()
                        sVideo = sUserId + "/" + sVideoID
                        spanDateAdded.InnerHtml = Convert.ToDateTime(r("DateAdded")).ToString("MMMM d, yyyy")
                        aUserName.InnerHtml = r("UserName").ToString()
                        aUserName.HRef = "/Profile.aspx?UserId=" + sUserId
                        spanViews.InnerHtml = r("Views").ToString()
                        spanTags.InnerHtml = VideoHelper.FormatTags(r("Tags").ToString())
                    End If
                End Using
            Else
                Response.Redirect(".", True)
                Return
            End If

            If Not Context.User.Identity.IsAuthenticated Then
                aComment1.Attributes("onclick") = "alert('Please login to post a comment'); return false;"
                aComment2.Attributes("onclick") = "alert('Please login to post a comment'); return false;"
            Else
                SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE Members SET Watched=Watched + 1 WHERE UserId=@UserId", New SqlParameter("@UserId", Membership.GetUser().ProviderUserKey))
            End If
        End If
    End Sub

    Protected Sub cmdComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles cmdComment.Click
        Dim guidVideoId As Guid = New Guid(Request.QueryString("VideoId"))
        Dim userId As Object = Membership.GetUser().ProviderUserKey
        Dim sContent As String = txtComment.Text.Trim()

        SqlHelper.ExecuteNonQuery("UPDATE_Comments", New SqlParameter("@UserId", userId), New SqlParameter("@VideoId", guidVideoId), New SqlParameter("@Content", sContent))

        Response.Redirect(Request.RawUrl, True)
    End Sub
End Class
