<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WebTesting._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
			<h2><asp:Label ID="title" runat="server" /></h2>
			<asp:Image ID="image" runat="server" />
			<p>
				<asp:HyperLink ID="link" runat="server" Text="More information" />
			</p>
    </div>
    </form>
</body>
</html>
