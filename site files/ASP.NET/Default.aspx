<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="ASP.NET_Bulletin._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button2" runat="server" Text="Return to Main View" Width="189px" PostBackUrl="http://kenfujioka.name/faq/Bulletin/ASP.NET/Default.aspx" />

        <asp:Button ID="Button3" runat="server" Text="Post a Bulletin" Width="216px" PostBackUrl="http://webpost.kenfujioka.name/" />

        <br />
        <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="true" 
            Height="380px" Width="503px" Font-Size="Medium">        
        </asp:ListBox>
        <br />
    
    </div>
    </form>
</body>
</html>
