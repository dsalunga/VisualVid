Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Imports DES

Partial Class _CMS_Members
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) ' Handles GridView1.RowCommand
        Dim user As MembershipUser
        Dim sUserId As String = e.CommandArgument.ToString()

        Select Case e.CommandName
            Case "View_Videos"
                Response.Redirect("Videos.aspx?UserId=" & sUserId, True)
            Case "Custom_Delete"
                ' Delete all videos first
                Using r As SqlDataReader = SqlHelper.ExecuteReader("SELECT_Videos", New SqlParameter("@UserId", New Guid(sUserId)))
                    While r.Read()

                        Dim sVideoId As String = r("VideoId").ToString()
                        SqlHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM Videos WHERE VideoId=@VideoId", New SqlParameter("@VideoId", New Guid(sVideoId)))
                        GridView1.DataBind()

                        Dim sFile As String = MapPath("/Videos/Members/" & sUserId & "/" + sVideoId)
                        Try
                            File.Delete(sFile + ".flv")
                            File.Delete(sFile + ".jpg")
                        Catch ex As Exception

                        End Try

                        Try
                            File.Delete(MapPath("/Videos/Members/" & sUserId & ".jpg"))
                        Catch ex As Exception

                        End Try

                    End While
                End Using

                Dim sMemberVideos As String = MapPath("/Videos/Members/" & sUserId)
                Try
                    Directory.Delete(sMemberVideos)
                Catch ex As Exception

                End Try

                user = Membership.GetUser(New Guid(sUserId))
                Membership.DeleteUser(user.UserName)
                GridView1.DataBind()

            Case "Toggle_Locked"
                user = Membership.GetUser(New Guid(sUserId))
                user.UnlockUser()
                Membership.UpdateUser(user)
                GridView1.DataBind()

            Case "Toggle_Approved"
                user = Membership.GetUser(New Guid(sUserId))
                user.IsApproved = Not user.IsApproved
                Membership.UpdateUser(user)
                GridView1.DataBind()
        End Select
    End Sub
End Class
