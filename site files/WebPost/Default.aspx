<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="TextBox2" Text="Enter post here" runat="server" Height="136px" 
            Width="185px">Enter post here</asp:TextBox>
        <br />
        <asp:TextBox ID="TextBox3" Text="Title" runat="server" Width="184px">Title of post</asp:TextBox>
        <br />
    
        <asp:TextBox ID="TextBox1" Text="Name" runat="server" Width="109px">Name</asp:TextBox>
        <asp:Button PostBackUrl="http://kenfujioka.name/WebPost/Send.aspx" ID="Button1" runat="server" Text="Send" Width="55px" />
    
        <br />
        <a href="Comment.aspx">To post a comment click here</a></div>
    </form>
</body>
</html>
