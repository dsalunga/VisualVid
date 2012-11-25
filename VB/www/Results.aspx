<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Results.aspx.vb" Inherits="Results" title="Search Results" ValidateRequest="false" %>
<%@ Register Src="Controls/MostWatched.ascx" TagName="MostWatched" TagPrefix="uc4" %>
<%@ Register Src="Controls/MemberLogin.ascx" TagName="MemberLogin" TagPrefix="uc2" %>
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
                <br />
                <h4><span id="spanText" runat="server">
                    Search Results for "<% =Request.QueryString("Search") %>"</span><hr />
                </h4>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GridLines="None" ShowHeader="False"
                    CellPadding="0" Width="100%" EmptyDataText="No videos found.">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <p>
                                    <a href='/Watch.aspx?VideoId=<%# Eval("VideoId") %>' title=''>
                                        <img width="92" height="64" src="/Videos/Members/<%# Eval("UserId") %>/<%# Eval("VideoId") %>.jpg"
                                            border="0" align="left" hspace="10" alt='<%# Eval("Title") %>' /></a><strong>
                                                <%# Eval("Title") %>
                                            </strong>
                                    <br />
                                    Date Added:
                                    <%# Eval("DateAdded", "{0:MMMM d, yyyy}") %>
                                    <br />
                                    From: <a href='/Profile.aspx?UserId=<%# Eval("UserId") %>' class="from">
                                        <%# Eval("UserName") %>
                                    </a>
                                    <br />
                                    Viewed:&nbsp;<%# Eval("Views") %>&nbsp;times
                                </p>
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                &nbsp;
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                    SelectCommand="SELECT_Videos" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:Parameter Name="UserId" Type="String" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="CategoryID" Type="Int32" ConvertEmptyStringToNull="true" />
                        <asp:Parameter DefaultValue="True" Name="IsActive" Type="Boolean" />
                        <asp:QueryStringParameter Name="Keyword" QueryStringField="Search" ConvertEmptyStringToNull="true" />
                        <asp:QueryStringParameter Name="Sort" QueryStringField="Sort" ConvertEmptyStringToNull="true" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td valign="top" width="305">
                <uc2:MemberLogin ID="MemberLogin1" runat="server" />
                <img src="images/video_browse.jpg" width="48" height="19" alt="" border="0">
                <div class="videolist">
                    <ul>
                        <li><a href='Results.aspx?Sort=DateAdded&Search=<% =Request.QueryString("Search") %>'>Most Recent</a></li>
                        <li><a href='Results.aspx?Sort=Views&Search=<% =Request.QueryString("Search") %>'>Most Viewed</a></li>
                    </ul>
                </div>
            </td>
            <td width="30">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>
