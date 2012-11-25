<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MostWatched.ascx.vb" Inherits="Controls_MostWatched" %>
<img src="/images/top5.jpg" width="147" height="26" alt="" border="0"><br />
<asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
<ItemTemplate>
<!--VIDEO ONE-->
<table width="330" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top" colspan="3" width="330" height="12">
            <img src="/images/top5_top.jpg" width="330" height="12" alt="" border="0"></td>
    </tr>
    <tr>
        <td width="15" valign="top" style="background-image: url(/images/top5_left.jpg);">
            <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        <td width="300" valign="top" style="background-color: #cccccc; padding: 0px; margin: 0px;
            border: 0px;">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <a href='/Watch.aspx?VideoId=<%# Eval("VideoId") %>' title=''>
                                <img width="92" height="64" src="/Videos/Members/<%# Eval("UserId") %>/<%# Eval("VideoId") %>.jpg"
                                    border="0" align="left" alt='Watch <%# Eval("Title") %>' /></a></td>
                    <td valign="top" style="padding-left: 5px;">
                        <strong><%# Eval("Title") %></strong>
                        <br />
                        <%# Eval("Description") %>
                        <br />
                        <br />
                        Date Added:&nbsp;<%# Eval("DateAdded", "{0:MMMM d, yyyy}") %><br>
                        From:&nbsp;<a href='/Profile.aspx?UserId=<%# Eval("UserId") %>' class="from"><%# Eval("UserName") %></a><br>
                        Viewed:&nbsp;<%# Eval("Views") %> times<br>
                        <!--Rating:
                        <img src="/images/star.jpg" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                            width="3" height="1" alt="" border="0">
                        <img src="/images/star.jpg" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                            width="3" height="1" alt="" border="0">
                        <img src="/images/star.jpg" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                            width="3" height="1" alt="" border="0">
                        <img src="/images/star.jpg" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                            width="3" height="1" alt="" border="0">
                        <img src="/images/star.jpg" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                            width="3" height="1" alt="" border="0">
                        (5.0/5.0)-->
                    </td>
                </tr>
            </table>
        </td>
        <td width="15" valign="top" style="background-image: url(/images/top5_right.jpg);">
            <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
    </tr>
    <tr>
        <td valign="top" colspan="3" width="330" height="24">
            <img src="/images/top5_bottom.jpg" width="330" height="24" alt="" border="0"></td>
    </tr>
</table>
<!--VIDEO ONE-->
</ItemTemplate>
</asp:Repeater>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
    SelectCommand="SELECT_Videos_MostWatched" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

<br>
<div align="right">
    <a href="Browse.aspx" title="">
        <img src="/images/archive.jpg" width="83" height="19" alt="" border="0"></a><img src="/images/spacer.gif"
            width="125" height="1" alt="" border="0"></div>