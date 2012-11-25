<%@ Page Language="C#" MasterPageFile="~/_CMS/MasterPage.master" AutoEventWireup="true"
    CodeFile="Members.aspx.cs" Inherits="_CMS_Members" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="header" style="color: White">
                VisualVid Members</td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    DataSourceID="SqlDataSource1" CellPadding="4" ForeColor="#333333" GridLines="None"
                    Width="100%" AutoGenerateColumns="False" DataKeyNames="UserId" OnRowCommand="GridView1_RowCommand" PageSize="15">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Left" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Actions">
                            <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CommandName="View_Videos" ImageUrl="~/_CMS/i/tv/o.gif"
                                    ID="Imagebutton1" AlternateText="Videos" CommandArgument='<%# Eval("UserId") %>'>
                                </asp:ImageButton>
                                <asp:ImageButton OnClientClick="return confirm('Are you sure you want to delete this user including the videos?');"
                                    runat="server" CommandName="Custom_Delete" ImageUrl="i/ico_x.gif" ID="Imagebutton2"
                                    AlternateText='Delete' CommandArgument='<%# Eval("UserId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approved" SortExpression="IsApproved">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton4" ToolTip="Toggle Approved" runat="server" ImageUrl='<%# DES.WebHelper.SetStateImage(Eval("IsApproved")) %>'
                                    CommandName="Toggle_Approved" CommandArgument='<%# Eval("UserId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Locked" SortExpression="IsLockedOut">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton3" ToolTip="Toggle Locked (Unlock Only)" runat="server"
                                    ImageUrl='<%# DES.WebHelper.SetStateImage(Eval("IsLockedOut")) %>' CommandName="Toggle_Locked"
                                    CommandArgument='<%# Eval("UserId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                        <asp:BoundField DataField="Videos" HeaderText="Videos" SortExpression="Videos" />
                        <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName" />
                        <asp:TemplateField SortExpression="BirthDate" HeaderText="Birth Date">
                            <ItemTemplate>
                                <%# Eval("BirthDate", "{0:MMMM d, yyyy}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="CreateDate" HeaderText="Created">
                            <ItemTemplate>
                                <%# Eval("CreateDate") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="LastActivityDate" HeaderText="Last Activity">
                            <ItemTemplate>
                                <%# Eval("LastActivityDate") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="Gender" HeaderText="Male" SortExpression="Gender" />
                    </Columns>
                    <PagerSettings PageButtonCount="25" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                    SelectCommand="SELECT_Members" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="false">
                    <SelectParameters>
                        <asp:Parameter Name="UserId" Type="String" ConvertEmptyStringToNull="true" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
