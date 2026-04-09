<%@ Control Language="VB" ClassName="PageHeader1" %>
<%@ Register Src="LoginControls.ascx" TagName="LoginControls" TagPrefix="uc2" %>
<%@ Register Src="HeaderSearch.ascx" TagName="HeaderSearch" TagPrefix="uc1" %>

<script runat="server">

</script>

<div class="header" align="center">
    <div id="s800" align="left">
        <table width="750" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td valign="top" width="454">
                    <table width="454" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td valign="top" height="80" style="background-image: url(/images/logo_bg.jpg);">
                                <a href="/">
                                    <img src="/images/logo.jpg" width="192" height="80" alt="" border="0"></a></td>
                        </tr>
                        <tr>
                            <td valign="top" height="40">
                                <table width="85%" style="height: 40px" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="10">
                                            <img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
                                        <td>
                                            <a href="/">
                                                <img src="/images/nav_homeA.jpg" width="37" height="40" alt="VisualVid Home" border="0"></a></td>
                                        <td>
                                            <a href="/Browse.aspx">
                                                <img src="/images/nav_videos.jpg" width="47" height="40" alt="Video Archive" border="0"></a></td>
                                        <td>
                                            <a href="/About.aspx">
                                                <img src="/images/nav_aboutus.jpg" width="62" height="40" alt="About Us" border="0"></a></td>
                                        <td>
                                            <a href="/Contact.aspx">
                                                <img src="/images/nav_contactus.jpg" width="72" height="40" alt="Contact Us" border="0"></a></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table width="321" style="height: 112px" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td valign="bottom" width="321" height="112" style="background-image: url(/images/disc.jpg);
                                padding: 0px; margin: 0px; border: 0px;">
                                <uc2:LoginControls ID="LoginControls1" runat="server" /><img src="/images/spacer.gif" width="1" height="75" alt="" border="0"><img src="/images/spacer.gif" width="65" height="1" alt="" border="0"><uc1:HeaderSearch
                                    ID="HeaderSearch1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
