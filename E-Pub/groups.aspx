﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="groups.aspx.cs" Inherits="E_Pub.groups" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <legend>Groups</legend>
        
    <asp:SqlDataSource ID="SqlDataGroups" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT g.group_name &quot;GROUPS&quot; FROM GROUPS AS g INNER JOIN USER_GROUPS AS ug ON g.Id = ug.group_id WHERE (ug.user_id = @id)">
        <SelectParameters>
            <asp:SessionParameter Name="id" SessionField="userId" />
        </SelectParameters>
    </asp:SqlDataSource>
        <asp:GridView ID="GroupList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataGroups" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GroupList_SelectedIndexChanged">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="GROUPS" HeaderText="GROUPS" SortExpression="GROUPS" />
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
        <asp:Button ID="CreateGroupButton" runat="server" Text="Create New Group" OnClick="CreateGroupButton_Click" />
        <asp:Label ID="WarningCreateGroup" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    </fieldset>
</asp:Content>
