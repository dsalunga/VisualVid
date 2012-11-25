using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Text;

namespace DES.Web.cmsadmin
{
	/// <summary>
	/// Summary description for Upload.
	/// </summary>
	public partial class Upload : System.Web.UI.Page
	{
		private string sControl;
		//private string sUploadFolder;
		private bool isFileOnly = false;
		private string sDestFile;
        private int iStartTrim = 0;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			sControl = Request.QueryString["Control"];
			//sUploadFolder = Request.QueryString["UploadFolder"];
			sDestFile = Request.QueryString["Filename"];

            isFileOnly = !string.IsNullOrEmpty(Request.QueryString["FileOnly"]);

            string sStartTrim = Request.QueryString["StartCharsToTrim"];
            if (!string.IsNullOrEmpty(sStartTrim))
            {
                try
                {
                    iStartTrim = int.Parse(sStartTrim);
                }
                catch { }
            }

            if (!Page.IsPostBack)
            {
                txtUploadTo.Text = Request.QueryString["UploadFolder"];
                MultiView1.SetActiveView(viewUpload);
            }
		}

		protected void btnUpload_Click(object sender, System.EventArgs e)
		{
            string sUploadFolder = txtUploadTo.Text.Trim();

            string sFilename = Path.GetFileName(fileToUpload.PostedFile.FileName);
			
			if(!string.IsNullOrEmpty(sDestFile))
                sFilename = sDestFile + sFilename; // ALTER FILE

            string sFilePath = sUploadFolder + "/" + sFilename;
            string sFullFile = MapPath(sFilePath);

			try
			{
                //StringBuilder sb_script = new StringBuilder();
                string sScript = string.Empty;

                if (!Directory.Exists(MapPath(sUploadFolder)))
                {
                    Directory.CreateDirectory(MapPath(sUploadFolder));
                }

                if (File.Exists(sFullFile))
                {
                    if (chkReplace.Checked)
                    {
                        // Replace
                        fileToUpload.PostedFile.SaveAs(Server.MapPath(sFilePath));
                    }
                    else
                    {
                        // File exists, cancel upload
                        lblMessage.Text = "File already exists. Upload process terminated.";
                        return;
                    }
                }
                else
                {
                    fileToUpload.PostedFile.SaveAs(Server.MapPath(sFilePath));
                }
				

                lblImageName.Text = sFilename;
                MultiView1.SetActiveView(viewDone); // Change view
				
				if(!string.IsNullOrEmpty(sControl))
				{
					if(!isFileOnly)
					{
                        if (iStartTrim > 0)
                            sFilename = sUploadFolder.Remove(0, iStartTrim) + "/" + sFilename;
					}

                    sScript = string.Format("FindControl('{0}').value='{1}';", sControl, sFilename);
				}

                sScript += "self.close();";
                cmdSave.OnClientClick = sScript;
			}
			catch(Exception ex)
			{
				lblMessage.Text = ex.Message;
				//lblMessage.Visible = true;
			}
		}
	}
}
