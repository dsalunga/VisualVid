<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemberLogin.ascx.cs" Inherits="Controls_MemberLogin" %>
<asp:Login ID="Login1" runat="server" FailureText="Your login attempt was not successful. Please try again.<br /><br />" DestinationPageUrl="~/Member/" VisibleWhenLoggedIn="False" OnLoggedIn="Login1_LoggedIn">
    <LayoutTemplate>
        <table width="263" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <img src="/images/login_top.jpg" width="263" height="38" alt="" border="0"></td>
            </tr>
            <tr>
                <td bgcolor="#8a6dc6" style="padding: 10px; color: white;">
                    <span style="color: yellow">
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                    <div class="label">
                        Username</div>
                    <asp:TextBox CssClass="input" ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
                    <br>
                    <div class="label">
                        Password</div>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
                    <div align="right">
                        <div align="left">
                            <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." /></div>
                        <asp:ImageButton runat="server" ID="LoginButton" CommandName="Login" ValidationGroup="ctl00$Login1"
                            ImageUrl="~/Images/signin.png" Width="64" Height="29" BorderWidth="0px" AlternateText=""
                            OnClick="LoginButton_Click" ToolTip="Sign In" /><img src="/images/spacer.gif" width="15"
                                height="1" alt="" border="0"></div>
                    Not yet a Member? <a href="/Register.aspx" class="logina">Sign-up</a><br>
                    Forgot username or password? Click <a href="/Forgot.aspx" class="logina">here</a>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/login_bottom.jpg" width="263" height="54" alt="" border="0"></td>
            </tr>
        </table>
        <br>
    </LayoutTemplate>
</asp:Login>
