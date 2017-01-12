<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="pictures.aspx.cs" Inherits="E_Pub.pictures" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="picture"></div>
    <fieldset>
    <div id="comments">
    </div>
        <legend>Comments</legend>
        <asp:TextBox ID="CommentBox" runat="server" Width="316px"></asp:TextBox>
        <asp:Button ID="CommentButton" runat="server" Text="Send" OnClick="CommentButton_Click" />
        <asp:Label ID="WarningComment" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    </fieldset>

    <script type="text/javascript">
        window.onload = getPicture();
        var username = document.getElementById("ContentPlaceHolder2_currentUserLink").textContent;
        var pictureOwner;
        var isAdmin;
        PageMethods.isAdmin(username, function (result) {
            if (result === true) {
                isAdmin = true;
            }
        }, function (result) {
            alert("FAILED TO FIND IF ADMIN: " + result);
        });
        function getPicture() {
            PageMethods.getPicture(getUrlVars()["pictureId"], function (result) {
                appendPicture(result);
                PageMethods.getPictureOwner(getUrlVars()["pictureId"], function(result){
                    pictureOwner = result;
                }, function(result){
                    alert("FAILED TO GET PICTURE OWNER"); 
                });
                getComments();
            }, function (result) {
                alert("FAILURE: " + result);
            });
        }

        function appendPicture(pictures) {
            var list = document.getElementById("picture");
            for (var i = 0; i < pictures.length; i++) {
                var picture = pictures[i].split("|");
                var item = document.createElement("div");
                item.href = "pictures.aspx?pictureId=" + picture[0];
                var itemI = document.createElement("img");
                var itemN = document.createElement("h1");
                item.className = "mainPicture";
                var type = picture[1].split(".")[1];
                itemI.id = "pictureItem" + i;
                itemI.src = "";
                itemN.textContent = picture[1].split(".")[0];
                item.appendChild(itemI);
                item.appendChild(itemN);
                list.appendChild(item);
                document.getElementById("pictureItem" + i).src = "data:image/" + type + ";base64," + picture[2];
            }
        }

        function getComments() {
            var cList = document.getElementById("comments");
            while(cList.firstChild){
                cList.removeChild(cList.firstChild);
            }
            PageMethods.getComments(getUrlVars()["pictureId"], function (result) {
                appendComments(result);
            }, function (result) {
                alert("FAILED TO LOAD COMMENTS!");
            });
        }

        function appendComments(comments) {
            var list = document.getElementById("comments");
            for (var i = 0; i < comments.length; i++) {
                var comment = comments[i].split("|");
                var item = document.createElement("div");
                var itemC = document.createElement("p");
                var itemT = document.createElement("time");
                var itemA = document.createElement("button");
                var itemN = document.createElement("button");
                item.className = "comment";
                itemC.className = "commentContent";
                itemT.className = "commentTime";
                itemC.textContent = comment[1] + ": " + comment[2];
                itemT.textContent = comment[4];
                itemA.onclick = function () { approveComment(this); };
                itemA.textContent = "APPROVE";
                itemA.id = "button" + comment[0];
                itemN.onclick = function () { negateComment(this); };
                itemN.textContent = "DELETE";
                itemN.id = "button" + comment[0];
                item.appendChild(itemC);
                item.appendChild(itemT);
                if (comment[5] === "PENDING") {
                    if (username === pictureOwner) {
                        item.appendChild(document.createElement("br"));
                        item.appendChild(itemA);
                        item.appendChild(itemN);
                    } else if (username === comment[1]) {
                        item.appendChild(document.createElement("br"));
                        item.appendChild(itemN);
                    } else if (isAdmin) {
                        item.appendChild(document.createElement("br"));
                        item.appendChild(itemN);
                    }
                } else if (isAdmin) {
                    item.appendChild(document.createElement("br"));
                    item.appendChild(itemN);
                }
                if (username === pictureOwner || comment[5] === "APPROVED"
                    || username === comment[1] || isAdmin) {
                    list.appendChild(item);
                }
            }
        }

        function approveComment(event) {
            event = event || window.event;
            var id = event.id.split("button")[1];
            PageMethods.approveComment(parseInt(id), function (result) {
                getComments();
            }, function (result) {
                alert("FAILURE: " + result);
            });
        }

        function negateComment(event) {
            event = event || window.event;
            var id = event.id.split("button")[1];
            PageMethods.negateComment(parseInt(id), function (result) {
                getComments();
            }, function (result) {
                alert("FAILURE: " + result);
            });
        }

        function getUrlVars() {
            var vars = {};
            var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                vars[key] = value;
            });
            return vars;
        }
    </script>
</asp:Content>
