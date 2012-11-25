<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Contact.aspx.cs" Inherits="Contact" ValidateRequest="false" %>

<%@ Register Src="Controls/MemberLogin.ascx" TagName="MemberLogin" TagPrefix="uc2" %>
<%@ Register Src="Controls/RecentVideos.ascx" TagName="RecentVideos" TagPrefix="uc1" %>
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
                    Contact Us</div>
                <br />
                Aliquam sed lorem at dui consectetuer ultrices. Curabitur ut nisi eu velit luctus
                placerat. Curabitur a mi. Sed pharetra, magna ac dapibus pretium, diam arcu rhoncus
                nunc, in adipiscing est pede feugiat enim. In euismod mauris sit amet velit. Mauris
                dui. Curabitur nulla mauris, nonummy sit amet, nonummy vitae, dapibus vitae, dolor.
                Morbi porta mauris in augue. Sed eget nulla nec arcu dignissim rutrum. Aenean malesuada
                accumsan nibh. Maecenas fermentum diam. Fusce nec ipsum pulvinar velit suscipit
                viverra. Sed non est non mi posuere sodales. Curabitur mi mi, mollis ultricies,
                gravida eget, tempor id, libero. Suspendisse ullamcorper lacinia justo. Aenean eget
                quam. Nullam mollis, neque vitae viverra dictum, metus orci dapibus nisl, sit amet
                euismod nisl lorem eu nibh. Fusce consectetuer, ligula sit amet pharetra scelerisque,
                ipsum libero nonummy magna, vel dictum elit arcu eget neque. Etiam scelerisque,
                lectus ut vestibulum dapibus, eros purus cursus risus, eget sollicitudin odio sapien
                adipiscing diam. Aenean at tortor eget nunc vestibulum volutpat.
            </td>
            <td valign="top" width="305">
                <uc2:MemberLogin ID="MemberLogin1" runat="server" />
                <uc1:RecentVideos ID="RecentVideos1" runat="server" />
            </td>
            <td width="30">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>
