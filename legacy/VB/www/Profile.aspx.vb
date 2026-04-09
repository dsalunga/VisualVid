Imports DES
Imports System.Data.SqlClient


Partial Class Profile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            Dim sUserId As String = Request.QueryString("UserId")

            imageUser.Src = "/Videos/Members/" + sUserId + ".jpg"

            If Not String.IsNullOrEmpty(sUserId) Then
                hiddenUserId.Value = sUserId

                Using r As SqlDataReader = SqlHelper.ExecuteReader("SELECT_Members", New SqlParameter("@UserId", New Guid(sUserId)))
                    If r.Read() Then
                        Dim birthDate As DateTime = CType(r("BirthDate"), DateTime)
                        spanAge.InnerHtml = CType(DateTime.Now.Subtract(birthDate).Ticks / (TimeSpan.TicksPerDay * 365), Integer)
                        spanGender.InnerHtml = IIf(CType(r("Gender"), Boolean), "Male", "Female")
                        spanEmail.InnerHtml = r("Email")
                        spanUsername.InnerHtml = r("Username")
                        spanCountry.InnerHtml = r("CountryName")
                        spanMemberSince.InnerHtml = CType(r("CreateDate"), DateTime).ToString("MMMM d, yyyy")
                        spanLastLogin.InnerHtml = r("LastLoginDate")
                        spanWatched.InnerHtml = r("Watched")
                    Else
                        Response.Redirect(".", True)
                    End If
                End Using
            Else
                Response.Redirect("/", True)
            End If
        End If
    End Sub
End Class