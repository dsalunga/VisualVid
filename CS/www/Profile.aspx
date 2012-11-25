<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Profile.aspx.cs" Inherits="Gela.VisualVid.MemberProfile" ValidateRequest="false" %>

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
                <div class="title">
                    User Profile</div>
                <br />
                <table cellpadding="5">
                    <tr>
                        <td valign="top">
                            <img id="imageUser" runat="server" src="/Images/img_userpic.jpg" title="" /></td>
                        <td>
                            <span id="spanUsername" runat="server" style="font-weight: bold; font-size: 10pt"></span>
                            <br />
                            <strong>Age:</strong>&nbsp;<span id="spanAge" runat="server"></span>
                            <br />
                            <strong>Gender:</strong>&nbsp;<span id="spanGender" runat="server"></span>
                            <br />
                            <strong>Country:</strong>&nbsp;<span id="spanCountry" runat="server"></span>
                            <br />
                            <strong>E-mail:</strong>&nbsp;<span id="spanEmail" runat="server"></span>
                            <br />
                            <br />
                            <br />
                            <span style="font-weight: bold; font-size: 10pt">About Me</span>
                            <br />
                            <strong>Member Since:</strong>&nbsp;<span id="spanMemberSince" runat="server"></span>
                            <br />
                            <strong>Videos Watched:</strong>&nbsp;<span id="spanWatched" runat="server"></span>
                            <br />
                            <strong>Last Login:</strong>&nbsp;<span id="spanLastLogin" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" width="305">
                <span class="title">User Videos</span>
                <br />
                <br />
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GridLines="None" ShowHeader="False"
                    CellPadding="0" Width="100%" OnRowCommand="GridView1_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <p>
                                    <a href='/Watch.aspx?VideoId=<%# Eval("VideoId") %>' title=''>
                                        <img width="92" height="64" src="/Videos/Members/<%# Eval("UserId") %>/<%# Eval("VideoId") %>.jpg"
                                            border="0" align="left" hspace="10" alt='<%# Eval("Title") %>' /></a> <strong>
                                                <%# Eval("Title") %>
                                            </strong>
                                    <br />
                                    Date Added:
                                    <%# Eval("DateAdded", "{0:MMMM d, yyyy}") %>
                                    <br />
                                    From: <a class="from">
                                        <%# Eval("UserName") %>
                                    </a>
                                </p>
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings PageButtonCount="25" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                    SelectCommand="SELECT_Videos" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hiddenUserId" Name="UserId" PropertyName="Value"
                            Type="String" />
                        <asp:Parameter Name="CategoryID" Type="Int32" />
                        <asp:Parameter DefaultValue="True" Name="IsActive" Type="Boolean" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:HiddenField ID="hiddenUserId" runat="server" />
            </td>
            <td width="30">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0" /></td>
        </tr>
    </table>
</asp:Content>
