<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="E_Pub.register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <fieldset>
            <legend>Register</legend>

            <asp:Label ID="registerUserLabel" runat="server" Text="Username: "></asp:Label>

            <asp:TextBox ID="regUserBox" runat="server" ValidationGroup="a"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="regUserBox" ErrorMessage="Username can't be null" ValidationGroup="a"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="registerPassLabel" runat="server" Text="Password: "></asp:Label>
            <asp:TextBox ID="regPassBox" runat="server" TextMode="Password" ValidationGroup="a"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="regPassBox" ErrorMessage="Password can't be null" ValidationGroup="a"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="regPassBox" ControlToValidate="regPass2Box" ErrorMessage="Password must be the same" ValidationGroup="a"></asp:CompareValidator>
            <br />
            <asp:Label ID="registerPassLabel2" runat="server" Text="Repeat Password: "></asp:Label>
            <asp:TextBox ID="regPass2Box" runat="server" TextMode="Password" ValidationGroup="a"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="regPass2Box" ErrorMessage="Password can't be null" ValidationGroup="a"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="registerEmailLabel" runat="server" Text="Email: "></asp:Label>
            <asp:TextBox ID="regEmailBox" runat="server" TextMode="Email" ValidationGroup="a"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="regEmailBox" ErrorMessage="Email can't be null" ValidationGroup="a"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="registerButton" runat="server" Text="Register" OnClick="registerButton_Click" /><br />
            <asp:Label ID="registerWarningLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
            <br />

        </fieldset>
        
        <a href="login.aspx">To Login</a>
    </div>
    </form>
</body>
</html>
