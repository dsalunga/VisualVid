<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RecentVideos.ascx.vb" Inherits="Controls_RecentVideos" %>
<img src="/images/recently.jpg" width="157" height="27" alt="" border="0" />
<asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
    <ItemTemplate>
        <p>
            <a href='/Watch.aspx?VideoId=<%# Eval("VideoId") %>' title=''>
                <img width="92" height="64" src="/Videos/Members/<%# Eval("UserId") %>/<%# Eval("VideoId") %>.jpg"
                    border="0" align="left" hspace="10" alt='Watch <%# Eval("Title") %>' /></a> <strong>
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
        </p>
        <br />
    </ItemTemplate>
</asp:Repeater>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
    SelectCommand="SELECT_Videos_Featured" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
<div align="right"><a href="/Browse.aspx?Sort=DateAdded" title="Video Archive">
    <img src="/images/archive.jpg" width="83" height="19" alt="" border="0"><img src="/images/spacer.gif"
        width="5" height="1" alt="" border="0" /></a></div>