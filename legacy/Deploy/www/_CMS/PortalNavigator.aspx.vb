
Partial Class _CMS_PortalNavigator
    Inherits System.Web.UI.Page

    Private Sub BuildTreeView()
        t.Nodes.Clear()

        ' INIT TREE VIEW
        Dim tn1 As TreeNode
        Dim tnRoot As TreeNode = New TreeNode(ConfigurationManager.AppSettings("System.WebName"))
        tnRoot.NavigateUrl = "PortalHeader.aspx"

        tn1 = New TreeNode("Members")
        tn1.ImageUrl = "~/_CMS/i/tv/u.gif"
        tn1.NavigateUrl = "Members.aspx"
        tnRoot.ChildNodes.Add(tn1)

        tn1 = New TreeNode("Videos")
        tn1.ImageUrl = "~/_CMS/i/tv/o.gif"
        tn1.NavigateUrl = "Videos.aspx"
        tnRoot.ChildNodes.Add(tn1)

        tn1 = New TreeNode("Password")
        tn1.ImageUrl = "~/_CMS/i/tv/l.gif"
        tn1.NavigateUrl = "Password.aspx"
        tnRoot.ChildNodes.Add(tn1)

        t.Nodes.Clear()
        t.Nodes.Add(tnRoot)
        t.CollapseAll()

        t.Nodes(0).Expanded = True
    End Sub

    Protected Sub cmdSync_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles cmdSync.Click
        Me.BuildTreeView()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Me.Load
        If Not Page.IsPostBack Then
            Me.BuildTreeView()
        End If
    End Sub
End Class
