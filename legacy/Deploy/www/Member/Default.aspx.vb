Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports DES

Partial Class Member_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            Dim sUserId As String = Membership.GetUser().ProviderUserKey.ToString()

            hiddenUserId.Value = sUserId
            imageUser.Src = "/Videos/Members/" + sUserId + ".jpg"

            If Not String.IsNullOrEmpty(sUserId) Then
                Using r As SqlDataReader = SqlHelper.ExecuteReader("SELECT_Members", New SqlParameter("@UserId", New Guid(sUserId)))
                    If r.Read() Then
                        Dim birthDate As DateTime = CType(r("BirthDate"), DateTime)

                        spanAge.InnerHtml = CType(DateTime.Now.Subtract(birthDate).Ticks / (TimeSpan.TicksPerDay * 365), Integer)
                        spanGender.InnerHtml = IIf(CType(r("Gender"), Boolean), "Male", "Female")
                        spanUsername.InnerHtml = r("Username")
                        spanUsername2.InnerHtml = spanUsername.InnerHtml
                        spanCountry.InnerHtml = r("CountryName")
                        spanMemberSince.InnerHtml = CType(r("CreateDate"), DateTime).ToString("MMMM d, yyyy")
                        'spanLastLogin.InnerHtml = r("LastLoginDate")
                        spanWatched.InnerHtml = r("Watched")
                        spanEmail.InnerHtml = r("Email")
                    End If
                End Using
            End If
        End If
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) 'Handles GridView1.RowCommand
        Dim sID As String = e.CommandArgument
        Select Case e.CommandName

            Case "Custom_Delete"
                SqlHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM Videos WHERE VideoId=@VideoId", New SqlParameter("@VideoId", New Guid(sID)))

                GridView1.DataBind()

                Dim sFile As String = MapPath("/Videos/Members/" + Membership.GetUser().ProviderUserKey.ToString() + "/" + sID)
                Try
                    File.Delete(sFile + ".flv")
                    File.Delete(sFile + ".jpg")
                Catch ex As Exception

                End Try

            Case "Custom_Edit"
                Response.Redirect("/Member/Video.aspx?VideoId=" + sID, True)
        End Select
    End Sub
End Class
