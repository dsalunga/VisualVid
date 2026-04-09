<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.vb" Inherits="Register" title="Member Registration" ValidateRequest="false" %>
<%@ Register Src="Controls/MemberLogin.ascx" TagName="MemberLogin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="800" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td colspan="4" height="10">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
        <tr>
            <td width="10">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
            <td valign="top">
                <table width="388" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <img src="images/newmember.jpg" width="388" height="37" alt="" border="0"></td>
                    </tr>
                    <tr>
                        <td bgcolor="#8a6dc6" style="padding: 20px; color: #666666; background-image: url(images/newmember_bg.jpg);
                            background-position: bottom;">
                            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" DisableCreatedUser="True"
                                OnCreatedUser="CreateUserWizard1_CreatedUser" FinishDestinationPageUrl="~/Login.aspx"
                                CreateUserButtonImageUrl="~/Images/signup.png" CreateUserButtonType="Image" OnCreatingUser="CreateUserWizard1_CreatingUser"
                                CancelDestinationPageUrl="~/Default.aspx" ContinueDestinationPageUrl="~/Login.aspx"
                                OnSendingMail="CreateUserWizard1_SendingMail" InvalidPasswordErrorMessage="Your password length must be at least {0} characters.">
                                <WizardSteps>
                                    <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                                        <ContentTemplate>
                                            <div class="field">
                                                <div class="label2">
                                                    E-mail Address<asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                                        ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></div>
                                                <asp:TextBox ID="Email" CssClass="input2" runat="server"></asp:TextBox>
                                            </div>
                                            <br />
                                            <div class="field">
                                                <div class="label2">
                                                    Username<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></div>
                                                <asp:TextBox ID="UserName" runat="server" CssClass="input2"></asp:TextBox>
                                            </div>
                                            <div class="field">
                                                <div class="label2">
                                                    Password<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></div>
                                                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="input2"></asp:TextBox>
                                            </div>
                                            <div class="field">
                                                <div class="label2">
                                                    Confirm Password<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server"
                                                        ControlToValidate="ConfirmPassword" ErrorMessage="Confirm Password is required."
                                                        ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></div>
                                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" CssClass="input2"></asp:TextBox>
                                            </div>
                                            <br>
                                            <div class="field">
                                                <div class="label2">
                                                    Security Question<asp:RequiredFieldValidator ID="QuestionRequired" runat="server"
                                                        ControlToValidate="Question" ErrorMessage="Security question is required." ToolTip="Security question is required."
                                                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></div>
                                                <asp:TextBox ID="Question" runat="server" CssClass="input2"></asp:TextBox>
                                            </div>
                                            <div class="field">
                                                <div class="label2">
                                                    Security Answer<asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer"
                                                        ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                                                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></div>
                                                <asp:TextBox ID="Answer" CssClass="input2" runat="server"></asp:TextBox>
                                            </div>
                                            <br />
                                            <div class="field">
                                                <div class="label2">
                                                    Country&nbsp;</div>
                                                <asp:DropDownList ID="cboCountry" runat="server" DataSourceID="SqlDataSource1" DataTextField="Name"
                                                    DataValueField="CountryCode" CssClass="input2">
                                                </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                                    SelectCommand="SELECT_Countries" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                            </div>
                                            <br />
                                            <div class="field">
                                                <div class="label2">
                                                    Gender<asp:RequiredFieldValidator ID="GenderRequired" runat="server" ControlToValidate="rblGender"
                                                        ErrorMessage="Gender is required." ToolTip="Gender is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></div>
                                                <img src="images/spacer.gif" width="5" height="1" alt="" border="0">
                                                <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem>Male</asp:ListItem>
                                                    <asp:ListItem>Female</asp:ListItem>
                                                </asp:RadioButtonList></div>
                                            <br />
                                            <div class="field">
                                                <div class="label2">
                                                    Date of Birth&nbsp;</div>
                                                <asp:DropDownList ID="cboYear" runat="server" CssClass="bdate" Width="68px">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cboDay" runat="server" CssClass="bdate" Width="52px">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cboMonth" runat="server" CssClass="bdate" Width="94px">
                                                </asp:DropDownList>
                                            </div>
                                            <br />
                                            <div class="field">
                                                <asp:CheckBox ID="chkAgree" runat="server" Style="margin-left: 20px; margin-right: 10px;" />I
                                                agree to the <a href="Terms.aspx">terms of use</a> and <a href="Privacy.aspx">privacy policy</a>.</div>
                                            <br>
                                            <div>
                                                <div>
                                                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                        ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                                                        ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div>
                                                <div align="center" colspan="2" style="color: red">
                                                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:CreateUserWizardStep>
                                    <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0" align="center">
                                                <tr>
                                                    <td align="center" colspan="2" style="width: 347px">
                                                        Complete</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Thank you for signing up.  You have successfully created your account.</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2" style="width: 347px">
                                                        <asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue"
                                                            Text="Continue" ValidationGroup="CreateUserWizard1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:CompleteWizardStep>
                                </WizardSteps>
                                <MailDefinition IsBodyHtml="True">
                                </MailDefinition>
                            </asp:CreateUserWizard>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img src="images/newmember_bottom.jpg" width="388" height="56" alt="" border="0"></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" EnableViewState="False"></asp:Label></td>
                    </tr>
                </table>
            </td>
            <td valign="top" width="305">
                <uc2:MemberLogin ID="MemberLogin1" runat="server" />
                <p>
                    <img src="images/benefits.jpg" width="248" height="45" alt="" border="0"></p>
                <div id="benefits">
                    <ul>
                        <li>Fusce porttitor nibh et sem. Nullam volutpat dui at sem. </li>
                        <li>Fusce porttitor nibh.. Nullam dui at sem. </li>
                        <li>Fusce porttitor nibh et sem. Nolutpat dui at sem. </li>
                        <li>Fusce porttitor sem. Nullam volutpat dui at sem. </li>
                        <li>Fusce porttitor nibh et sem. Nullam volutpat dui at sem. </li>
                    </ul>
                    <p>
                        Read more about the benefits in our <a href="About.aspx">About Us</a> page</p>
                </div>
            </td>
            <td width="30">
                <img src="images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
    </table>
</asp:Content>
