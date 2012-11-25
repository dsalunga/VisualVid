<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Master.aspx.vb" Inherits="_CMS_Master" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Welcome to the Content Management System</title>

    <script language="JavaScript" type="text/javascript">
  // this page should never load inside of another frame
  if (top.location != self.location)
    {
    top.location = self.location;
    }
    </script>

</head>
<frameset cols="250,*" framespacing="6" frameborder="1" bordercolor="#3366cc">
    <frame name="frameLeft" src="PortalNavigator.aspx" marginheight="0" marginwidth="0"
            frameborder="0" />
    <frame name="_f" src="Welcome.aspx" frameborder="no" />
</frameset>
</html>
