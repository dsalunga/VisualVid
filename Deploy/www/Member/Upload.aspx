<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Upload.aspx.vb" Inherits="Member_Upload" title="Upload Video" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="800" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td>
                <br />
                <br />
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="viewStep1" runat="server">
                        <div>
                            <span class="title" style="font-weight: bold">Video Upload (Step 1 of 2)</span> &nbsp; <span style="font-size: 8pt">(All fields required)</span></div>
                        <p class="largeText">
                            Uploading a video is a two-step process—on the next page, you'll be able to choose
                            your video file and set the privacy settings.</p>
                        <div class="title">
                            Upload Tips</div>
                        <ul>
                            <li>Uploads will usually take 1-5 minutes per MB on a high-speed connection. </li>
                            <li>Converting your video takes a few minutes; you can add more info or upload more
                                videos while it's processing. </li>
                            <li>Videos are limited to 100 MB. </li>
                            <li>Videos saved with the following settings convert the best:
                                <ul>
                                    <li>MPEG4 (Divx, Xvid) format </li>
                                    <li>320x240 resolution </li>
                                    <li>MP3 audio </li>
                                    <li>30 frames per second framerate </li>
                                </ul>
                            </li>
                        </ul>
                        <table align="left">
                            <tr>
                                <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                    Title<asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                                        ErrorMessage="Title">*</asp:RequiredFieldValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtTitle" runat="server" Columns="50"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                    Description<asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                        ErrorMessage="Description">*</asp:RequiredFieldValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtDescription" runat="server" Columns="50" TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                    Tags<asp:RequiredFieldValidator ID="rfvTags" runat="server" ControlToValidate="txtTags"
                                        ErrorMessage="Tags">*</asp:RequiredFieldValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtTags" runat="server" Columns="50"></asp:TextBox><br />
                                    <span class="formFieldInfo"><strong>Enter one or more tags, separated by spaces.</strong><br />
                                        Tags are keywords used to describe your video so it can be easily found by other
                                        users.<br />
                                        For example, if you have a surfing video, you might tag it: <code>surfing beach waves</code>.</span></td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" style="font-weight: bold; width: 120px;">
                                    Video Category<asp:RequiredFieldValidator ID="rfvVideoCategory" runat="server" ControlToValidate="rblCategories"
                                        ErrorMessage="Video Category">*</asp:RequiredFieldValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:RadioButtonList ID="rblCategories" runat="server" DataSourceID="SqlDataSource1"
                                        DataTextField="Name" DataValueField="CategoryID" RepeatColumns="2">
                                    </asp:RadioButtonList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                        SelectCommand="SELECT_Categories" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                    <br />
                                    <asp:ImageButton ID="cmdGoToUpload" runat="server" ImageUrl="~/Images/btn_goupload.png"
                                        OnClick="cmdGoToUpload_Click" />
                                    <asp:ImageButton ID="cmdCancel" runat="server" CausesValidation="False" ImageUrl="~/Images/btn_cancel.png"
                                        OnClick="cmdCancel_Click" />
                                    <br />
                                    <br />
                                    <asp:ValidationSummary ID="vsSummary" runat="server" HeaderText="The following are required:" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="viewStep2" runat="server">
                        <h3>
                            Video Upload (Step 2 of 2)</h3>
                        <table cellpadding="3" width="100%">
                            <tr valign="top">
                                <td class="formField">
                                    <div class="formHighlight">
                                        <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" /><br />
                                        <strong>Max file size: 100 MB. Max length: 10 minutes.</strong>
                                        <p>
                                            <strong>Do not upload copyrighted, obscene or any other material which violates VisualVid's
                                                Terms of Use.</strong></p>
                                    </div>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="formField">
                                    <h4>
                                        Do not upload any TV shows, music videos, music concerts, or commercials without
                                        permission unless they consist entirely of content you created yourself.</h4>
                                    <p>
                                        By clicking "Upload Video," you are representing that this video does not violate
                                        VisualVid's <a href="/Terms.aspx" target="_blank">Terms of Use</a> and that you
                                        own all copyrights in this video or have express permission from the copyright owner(s)
                                        to upload it.</p>
                                    <p>
                                        Read <a href="/Privacy.aspx" target="_blank">Copyright Tips</a> for more information
                                        about copyright and VisualVid's policy.</p>
                                    <p>
                                        &nbsp;<asp:ImageButton ID="cmdUpload" runat="server" ImageUrl="~/Images/btn_uploadnow.png"
                                            OnClick="cmdUpload_Click" />&nbsp;</p>
                                    <p>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;</p>
                                    <div style="margin-top: 10px">
                                        <b>PLEASE BE PATIENT—THIS MAY TAKE SEVERAL MINUTES.<br />
                                            ONCE COMPLETED, YOU WILL SEE A CONFIRMATION MESSAGE.</b>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View runat="server" ID="viewDone">
                        <div style="font-weight: bold" class="title">
                            Uploading Done!</div>
                        <br />
                        <br />
                        Congratulations! Your video is current being processed.<br />
                        Please wait a little moment for your video to appear in the site.<br />
                        You will receive an email once your video is posted.
                        <br />
                        <br />
                        <asp:Button ID="cmdFinish" runat="server" Height="30px" Text="Done" Width="85px"
                            OnClick="cmdFinish_Click" /></asp:View>
                </asp:MultiView>
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>