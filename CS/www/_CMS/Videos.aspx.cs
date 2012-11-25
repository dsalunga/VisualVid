using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DES;

public partial class _CMS_Videos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string sUserId = Request.QueryString["UserId"];
            if (!string.IsNullOrEmpty(sUserId))
            {
                DropDownList1.DataBind();
                try
                {
                    DropDownList1.SelectedValue = sUserId;
                }
                catch { }

                DataList1.DataBind();
            }
        }
    }
    protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string sID = e.CommandArgument.ToString();

        switch (e.CommandName)
        {
            case "Custom_Delete":
                object obj = SqlHelper.ExecuteScalar(CommandType.Text,
                    "SELECT UserId FROM Videos WHERE VideoId=@VideoId",
                    new SqlParameter("@VideoId", new Guid(sID))
                );

                SqlHelper.ExecuteNonQuery(CommandType.Text,
                    "DELETE FROM Videos WHERE VideoId=@VideoId",
                    new SqlParameter("@VideoId", new Guid(sID))
                );
                DataList1.DataBind();

                string sFile = MapPath("/Videos/Members/" + obj + "/" + sID);
                try
                {
                    File.Delete(sFile + ".flv");
                    File.Delete(sFile + ".jpg");
                }
                catch { }
                break;
        }
    }
}
