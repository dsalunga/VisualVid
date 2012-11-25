<%@ Control Language="c#" Inherits="DES.Web._CMS.LoginInfo" CodeFile="LoginInfo.ascx.cs" %>
<table cellpadding="2" cellspacing="0" border="0">
    <tr>
        <td style="font-weight: bold">
            User Information<hr width="100%" size="1" noshade="noshade" />
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Username:&nbsp;</td>
                    <td>
                        <asp:Literal ID="lUsername" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>
                        Full Name:&nbsp;</td>
                    <td nowrap="nowrap"><% =Request.UserHostAddress %></td>
                </tr>
                <tr>
                    <td>
                        Last Login:&nbsp;</td>
                    <td nowrap>
                        <asp:Literal ID="lLastLogin" runat="server"></asp:Literal></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
