<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Browse.aspx.cs" Inherits="Browse" ValidateRequest="false" %>

<%@ Register Src="Controls/MemberLogin.ascx" TagName="MemberLogin" TagPrefix="uc2" %>
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
                <p>
                    <img src="/images/videos.jpg" width="49" height="26" alt="" border="0" /></p>
                <!--
                <div id="videopage">
                    Pages:<img src="images/spacer.gif" width="5" height="1" alt="" border="0">
                    <a href="#">1</a> <a href="#">2</a> <a href="#">3</a> <a href="#">4</a> <a href="#">
                        5</a></div>
                        -->
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GridLines="None" ShowHeader="False"
                    CellPadding="0" PageSize="10" Width="373px">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div style="padding-bottom: 20px; padding-top: 10px">
                                    <a href='/Watch.aspx?VideoId=<%# Eval("VideoId") %>' title=''>
                                        <img width="92" height="64" src="/Videos/Members/<%# Eval("UserId") %>/<%# Eval("VideoId") %>.jpg"
                                            border="0" align="left" hspace="10" alt='Watch <%# Eval("Title") %>' /></a><strong>
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
                                    Viewed:&nbsp;<%# Eval("Views") %>
                                    times
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings PageButtonCount="25" Position="TopAndBottom" />
                    <PagerStyle CssClass="videopage" />
                </asp:GridView>
                &nbsp;
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                    SelectCommand="SELECT_Videos" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:Parameter Name="UserId" Type="String" />
                        <asp:Parameter Name="CategoryID" Type="Int32" />
                        <asp:Parameter DefaultValue="True" Name="IsActive" Type="Boolean" />
                        <asp:QueryStringParameter Name="Sort" QueryStringField="Sort" ConvertEmptyStringToNull="true" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <br>
                <!--
                <div id="videopage">
                    Pages:<img src="images/spacer.gif" width="5" height="1" alt="" border="0">
                    <a href="#">1</a> <a href="#">2</a> <a href="#">3</a> <a href="#">4</a> <a href="#">
                        5</a></div>
                        -->
            </td>
            <td valign="top" width="305">
                <uc2:MemberLogin ID="MemberLogin1" runat="server" />
                <img src="/images/video_browse.jpg" width="48" height="19" alt="" border="0">
                <div class="videolist">
                    <ul>
                        <li><a href="Browse.aspx?Sort=DateAdded">Most Recent</a></li>
                        <li><a href="Browse.aspx?Sort=Views">Most Viewed</a></li>
                        <!--
                        <li><a href="#">Top Rated</a></li>
                        <li><a href="#">Most Discussed</a></li>
                        <li><a href="#">Top Favorites</a></li>
                        <li><a href="#">Most Linked</a></li>
                        -->
                    </ul>
                </div>
                <!--
                <br>
                <br>
                <img src="images/video_time.jpg" width="34" height="19" alt="" border="0">
                <div class="videolist">
                    <ul>
                        <li><a href="#">Today</a></li>
                        <li><a href="#">This Week</a></li>
                        <li><a href="#">This Month</a></li>
                        <li><a href="#">All Time</a></li>
                    </ul>
                </div>
                <br>
                <br>
                <img src="images/video_category.jpg" width="61" height="25" alt="" border="0">
                <div class="videolist">
                    <ul>
                        <li><a href="#">All</a></li>
                        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource2">
                            <ItemTemplate>
                                <li><a href="#">
                                    <%# Eval("Name") %>
                                </a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                            SelectCommand="SELECT_Categories" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </ul>
                </div>
                -->
            </td>
            <td width="30">
                <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>
