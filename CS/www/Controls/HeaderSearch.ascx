<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderSearch.ascx.cs"
    Inherits="Controls_HeaderSearch" %>
<asp:TextBox ID="txtSearch" CssClass="search" runat="server"></asp:TextBox><img src="/images/spacer.gif" width="10" height="1" alt="" border="0" /><asp:ImageButton runat="server" ID="cmdSearch" ImageUrl="~/images/search.png" Width="64"
    Height="29" BorderWidth="0px" OnClick="cmdSearch_Click" ToolTip="Search Keyword" />