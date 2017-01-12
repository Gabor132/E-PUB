<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="E_Pub.login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend>Login</legend>
            <div>
                <asp:Label ID="loginUserLabel" runat="server" Text="Username: "></asp:Label>
                <asp:TextBox ID="userBox" runat="server" ValidationGroup="a"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="userBox" ErrorMessage="Username can't be null" ValidationGroup="a"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="loginPasslabel" runat="server" Text="Password: "></asp:Label>
                <asp:TextBox ID="passBox" runat="server" TextMode="Password" ValidationGroup="a"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="passBox" ErrorMessage="Password can't be null" ValidationGroup="a"></asp:RequiredFieldValidator>
                <br />
                <asp:Button ID="loginButton" runat="server" Text="Login" OnClick="loginButton_Click" /><br />
                <asp:Label ID="loginWarningLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
            </div>
        </fieldset>
        
        <a href="register.aspx">To Register</a>
        <p>
            <asp:Button ID="guestButton" runat="server" Text="Login as Guest" OnClick="guestButton_Click" />
        </p>
    </form>
</body>
</html>
