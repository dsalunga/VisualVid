<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Watch.aspx.vb" Inherits="Watch" ValidateRequest="false" %>
<%@ Register Src="Controls/MemberLogin.ascx" TagName="MemberLogin" TagPrefix="uc2" %>
<%@ Register Src="Controls/RecentVideos.ascx" TagName="RecentVideos" TagPrefix="uc3" %>
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
                <span class="videotitle" id="spanTitle" runat="server"></span>
                <br />
                <span id="spanDescription" runat="server"></span>
                <br />
                <br />
                <table width="320" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td valign="top">
                            <p id="flvplayer" style="height: 260px; width: 320px">
                                <a href="http://www.macromedia.com/go/getflashplayer">Get the Flash Player</a> to
                                see this player.</p>

                            <script type="text/javascript">
	                            var FO = { 	movie:"/Videos/flvplayer.swf",width:"320",height:"260",majorversion:"7",build:"0",bgcolor:"#FFFFFF",flashvars:"file=<% =sVideo %>&autoStart=true" };
	                            UFO.create(FO, "flvplayer");
                            </script>

                            <!--
                            <img src="/images/video.jpg" width="320" height="266" alt="" border="0">-->
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <img src="/images/video_ref.jpg" width="320" height="32" alt="" border="0"></td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding: 5px;">
                            <strong>Date Added:</strong>&nbsp;<span id="spanDateAdded" runat="server"></span><br />
                            <strong>From:</strong>&nbsp;<a href="#" class="from" id="aUserName" runat="server">pulvinar
                                vel</a><br>
                            <strong>Viewed:</strong>&nbsp;<span id="spanViews" runat="server"></span> times<br>
                            <!--<strong>Rating:</strong>
                            <img src="/images/star2.gif" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                                width="3" height="1" alt="" border="0">
                            <img src="/images/star2.gif" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                                width="3" height="1" alt="" border="0">
                            <img src="/images/star2.gif" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                                width="3" height="1" alt="" border="0">
                            <img src="/images/star2.gif" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                                width="3" height="1" alt="" border="0">
                            <img src="/images/star2.gif" width="12" height="13" alt="" border="0"><img src="/images/spacer.gif"
                                width="3" height="1" alt="" border="0">
                            (5.0/5.0)<br />-->
                            <strong>Tags:</strong>&nbsp;<span id="spanTags" runat="server"></span><!--<a href="#" class="tags">weird</a>, <a href="#" class="tags">
                                ducks</a>, <a href="#" class="tags">funny</a>-->
                        </td>
                    </tr>
                    <tr>
                        <td height="20">
                            <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            <a href="#">
                                <img src="/images/postvideo.jpg" width="98" height="39" alt="" border="0"></a><img
                                    src="/images/postdiv.jpg" width="1" height="39" alt="" border="0"><a href="#"><img
                                        src="/images/sharevid.jpg" width="114" height="39" alt="" border="0"></a><img src="/images/postdiv.jpg"
                                            width="1" height="39" alt="" border="0"><a href="#"><img src="/images/ratevid.jpg"
                                                width="106" height="39" alt="" border="0"></a></td>
                    </tr>
                    <tr>
                        <td height="20">
                            <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
                    </tr>
                    <tr>
                        <td class="comment">
                            Comments and Critique</td>
                    </tr>
                    <tr>
                        <td height="5">
                            <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <!--
                            <div style="float: left">
                                <a href="#" class="from">viel all 69 comments</a></div>-->
                            <div style="float: left">
                                <a runat="server" id="aComment1" href="#" onclick="document.getElementById('divComment').style.display=''; return false;"
                                    class="from">post a comment</a></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                            <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divComment" style="display: none">
                                Post a comment:<br />
                                <asp:TextBox ID="txtComment" runat="server" Columns="47" Rows="5" TextMode="MultiLine"></asp:TextBox><br />
                                <asp:Button ID="cmdComment" runat="server" Height="30px" Text="Submit Comment" OnClick="cmdComment_Click" /><br />
                                <br />
                            </div>
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                                DataSourceID="SqlDataSource1" EmptyDataText="No comments for this video.<br /><br />" AutoGenerateColumns="False"
                                GridLines="None" ShowHeader="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <table width="320" cellpadding="0" cellspacing="0" border="0">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="commentbar">
                                                    <div style="float: left">
                                                        <a href='/Profile.aspx?UserId=<%# Eval("UserId") %>' class="from"><%# Eval("UserName") %></a>&nbsp;(<%# Eval("DatePosted") %>)</div>
                                                        <!--
                                                    <div style="float: right">
                                                        <a href="#" class="from">reply</a></div>-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 10px;">
                                                    <%# Eval("Content") %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10">
                                                    <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                SelectCommand="SELECT_Comments" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="true">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="VideoId" QueryStringField="VideoId" Type="String"
                                        ConvertEmptyStringToNull="true" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <!--
                            <div style="float: left">
                                <a href="#" class="from">viel all 69 comments</a></div>-->
                            <div style="float: left">
                                <a id="aComment2" runat="server" href="#" onclick="document.getElementById('divComment').style.display=''; return false;"
                                    class="from">post a comment</a></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" width="305">
                <uc2:MemberLogin ID="MemberLogin1" runat="server" />
                <uc3:RecentVideos ID="RecentVideos1" runat="server" />
            </td>
            <td width="30">
                <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>
