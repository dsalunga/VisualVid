<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Login.aspx.vb" Inherits="_CMS_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Site :: Administration</title>
    <link rel="stylesheet" type="text/css" href="styles/style.css" />

    <script type="text/javascript">
        if (top.location != self.location)
        {
            top.location.href = self.location.href;
        }
    </script>

</head>
<body>
    <form id="frmLogin" method="post" runat="server">
        <table width="100%" border="0" style="height: 500px">
            <tr>
                <td>
                    <table width="530" cellpadding="0" cellspacing="0" align="center" border="0">
                        <tr>
                            <td colspan="3">
                                <img src="images/login_topframe.gif" width="530" height="21" alt="" border="0"></td>
                        </tr>
                        <tr>
                            <td width="1" bgcolor="#666666">
                                <img src="Images/spacer.gif" width="1" height="1" alt="" border="0" /></td>
                            <td width="528" align="center">
                                <a href="/" title="">
                                    <img src="../_Uploads/Image/logo.jpg" alt="" border="0"></a></td>
                            <td width="1" bgcolor="#666666">
                                <img src="images/spacer.gif" width="1" height="1" alt="" border="0" /></td>
                        </tr>
                        <tr>
                            <td width="1" bgcolor="#666666">
                                <img src="images/spacer.gif" width="1" height="1" alt="" border="0" /></td>
                            <td width="528" align="center">
                                <img src="images/spacer.gif" width="1" height="25" alt="" border="0" />
                                <br />
                                <asp:MultiView ID="MultiView1" runat="server">
                                    <asp:View ID="viewLogin" runat="server">
                                        <asp:Login ID="Login1" runat="server" LoginButtonImageUrl="images/btn_login_gray.jpg"
                                            LoginButtonType="Image" DestinationPageUrl="~/_CMS/Master.aspx" OnLoggingIn="Login1_LoggingIn">
                                            <LayoutTemplate>
                                                <table border="0" cellpadding="5">
                                                    <tr>
                                                        <td align="right">
                                                            <img src="images/login_usernametext.gif" width="74" height="15" alt="" border="0" /></td>
                                                        <td>
                                                            <asp:TextBox ID="UserName" runat="server" Columns="30" CssClass="login_input"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <img src="images/login_passwordtext.gif" width="75" height="15" alt="" border="0" /></td>
                                                        <td>
                                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" Columns="30" CssClass="login_input"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2" style="color: red">
                                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <asp:ImageButton ID="LoginImageButton" runat="server" AlternateText="Log In" CommandName="Login"
                                                                ImageUrl="images/btn_login_gray.jpg" ValidationGroup="Login1" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left">
                                                            Forgot your password? <a href="Login.aspx?Mode=Forgot" title="Click here to recover your password">
                                                                Click here</a></td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                        </asp:Login>
                                    </asp:View>
                                    <asp:View ID="viewForgot" runat="server">
                                        <asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
                                            <SuccessTemplate>
                                                <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0">
                                                                <tr>
                                                                    <td align="left">
                                                                        Your password has been sent to you.<br />
                                                                        <a href="Login.aspx" title="Click here to login">Click here to login</a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </SuccessTemplate>
                                        </asp:PasswordRecovery>
                                    </asp:View>
                                    <asp:View ID="view1" runat="server">
                                        <asp:MultiView ID="MultiView2" runat="server">
                                            <asp:View ID="viewSuccess" runat="server">
                                                Thank you! Your account has been successfully activated.
                                                <br />
                                                <a href="Login.aspx" title="Click here to login">Click here to login</a>
                                            </asp:View>
                                            <asp:View ID="viewNoNeed" runat="server">
                                                Your account has been activated already.
                                            </asp:View>
                                            <asp:View ID="viewFailed" runat="server">
                                                Sorry!
                                            </asp:View>
                                        </asp:MultiView>
                                    </asp:View>
                                </asp:MultiView>
                                <br />
                                <img src="images/spacer.gif" width="1" height="20" alt="" border="0" /></td>
                            <td width="1" bgcolor="#666666">
                                <img src="images/spacer.gif" width="1" height="1" alt="" border="0" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <img src="images/login_bottomframe.gif" width="530" height="19" alt="" border="0" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>