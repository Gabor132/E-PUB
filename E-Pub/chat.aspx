<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="chat.aspx.cs" Inherits="E_Pub.chat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <legend>Chat</legend>
        <asp:SqlDataSource ID="SqlDataMembers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT u.username FROM USERS u JOIN USER_GROUPS ug ON (u.id = ug.user_id) JOIN GROUPS g 
    ON (g.id = ug.group_id) WHERE g.group_name = @groupName;">
            <SelectParameters>
                <asp:SessionParameter Name="groupName" SessionField="groupName" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Label ID="MembersLabel" runat="server" Text="Members:"></asp:Label>
        <br/>
        <asp:ListBox ID="MembersList" runat="server" DataSourceID="SqlDataMembers" DataTextField="username" DataValueField="username"></asp:ListBox>
        <br/>
        <div id="messageList"></div>
        <script type="text/javascript">
            window.onload = function () {
                PageMethods.isAdmin(username, function (result) {
                    if (result === true) {
                        isAdmin = true;
                    }
                    getMessages();
                }, function (result) {
                    alert("FAILED TO FIND IF ADMIN: " + result);
                });
            }
            var lastMessageId = -1;
            var username = document.getElementById("ContentPlaceHolder2_currentUserLink").textContent;
            var isAdmin;
            function appendMessages(messages) {
                var list = document.getElementById("messageList");
                for (var i = 0; i < messages.length; i++) {
                    var message = messages[i].split("|");
                    var item = document.createElement("div");
                    var deleteButton = document.createElement("button");
                    deleteButton.id = "delete" + message[0];
                    deleteButton.textContent = "Delete";
                    deleteButton.onclick = function () { deleteMessage(this); }
                    item.className = "messageItem";
                    var itemC = document.createElement("p");
                    itemC.textContent = message[1] + ": " + message[2];
                    var itemD = document.createElement("time");
                    itemD.className = "time";
                    itemD.textContent = "   - " + message[3];
                    if (message[1] == username) {
                        item.className = "messageItemOwner";
                        itemD.className = "timeOwner";
                    }
                    item.appendChild(itemC);
                    item.appendChild(itemD);
                    if (isAdmin) {
                        item.appendChild(document.createElement("br"));
                        item.appendChild(deleteButton);
                    }
                    list.appendChild(item);
                }
                if (messages.length > 0) {
                    return messages[messages.length - 1].split("|")[0];
                }
                return lastMessageId;
            }

            function deleteMessage(event) {
                event = event || window.event;
                var id = event.id.split("delete")[1];
                PageMethods.deleteMessage(parseInt(id), function (result) {
                    
                }, function (result) {
                    alert("FAILED TO DELETE MESSAGE " + result);
                });
            }

            function getMessages() {
                PageMethods.getAllMessages(getUrlVars()["groupId"], function (result) {
                    lastMessageId = appendMessages(result);
                }, function (result) {
                    alert("FAILURE: " + result);
                });

                setInterval(function () {
                    PageMethods.getNewMessages(getUrlVars()["groupId"], lastMessageId, function (result) {
                        lastMessageId = appendMessages(result);
                    }, function (result) {
                        alert("FAILURE NEW: " + result);
                    });
                }, 5000);
            }

            function getUrlVars() {
                var vars = {};
                var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }
        </script>
        
        <asp:Label ID="MessageLabel" runat="server" Text="Message: "></asp:Label>
        <asp:TextBox ID="MessageBox" runat="server"></asp:TextBox>
        <asp:Button ID="SendButton" runat="server" Text="Send" OnClick="SendButton_Click" />
    </fieldset>
</asp:Content>
