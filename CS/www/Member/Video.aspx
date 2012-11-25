<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Video.aspx.cs" Inherits="Member_Video" %>

<%@ Register Src="UserNavigation.ascx" TagName="UserNavigation" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="800" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td colspan="4" height="10">
                <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
        <tr>
            <td width="10">
                <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
            <td valign="top">
                <table border="0">
                    <tr>
                        <td valign="top">
                            <a href="" title='' runat="server" id="aVideo">
                                <img width="92" id="imageVideo" runat="server" height="64" border="0" align="left"
                                    hspace="10" alt="" /></a></td>
                        <td>
                            <table align="left">
                                <tr>
                                    <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                        Title<asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                                            ErrorMessage="Title">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtTitle" runat="server" Columns="50"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                        Description<asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                            ErrorMessage="Description">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtDescription" runat="server" Columns="50" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                        Tags<asp:RequiredFieldValidator ID="rfvTags" runat="server" ControlToValidate="txtTags"
                                            ErrorMessage="Tags">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtTags" runat="server" Columns="50"></asp:TextBox><br />
                                        <span class="formFieldInfo"><strong>Enter one or more tags, separated by spaces.</strong><br />
                                            Tags are keywords used to describe your video so it can be easily found by other
                                            users.<br />
                                            For example, if you have a surfing video, you might tag it: <code>surfing beach waves</code>.</span></td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                        Video Category<asp:RequiredFieldValidator ID="rfvVideoCategory" runat="server" ControlToValidate="rblCategories"
                                            ErrorMessage="Video Category">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:RadioButtonList ID="rblCategories" runat="server" DataSourceID="SqlDataSource1"
                                            DataTextField="Name" DataValueField="CategoryID" RepeatColumns="2">
                                        </asp:RadioButtonList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                            SelectCommand="SELECT_Categories" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                        &nbsp;<br />
                                        <asp:ImageButton ID="cmdUpload" runat="server" ImageUrl="~/Images/btn_update.png"
                                            OnClick="cmdUpload_Click" />&nbsp;
                                        <asp:ImageButton ID="cmdCancel" runat="server" CausesValidation="False" ImageUrl="~/Images/btn_cancel.png"
                                            OnClick="cmdCancel_Click" /><br />
                                        <br />
                                        <asp:ValidationSummary ID="vsSummary" runat="server" HeaderText="The following are required:" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="30">
                <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>
