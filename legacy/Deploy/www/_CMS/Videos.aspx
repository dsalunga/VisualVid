<%@ Page Language="VB" MasterPageFile="~/_CMS/MasterPage.master" AutoEventWireup="true" CodeFile="Videos.aspx.vb" Inherits="_CMS_Videos" title="Member Videos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="header" style="color: White">
                Member Videos</td>
        </tr>
        <tr>
            <td class="control_box">
                VisualVid User:&nbsp;
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2"
                    DataTextField="UserName" DataValueField="UserId" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True" Value="">All Users</asp:ListItem>
                </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                    SelectCommand="SELECT V.UserId, U.UserName FROM Videos AS V INNER JOIN aspnet_Users AS U ON V.UserId = U.UserId GROUP BY V.UserId, U.UserName ORDER BY U.UserName">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" CellPadding="0"
                    RepeatColumns="2" OnItemCommand="DataList1_ItemCommand">
                    <ItemTemplate>
                        <itemtemplate>
                                <p>
                                    <a href='/Watch.aspx?VideoId=<%# Eval("VideoId") %>' target="_blank" title=''>
                                        <img width="92" height="64" src="/Videos/Members/<%# Eval("UserId") %>/<%# Eval("VideoId") %>.jpg"
                                            border="0" align="left" hspace="10" alt='<%# Eval("Title") %>' /></a><strong>
                                                <%# Eval("Title") %>
                                            </strong>
                                    <br />
                                    <div>
                                    Date Added:
                                    <%# Eval("DateAdded", "{0:MMMM d, yyyy}") %>
                                    </div>
                                    From: <a onclick="return false;" href='/Profile.aspx?UserId=<%# Eval("UserId") %>' target="_blank" class="from">
                                        <%# Eval("UserName") %>
                                    </a>
                                    <br />
                                    Viewed:&nbsp;<%# Eval("Views") %> times
                                    <div style="padding: 4px">
                                    <asp:LinkButton Visible="false" CommandName="Deactivate_Video" CommandArgument='<%# Bind("VideoId") %>'
                                    runat="server" ID="cmdDeactivate" Text="[ Deactivate ]" Font-Overline="false"></asp:LinkButton>
                                <asp:LinkButton OnClientClick="return confirm('Are you sure you want to delete this item?');"
                                    ID="cmdDelete" CommandName="Custom_Delete" CommandArgument='<%# Bind("VideoId") %>'
                                    runat="server" Text="[ Delete ]" Font-Overline="false"></asp:LinkButton>
                            </div>
                                </p><br />
                            </itemtemplate>
                    </ItemTemplate>
                </asp:DataList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                    SelectCommand="SELECT_Videos" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="false">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownList1" Name="UserId" PropertyName="SelectedValue"
                            Type="String" ConvertEmptyStringToNull="true" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>