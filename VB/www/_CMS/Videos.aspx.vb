Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports DES

Partial Class _CMS_Videos
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles Me.Load
        If Not Page.IsPostBack Then
            Dim sUserId As String = Request.QueryString("UserId")
            If Not String.IsNullOrEmpty(sUserId) Then

                DropDownList1.DataBind()
                Try
                    DropDownList1.SelectedValue = sUserId
                Catch ex As Exception

                End Try

                DataList1.DataBind()
            End If
        End If
    End Sub

    Protected Sub DataList1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) ' Handles DataList1.ItemCommand
        Dim sID As String = e.CommandArgument.ToString()

        Select Case e.CommandName

            Case "Custom_Delete"
                Dim obj As Object = SqlHelper.ExecuteScalar(CommandType.Text, "SELECT UserId FROM Videos WHERE VideoId=@VideoId", New SqlParameter("@VideoId", New Guid(sID)))

                SqlHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM Videos WHERE VideoId=@VideoId", New SqlParameter("@VideoId", New Guid(sID)))
                DataList1.DataBind()

                Dim sFile As String = MapPath("/Videos/Members/" + obj.ToString() + "/" + sID)
                Try
                    File.Delete(sFile + ".flv")
                    File.Delete(sFile + ".jpg")
                Catch ex As Exception

                End Try
        End Select

    End Sub
End Class
