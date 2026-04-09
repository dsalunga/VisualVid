<%@ Page Language="c#" Inherits="DES.Web.cmsadmin.Upload" CodeFile="Upload.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://schemas.microsoft.com/intellisense/xhtml-transitional-10">
<head>
    <title>Upload Image</title>
    <link rel="stylesheet" type="text/css" href="/_CMS/Styles/Style.css" />

    <script language="javascript" type="text/javascript">
		function FindControl(sControlName)
		{
		    return window.opener.document.getElementById(sControlName);
		}
    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
        <div style="padding: 5px; background-color: #1c6295; color: White; font-size: 14px;
            text-align: center; font-weight: bold">
            Upload File</div>
        <br />
        <div style="text-align: center">
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="viewUpload" runat="server">
                    <table cellpadding="4" cellspacing="4">
                        <tr>
                            <td align="left">
                                <span style="font-weight: bold">Upload to:</span>
                                <br />
                                <asp:TextBox ID="txtUploadTo" runat="server" Columns="50" /></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <input id="fileToUpload" type="file" name="fileToUpload" runat="server" size="35">
                                <br />
                                <asp:CheckBox ID="chkReplace" Text="Replace file if existing" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                                    Height="30px" Width="85px" /></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" /></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="viewDone" runat="server">
                    <asp:Label CssClass="header" ID="lblImageName" Font-Bold="True" runat="server" />
                    <br />
                    <br />
                    <asp:Button ID="cmdSave" runat="server" Width="85px" Text="Close" Height="30px" />
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
