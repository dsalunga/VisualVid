Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Diagnostics

Imports DES

Partial Class Member_Upload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            MultiView1.SetActiveView(viewStep1)
        End If
    End Sub

    Protected Sub cmdFinish_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles cmdFinish.Click
        Response.Redirect("/Member/", True)
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdCancel.Click
        Response.Redirect("/Member/", True)
    End Sub

    Protected Sub cmdGoToUpload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdGoToUpload.Click
        MultiView1.SetActiveView(viewStep2)
    End Sub

    Protected Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles cmdUpload.Click
        If FileUpload1.HasFile Then
            Dim iMaxVideoSize As Integer = Convert.ToInt32(ConfigurationManager.AppSettings("System.MaxVideoSize"))
            Dim iFileSize As Integer = (FileUpload1.PostedFile.ContentLength / 1024) / 1024
            If iFileSize > iMaxVideoSize Then
                lblMessage.Text = String.Format("[ Sorry, the maximum video size is {0}, your file is {1} ]", iMaxVideoSize, iFileSize)
                Return
            End If

            Dim sTitle As String = txtTitle.Text.Trim()
            Dim sDescription As String = txtDescription.Text.Trim()
            Dim sTags As String = txtTags.Text.Trim()
            Dim iCategoryID As Integer = Convert.ToInt32(rblCategories.SelectedValue)
            Dim sOriginalExtension As String = Path.GetExtension(FileUpload1.FileName)
            Dim userKey As Object = Membership.GetUser().ProviderUserKey

            'string sGuid = SqlHelper.ExecuteScalar(CommandType.Text, "SELECT NEWID()").ToString()

            Dim obj As Object = SqlHelper.ExecuteScalar("UPDATE_Videos", New SqlParameter("@Title", sTitle), New SqlParameter("@Description", sDescription), New SqlParameter("@Tags", sTags), New SqlParameter("@CategoryID", iCategoryID), New SqlParameter("@OriginalExtension", sOriginalExtension), New SqlParameter("@UserId", userKey))
            Dim sGuid As String = obj.ToString()
            Dim sNewFilename As String = sGuid + sOriginalExtension

            FileUpload1.SaveAs(MapPath("/Videos/Pending/" + sNewFilename))

            Dim sMemberDir As String = MapPath("/Videos/Members/" + userKey.ToString())
            If Not Directory.Exists(sMemberDir) Then
                Directory.CreateDirectory(sMemberDir)
            End If

            Dim ms As Process() = Process.GetProcessesByName("VisualVid")
            If ms.Length = 0 Then
                ' foreach (Process m in ms){}
                ' Thread.Sleep(30000); // Wait for 30 seconds
                ' Console.WriteLine("Encoder running. Wait for 30 seconds...");

                Dim p As Process = New Process()
                p.StartInfo.FileName = MapPath("/Videos/bin/VisualVid.exe")
                p.StartInfo.WorkingDirectory = MapPath("/Videos/bin")
                p.Start()
                p.PriorityClass = ProcessPriorityClass.BelowNormal
            End If

            MultiView1.SetActiveView(viewDone)
        Else
            lblMessage.Text = "[ Invalid upload file. ]"
        End If
    End Sub
End Class
