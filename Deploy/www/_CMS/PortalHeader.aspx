<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PortalHeader.aspx.vb" Inherits="_CMS_PortalHeader" %>

<%@ Register TagPrefix="uc1" TagName="LoginInfo" Src="Controls/LoginInfo.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/_CMS/styles/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        if (top.location.pathname.toLowerCase() == "/_cms/portalheader.aspx")
        {
            top.location.href = "Master.aspx";
        }
    </script>
</head>
<body>
    <form runat="server" id="frmMain">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <!--Begin top image-->
                        <tr>
                            <td valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <!-- 
                                    <td width="340" valign="top">
                                        <img alt="" src="/Uploads/Image/logo.jpg"> </td>-->
                                        <td align="left" style="padding: 2px; background-image: url(/Uploads/Image/HeaderBackground.jpg);
                                            background-position: right; background-repeat: no-repeat">
                                            <uc1:LoginInfo ID="LoginInfo2" runat="server"></uc1:LoginInfo>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <!--End top image-->
                        <tr>
                            <td valign="top" background="images/filler01.gif">
                                <img src="images/filler01.gif" width="4" height="10"></td>
                        </tr>
                        <tr>
                            <td valign="top" background="images/top_menu.gif">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="3">
                                            <img src="images/top_menu.gif" width="3" height="25"></td>
                                        <!--Begin top nav-->
                                        <td>
                                            <table cellspacing="5" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td width="100%">
                                                        &nbsp;</td>
                                                    <td nowrap>
                                                        <asp:LoginStatus ID="LoginStatus1" runat="server" CssClass="linkw" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <!--End top nav-->
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>