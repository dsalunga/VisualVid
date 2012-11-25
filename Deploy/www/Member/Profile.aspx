<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Profile.aspx.vb" Inherits="Member_Profile" title="My Profile" %>
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
                        <td colspan="2" class="title">
                            Update your profile
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Photo:
                        </td>
                        <td>
                            &nbsp;<asp:Image ID="imageUser" runat="server" /><br />
                            <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>
                            Username:</td>
                        <td>
                            <asp:TextBox ID="txtUsername" runat="server" Columns="34" ReadOnly="True"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            Country
                        </td>
                        <td>
                            <asp:DropDownList ID="cboCountry" runat="server" DataSourceID="SqlDataSource1" DataTextField="Name"
                                DataValueField="CountryCode" CssClass="input2">
                            </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                SelectCommand="SELECT_Countries" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Gender<asp:RequiredFieldValidator ID="GenderRequired" runat="server" ControlToValidate="rblGender"
                                ErrorMessage="Gender is required." ToolTip="Gender is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>
                            Date of Birth
                        </td>
                        <td>
                            <asp:DropDownList ID="cboYear" runat="server" CssClass="bdate">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cboDay" runat="server" CssClass="bdate">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cboMonth" runat="server" CssClass="bdate">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;<asp:ImageButton ID="cmdUpdate" runat="server" ImageUrl="~/Images/btn_update.png"
                                OnClick="cmdUpdate_Click" />&nbsp;
                            <asp:ImageButton ID="cmdCancel" runat="server" CausesValidation="False" ImageUrl="~/Images/btn_cancel.png"
                                OnClick="cmdCancel_Click" /></td>
                    </tr>
                </table>
            </td>
            <td width="30">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0" /></td>
        </tr>
    </table>
</asp:Content>