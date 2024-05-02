<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Read.aspx.cs" Inherits="ASP.NET_Bulletin.Read" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #TextArea1
        {
            width: 369px;
            margin-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="Button1" runat="server" Text="Return to Main View" 
            PostBackUrl="http://kenfujioka.name/faq/Bulletin/ASP.NET/" Width="191px"/>
            <asp:Button
                ID="Button2" runat="server" Text="Post a Bulletin" 
            PostBackUrl="http://webpost.kenfujioka.name" Width="185px" />

     <asp:TreeView ID="TreeView1" runat="server" Width="503px" Font-Size="Medium" ShowCheckBoxes="All">
     </asp:TreeView>   
    </div>
    </form>

    
</body>
</html>
