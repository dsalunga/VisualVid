<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Confirm.aspx.cs" Inherits="Confirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="800" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td align="center">
                <br />
                <br />
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="viewSuccess" runat="server">
                        Thank you! Your account is now active.<br />You can start uploading your videos by logging-in.
                    </asp:View>
                    <asp:View ID="viewNoNeed" runat="server">
                        Your account has been activated already.
                    </asp:View>
                    <asp:View ID="viewFailed" runat="server">
                        Sorry!
                    </asp:View>
                </asp:MultiView>
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
