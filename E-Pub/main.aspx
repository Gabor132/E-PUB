<%@ Page Title="main" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="E_Pub.main" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <fieldset>
            <legend>
                <asp:Label ID="profileUserLabel" runat="server" Text=""></asp:Label>
            </legend>
            <asp:Label ID="profileEmailLabel" runat="server" Text="Email: "></asp:Label>

        <asp:DropDownList ID="ProfileTypeList" runat="server" OnSelectedIndexChanged="ProfileTypeList_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem>PUBLIC</asp:ListItem>
            <asp:ListItem>PRIVATE</asp:ListItem>
        </asp:DropDownList>

            <br />
                <asp:Button ID="friendRequestButton" runat="server" Text="Send Friend Request" OnClick="friendRequestButton_Click" />
            <asp:Button ID="acceptFriendRequestButton" runat="server" Text="Accept Friend Request" OnClick="acceptFriendRequestButton_Click" />
            <asp:Button ID="deleteFriendRequestButton" runat="server" Text="Delete Request" OnClick="deleteFriendRequestButton_Click" />
            <asp:Label ID="friendRequestLabel" runat="server" Text="Friend request sent"></asp:Label>
        </fieldset>
        <fieldset>
            <legend>Friends</legend>
            <asp:SqlDataSource ID="FriendsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT case u1.username when @username then u2.username else u1.username end &quot;FRIENDS&quot;
 FROM FRIEND_REQUESTS AS fr INNER JOIN USERS AS u1 ON fr.sender_id = u1.id INNER JOIN USERS AS u2 ON fr.receiver_id = u2.id
  WHERE ((u2.username = @username) OR (u1.username = @username)) AND fr.response = 'ACCEPTED';">
                <SelectParameters>
                    <asp:QueryStringParameter Name="username" QueryStringField="profile" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="friendList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="FriendsDataSource" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="FRIENDS" HeaderText="FRIENDS" ReadOnly="True" SortExpression="FRIENDS" />
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </fieldset>
        <fieldset>
            <legend>Albums</legend>

            <asp:SqlDataSource ID="SqlDataAlbums" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT a.album_name as &quot;ALBUM NAME&quot; , a.date_creation as &quot;CREATION DATE&quot; FROM ALBUMS a JOIN USERS u ON (a.user_id = u.id) WHERE u.username = @username;">
                <SelectParameters>
                    <asp:QueryStringParameter Name="username" QueryStringField="profile" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="AlbumView" runat="server" AllowPaging="True" AllowSorting="True" CellPadding="4" DataSourceID="SqlDataAlbums" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="125px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged1">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="ALBUM NAME" HeaderText="ALBUM NAME" SortExpression="ALBUM NAME" />
                    <asp:BoundField DataField="CREATION DATE" HeaderText="CREATION DATE" SortExpression="CREATION DATE" />
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>

            <asp:Label ID="CreateAlbumLabel" runat="server" Text="Album name: "></asp:Label>
            <asp:TextBox ID="CreateAlbumText" runat="server"></asp:TextBox>

            <asp:Button ID="CreateAlbumButton" runat="server" Text="Create album" OnClick="CreateAlbumButton_Click" ValidationGroup="a" />

            <asp:Label ID="WarningCreateAlbum" runat="server" ForeColor="Red" Visible="False"></asp:Label>

            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="CreateAlbumText" ValidationGroup="a">Albumul are nevoie de un nume!</asp:RequiredFieldValidator>

        </fieldset>
</asp:Content>
