<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginControls.ascx.vb" Inherits="Controls_LoginControls" %>
<div id="hfooter">
    <asp:LoginView ID="LoginView1" runat="server">
        <AnonymousTemplate>
            <a href="/Register.aspx">Sign Up</a> |
        </AnonymousTemplate>
        <LoggedInTemplate>
            <a href="/Member/">My Account</a> |</LoggedInTemplate>
    </asp:LoginView>
    <asp:LoginStatus TabIndex="5" ID="LoginStatus1" runat="server" LoginText="Log In" LogoutText="Log Out" LogoutAction="Redirect" LogoutPageUrl="/" />
    <br />
    <asp:LoginName ID="LoginName1" runat="server" FormatString="<span style='font-weight:normal'>Welcome<span>, <span style='color:yellow;font-weight:normal'>{0}</span>" />
    &nbsp;
</div>