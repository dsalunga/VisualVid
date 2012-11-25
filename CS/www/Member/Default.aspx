<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Member_Default" %>

<%@ Register Src="../Controls/RecentVideos.ascx" TagName="RecentVideos" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="800" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td colspan="4" height="10">
                <img src="/Images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
        <tr>
            <td width="10">
                <img src="/Images/spacer.gif" width="1" height="1" alt="" border="0"></td>
            <td valign="top">
                <table width="388" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td background="/Images/welcome2.jpg" height="37">
                            <span class="username" id="spanUsername" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#8A6DC6" style="padding: 15px; color: #666666; background-image: url(/images/newmember_bg.jpg);
                            background-position: bottom;">
                            <div>
                                <img id="imageUser" runat="server" src="/Images/img_userpic.jpg" alt="" align="left" />
                                &nbsp;&nbsp;<strong>Username:</strong>&nbsp;<span id="spanUsername2" runat="server"></span><br />
                                &nbsp;&nbsp;<strong>Gender:</strong>&nbsp;<span id="spanGender" runat="server"></span><br />
                                &nbsp;&nbsp;<strong>Age:</strong>&nbsp;<span id="spanAge" runat="server"></span><br />
                                &nbsp;&nbsp;<strong>Country:</strong>&nbsp;<span id="spanCountry" runat="server"></span><br />
                                &nbsp;&nbsp;<strong>E-mail:</strong>&nbsp;<span id="spanEmail" runat="server"></span><br />
                                &nbsp;&nbsp;<strong>Since:</strong>&nbsp;<span id="spanMemberSince" runat="server"></span><br />
                                &nbsp;&nbsp;<strong>Watched:</strong>&nbsp;<span id="spanWatched" runat="server"></span><br />
                                <br />
                            </div>
                            <div align="right">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <a href="Profile.aspx" title="">
                                                <img src="/Images/btn_update.png" width="70" height="29" alt="" border="0" /></a></td>
                                        <td>
                                            <a href="Password.aspx" title="">
                                                <img src="/Images/btn_changepass.png" alt="" border="0" /></a></td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <img src="/Images/spacer.gif" width="1" height="20" alt=""></div>
                            <div class="line">
                                <img src="/Images/spacer.gif" width="1" height="1" alt=""></div>
                            <div>
                                <img src="/Images/spacer.gif" width="1" height="20" alt=""></div>
                            <div>
                                <img src="/Images/title_upvid.png" width="109" height="23" alt=""></div>
                            <div align="right">
                                <a href="Upload.aspx" title="">
                                    <img src="/Images/btn_upload.png" width="70" height="29" alt="" border="0"></a></div>
                            <!--<div>
                                <img src="/Images/spacer.gif" width="1" height="20" alt=""></div>
                                -->
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GridLines="None" ShowHeader="False"
                                CellPadding="0" Width="100%" OnRowCommand="GridView1_RowCommand" PageSize="5">
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
                                                From: <a href='/Profile.aspx?UserId=<%# Eval("UserId") %>' class="from" target="_blank">
                                                    <%# Eval("UserName") %>
                                                </a>
                                                <div style="padding: 2px">
                                                    <asp:ImageButton ID="LinkButton1" CommandName="Custom_Edit" CommandArgument='<%# Bind("VideoId") %>'
                                                        runat="server" ImageUrl="~/Images/btn_edit.png"></asp:ImageButton>
                                                    <asp:ImageButton OnClientClick="return confirm('Are you sure you want to delete this item?');"
                                                        ID="cmdDelete" CommandName="Custom_Delete" CommandArgument='<%# Bind("VideoId") %>'
                                                        runat="server" ImageUrl="~/Images/btn_delete.png"></asp:ImageButton>
                                                </div>
                                            </p>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings PageButtonCount="25" NextPageImageUrl="/images/btn_rightarrow.png"
                                    PreviousPageImageUrl="/images/btn_leftarrow.png" />
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
                            <br />
                            <!--    
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top" width="19">
                                        <img src="/Images/btn_leftarrow.png" width="19" height="20" alt=""></td>
                                    <td>
                                        <div id="videoblock" align="left">
                                            <center><img src="/Images/recent1.jpg" width="92" height="64" alt=""><br><strong>Mauris scel...</strong><br>3 weeks ago</center>
                                        </div>
                                        <div id="Div1">
                                            <center><img src="/Images/recent2.jpg" width="92" height="64" alt=""><br><strong>Mauris scel...<br></strong>1 month ago</center>
                                        </div>
                                        <div id="Div2">
                                            <center><img src="/Images/recent3.jpg" width="92" height="64" alt=""><br><strong>Mauris scel...<br></strong>1 month ago</center>
                                        </div>
                                    </td>
                                    <td valign="top" width="19">
                                        <img src="/Images/btn_rightarrow.png" width="19" height="20" alt=""></td>
                                </tr>
                                <tr>
                                    <td>
                                        <img src="/Images/spacer.gif" width="1" height="30" alt=""></td>
                                </tr>
                            </table>
                            -->
                            <div align="right">
                                <a href="Upload.aspx" title="">
                                    <img src="/Images/btn_upload.png" width="70" height="29" alt="" border="0"></a></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img src="/Images/newmember_bottom.jpg" width="388" height="56" alt="" border="0"></td>
                    </tr>
                </table>
            </td>
            <td valign="top" width="305">
                <p>
                    <img src="/Images/whatsnew.jpg" width="227" height="45" alt=""></p>
                <div id="benefits">
                    <ul>
                        <li>Fusce porttitor nibh et sem. Nullam volutpat dui at sem. </li>
                        <li>Fusce porttitor nibh.. Nullam dui at sem. </li>
                        <li>Fusce porttitor nibh et sem. Nolutpat dui at sem. </li>
                    </ul>
                </div>
                <div>
                    <img src="/Images/spacer.gif" width="1" height="20" alt=""></div>
                <uc1:RecentVideos ID="RecentVideos1" runat="server" />
            </td>
            <td width="30">
                <img src="/Images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>
