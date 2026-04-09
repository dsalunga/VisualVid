<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="About.aspx.vb" Inherits="About" title="About Us" ValidateRequest="false" %>
<%@ Register Src="Controls/RecentVideos.ascx" TagName="RecentVideos" TagPrefix="uc3" %>
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
            <td valign="top" style="padding: 20px;">
            <div class="title">
                    About Us</div>
                <br />
                Nulla ac arcu. Donec porta, turpis nonummy volutpat luctus, eros eros dictum arcu,
                et iaculis tortor massa eu arcu. Suspendisse feugiat quam non erat euismod laoreet.
                Fusce a neque. Nam ac elit. Vivamus enim leo, convallis at, semper nec, tincidunt
                sed, velit. Etiam odio nulla, ullamcorper nec, lobortis quis, lacinia et, ante.
                Donec ac turpis id velit pulvinar mollis. Pellentesque eu nulla. Sed posuere. Sed
                a nulla. Donec lectus eros, fringilla non, gravida sed, egestas et, nibh. Suspendisse
                posuere metus ac leo. Maecenas commodo diam.
            </td>
            <td valign="top" width="305">
                <uc1:MemberLogin ID="MemberLogin1" runat="server" />
                <uc3:RecentVideos ID="RecentVideos1" runat="server" />
            </td>
            <td width="30">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>

