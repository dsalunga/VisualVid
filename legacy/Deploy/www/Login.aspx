<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" title="Member Login" ValidateRequest="false" %>
<%@ Register Src="Controls/RecentVideos.ascx" TagName="RecentVideos" TagPrefix="uc2" %>
<%@ Register Src="Controls/MemberLogin.ascx" TagName="MemberLogin" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="800" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td colspan="4" height="10">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
        <tr>
            <td width="10">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
            <td valign="top">
                <uc1:MemberLogin ID="MemberLogin1" runat="server" />
            </td>
            <td valign="top" width="305">
                <uc2:RecentVideos ID="RecentVideos1" runat="server" />
            </td>
            <td width="30">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>