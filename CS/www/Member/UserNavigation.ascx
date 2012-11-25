<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserNavigation.ascx.cs" Inherits="Member_UserNavigation" %>
<table width="300" cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td align="center">
                <fieldset>
                    <legend style="font-weight: bold">My Account</legend>
                    <br />
                    <div style="text-align: left; width: 200px">
                        &bull;&nbsp;<a class="from" href="Profile.aspx" title="Change my profile">Change my profile</a>
                        <br />
                        &bull;&nbsp;<a class="from" href="Password.aspx" title="Change my password">Change my password</a>
                    </div>
                    <br />
                </fieldset>
                <br />
                <fieldset>
                    <legend style="font-weight: bold">My Videos</legend>
                    <br />
                    <div style="text-align: left; width: 200px">
                        &bull;&nbsp;<a class="from" href="Upload.aspx" title="Upload New Video">Upload New Video</a>
                        <br />
                        &bull;&nbsp;<a class="from" href="Videos.aspx" title="My Uploaded Videos">My Uploaded Videos</a>
                    </div>
                    <br />
                </fieldset>
                <br />
                <br />
            </td>
        </tr>
    </table>