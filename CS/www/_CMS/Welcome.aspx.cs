using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace DES.Web.cmsadmin
{
	/// <summary>
	/// Summary description for AdminMain.
	/// </summary>
	public partial class AdminMain : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			if(!Page.IsPostBack)
			{
                lWebAppName.Text = ConfigurationManager.AppSettings["System.WebName"]; // ConfigurationManager.AppSettings["WebAppName"];
			}
		}
	}
}
