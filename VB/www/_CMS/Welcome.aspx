<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Welcome.aspx.vb" Inherits="_CMS_Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Site :: Administration</title>
    <link rel="stylesheet" type="text/css" href="styles/style.css">
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" height="200">
                    <span class="welcome">Welcome to&nbsp;<asp:Literal ID="lWebAppName" runat="server"></asp:Literal>&nbsp;Administration
                        Page!</span></td>
            </tr>
        </table>
    </form>
</body>
</html>
