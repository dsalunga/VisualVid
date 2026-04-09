Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Imports DES

Partial Class Member_Profile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            cboMonth.Items.Add(New ListItem("---", String.Empty))
            cboDay.Items.Add(New ListItem("---", String.Empty))
            cboYear.Items.Add(New ListItem("---", String.Empty))


            For i As Integer = 1 To 12
                cboMonth.Items.Add(New ListItem(DateTimeFormatInfo.InvariantInfo.MonthNames(i - 1), i.ToString()))
            Next

            For i As Integer = 1 To 31
                cboDay.Items.Add(i.ToString())
            Next

            For i As Integer = DateTime.Now.Year To 1950 Step -1
                cboYear.Items.Add(i.ToString())
            Next

            cboCountry.DataBind()

            Dim sUserId As String = Membership.GetUser().ProviderUserKey.ToString()
            imageUser.ImageUrl = "/Videos/Members/" + sUserId + ".jpg"

            Using r As SqlDataReader = SqlHelper.ExecuteReader("SELECT_Members", New SqlParameter("@UserId", New Guid(sUserId)))

                If r.Read() Then

                    Try
                        cboCountry.SelectedValue = r("CountryCode")
                    Catch ex As Exception

                    End Try

                    Dim birthDate As DateTime = CType(r("BirthDate"), DateTime)

                    cboMonth.SelectedValue = birthDate.Month.ToString()
                    cboYear.SelectedValue = birthDate.Year.ToString()
                    cboDay.SelectedValue = birthDate.Day.ToString()

                    rblGender.SelectedIndex = IIf(CType(r("Gender"), Boolean), 0, 1)

                    txtUsername.Text = r("Username")
                End If
            End Using
        End If
    End Sub

    Protected Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdUpdate.Click
        Dim birthDate As DateTime = New DateTime(Convert.ToInt32(cboYear.SelectedValue), Convert.ToInt32(cboMonth.SelectedValue), Convert.ToInt32(cboDay.SelectedValue))
        Dim iCountryCode As Integer = Convert.ToInt32(cboCountry.SelectedValue)
        Dim isMale As Boolean = rblGender.SelectedIndex = 0
        Dim sUserId As String = Membership.GetUser().ProviderUserKey.ToString()


        SqlHelper.ExecuteNonQuery("UPDATE_Members", New SqlParameter("@UserId", New Guid(sUserId)), New SqlParameter("@CountryCode", iCountryCode), New SqlParameter("@Gender", isMale), New SqlParameter("@BirthDate", birthDate))

        ' Upload Photo
        If FileUpload1.HasFile Then

            Dim sTempDir As String = "/_Uploads/Temp/"

            If Not Directory.Exists(MapPath(sTempDir)) Then
                Directory.CreateDirectory(MapPath(sTempDir))
            End If

            Dim sUploadFile As String = FileUpload1.FileName ' original file
            Dim sDesFilename As String = sUserId + ".jpg" ' create a jpeg version of filename
            Dim sTempFilename As String = "TempImage" + DateTime.Now.Second.ToString() + Path.GetExtension(sUploadFile) ' create a temporary filename
            FileUpload1.SaveAs(MapPath(sTempDir + sTempFilename)) ' upload to temp folder

            ' GET SIZE RATIO
            Dim imageSource As System.Drawing.Image = ImageUtil.GetImage(MapPath(sTempDir + sTempFilename))
            Dim iWidth As Integer = 118
            Dim iHeight As Integer = (imageSource.Height * iWidth) / imageSource.Width

            ' resize the image / create thumbnail
            ImageUtil.GenerateThumbnail(imageSource, MapPath("/Videos/Members/" + sDesFilename), iWidth, iHeight, System.Drawing.Imaging.ImageFormat.Jpeg)

            imageSource.Dispose() ' FREE UP MEMORY

            ' DELETE TEMPORARY FILE
            Try
                File.Delete(MapPath(sTempDir + sTempFilename))
            Catch ex As Exception

            End Try


            Me.imageUser.ImageUrl = "/Videos/Members/" + sDesFilename ' show preview
            'imagePhoto.ImageUrl = imagePreview.ImageUrl;
            'hiddenImageFilename.Value = sDesFilename;
        End If


        lblStatus.Text = "Update successful."
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdCancel.Click
        Response.Redirect("/Member/", True)
    End Sub
End Class
