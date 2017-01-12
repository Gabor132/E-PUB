<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="groupCreate.aspx.cs" Inherits="E_Pub.groupCreate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <legend>Create Group</legend>
    <asp:SqlDataSource ID="FriendsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT case u1.id when @id then u2.username else u1.username end &quot;FRIENDS&quot;
FROM FRIEND_REQUESTS AS fr INNER JOIN USERS AS u1 ON fr.sender_id = u1.id INNER JOIN USERS AS u2 ON fr.receiver_id = u2.id
WHERE ((u2.id = @id) OR (u1.id = @id)) AND fr.response = 'ACCEPTED';">
        <SelectParameters>
            <asp:SessionParameter Name="id" SessionField="userId" />
        </SelectParameters>
    </asp:SqlDataSource>
        <asp:Label ID="GroupNameLabel" runat="server" Text="Group name: "></asp:Label>
        <asp:TextBox ID="GroupNameTextBox" runat="server"></asp:TextBox>
        <asp:CheckBoxList ID="FriendList" runat="server" DataSourceID="FriendsDataSource" DataTextField="FRIENDS" DataValueField="FRIENDS">
        </asp:CheckBoxList>
        </br>
    <asp:Button ID="CreateGroupButton" runat="server" Text="Create Group" OnClick="CreateGroupButton_Click" />
    </fieldset>
</asp:Content>
