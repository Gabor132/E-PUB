<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="albums.aspx.cs" Inherits="E_Pub.albums" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <legend>
                <asp:Label ID="AlbumLabel" runat="server" Text=""></asp:Label>
            </legend>
            <div id="picturesList"></div>
        </fieldset>
        <fieldset>
            <legend>Add picture</legend>
            <asp:FileUpload ID="PictureUpload" runat="server" Height="22px" />
            <asp:Button ID="UploadButton" runat="server" Text="Upload" OnClick="UploadButton_Click" />
            <asp:Label ID="UploadLabel" runat="server" Text=""></asp:Label>
            <asp:Label ID="WarningUpdate" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        </fieldset>
    </div>
    <script type="text/javascript">
        var username = document.getElementById("ContentPlaceHolder2_currentUserLink").textContent;
        var isAdmin;
        window.onload = function () {

            PageMethods.isAdmin(username, function (result) {
                if (result === true) {
                    isAdmin = true;
                }
                getPictures();
            }, function (result) {
                alert("FAILED TO FIND IF ADMIN: " + result);
            });
        };

        function getPictures() {
            PageMethods.getPictures(getUrlVars()["albumId"], function (result) {
                appendPictures(result);
            }, function (result) {
                alert("FAILURE: " + result);
            });
        }

        function appendPictures(pictures) {
            var list = document.getElementById("picturesList");
            for (var i = 0; i < pictures.length; i++) {
                var picture = pictures[i].split("|");
                var item = document.createElement("a");
                item.href = "pictures.aspx?pictureId=" + picture[0];
                var itemI = document.createElement("img");
                var itemN = document.createElement("p");
                var deletePictureButton = document.createElement("button");
                deletePictureButton.id = "button" + picture[0];
                deletePictureButton.textContent = "Delete";
                deletePictureButton.onclick = function () {
                    deletePicture(this);
                };
                item.className = "pictureItem";
                var type = picture[1].split(".")[1];
                itemI.id = "pictureItem" + i;
                itemI.src = "";
                itemN.textContent = picture[1].split(".")[0];
                item.appendChild(itemI);
                item.appendChild(itemN);
                if (isAdmin) {
                    item.appendChild(deletePictureButton);
                }
                list.appendChild(item);
                document.getElementById("pictureItem" + i).src = "data:image/"+type+";base64," + picture[2];
            }
        }

        function deletePicture(event) {
            event = event || window.event;
            var id = event.id.split("button")[1];
            PageMethods.deletePicture(id, function (result) {

            }, function (result) {
                alert("FAILED TO GET PICTURE OWNER");
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
