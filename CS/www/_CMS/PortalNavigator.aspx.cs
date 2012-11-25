using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DES;

public partial class _CMS_PortalNavigator : System.Web.UI.Page
{
    //private bool showOrphan;
    //private bool showSiteSecurity;
    //private bool serverMode;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        // Put user code to initialize the page here

        if (!Page.IsPostBack)
        {
            this.BuildTreeView();
        }
    }

    private void BuildTreeView()
    {
        /*
        string sUserID = null;
        if (!Roles.IsUserInRole("Administrators"))
        {
            sUserID = Membership.GetUser().ProviderUserKey.ToString();
        }
        */

        //showOrphan = SystemSettings.GetSettings("System.ShowOrphan") == "true";
        //serverMode = SystemSettings.GetSettings("System.TreeViewServerMode") == "true";
        //showSiteSecurity = SystemSettings.GetSettings("System.ShowSiteSecurity") == "true";

        //t.EnableClientScript = !serverMode;
        //t.EnableViewState = serverMode;
        t.Nodes.Clear();

        // INIT TREE VIEW
        TreeNode tn1, tn2, tn3;

        TreeNode tnRoot = new TreeNode(ConfigurationManager.AppSettings["System.WebName"]);
        tnRoot.NavigateUrl = "PortalHeader.aspx";

        /*
        if (Roles.IsUserInRole("Administrators")) // PERMISSION
        {
            tn1 = new TreeNode("Site Templates");
            tn1.SelectAction = TreeNodeSelectAction.Expand;
            //tn1.NavigateUrl = "SiteAdmins.aspx";
            {
                tn2 = new TreeNode("Template Pages");
                tn2.ImageUrl = "~/_CMS/i/tv/o.gif";
                tn2.NavigateUrl = "PageTemplates.aspx?PageType=L";
                tn1.ChildNodes.Add(tn2);

                tn2 = new TreeNode("Style Sheets");
                tn2.ImageUrl = "~/_CMS/i/tv/o.gif";
                tn2.NavigateUrl = "CSSTemplates.aspx";
                tn1.ChildNodes.Add(tn2);
            }
            tnRoot.ChildNodes.Add(tn1);
        }
        */

        tn1 = new TreeNode("Members");
        tn1.ImageUrl = "~/_CMS/i/tv/u.gif";
        tn1.NavigateUrl = "Members.aspx";
        tnRoot.ChildNodes.Add(tn1);

        tn1 = new TreeNode("Videos");
        tn1.ImageUrl = "~/_CMS/i/tv/o.gif";
        tn1.NavigateUrl = "Videos.aspx";
        tnRoot.ChildNodes.Add(tn1);

        tn1 = new TreeNode("Password");
        tn1.ImageUrl = "~/_CMS/i/tv/l.gif";
        tn1.NavigateUrl = "Password.aspx";
        tnRoot.ChildNodes.Add(tn1);

        //tn1 = new TreeNode("Web Site");
        //tn1.ImageUrl = "~/_CMS/i/tv/ws.gif";
        //tnRoot.ChildNodes.Add(tn1);
        


        t.Nodes.Clear();
        t.Nodes.Add(tnRoot);
        t.CollapseAll();

        t.Nodes[0].Expanded = true;
        //t.Target = "frameRight";
        //t.Target = "_blank";
    }


    protected void cmdSync_Click(object sender, EventArgs e)
    {
        this.BuildTreeView();
    }
}
