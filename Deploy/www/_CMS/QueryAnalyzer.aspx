<%@ Page Language="c#" Inherits="DES.Web.CMS.QueryAnalyzer" ValidateRequest="False"
    CodeFile="QueryAnalyzer.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Site :: Administration - <% =DateTime.Now %></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="styles/style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="frmSection" method="post" runat="server">
        <table width="100%">
            <tr>
                <td><asp:TextBox ID="txtCode" runat="server" Columns="25"></asp:TextBox>&nbsp;
                    <asp:TextBox ID="txtQS" runat="server" Columns="50"></asp:TextBox>
                    <asp:CheckBox ID="chkCustom" runat="server" Text="Custom" /></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtQuery" runat="server" Rows="15" TextMode="MultiLine" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="cmdExecute" runat="server" Width="80px" Text="Execute" OnClick="cmdExecute_Click">
                    </asp:Button><asp:RadioButton ID="chkQuery" runat="server" Text="Query" GroupName="radioOptions" Checked="True" />&nbsp;<asp:RadioButton ID="chkDownload" runat="server" Text="Download" GroupName="radioOptions" />&nbsp;
                    <asp:RadioButton ID="chkUpload" runat="server" Text="Upload" GroupName="radioOptions" />
                    <asp:FileUpload ID="FileUpload1" runat="server" />&nbsp;
                    <asp:CheckBox ID="chkSchema" runat="server" Text="Schema" /></td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    <asp:PlaceHolder ID="phGrid" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
