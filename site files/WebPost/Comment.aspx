<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Comment.aspx.cs" Inherits="Comment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
        <asp:TextBox ID="TextBox2" Text="Enter Comment here" runat="server" Height="136px" 
            Width="185px">Enter content here</asp:TextBox>
        <br />
        <asp:TextBox ID="TextBox3" Text="Post #" runat="server" Width="67px" 
            ></asp:TextBox>
        <asp:TextBox ID="TextBox4" runat="server" Width="92px">Comment #</asp:TextBox>
        <br />
    
        <asp:TextBox ID="TextBox1" Text="Name" runat="server" Width="109px">Name</asp:TextBox>
        <asp:Button PostBackUrl="http://kenfujioka.name/WebPost/SendComment.aspx" ID="Button1" runat="server" Text="Send" Width="55px" />

    </form>
</body>
</html>
