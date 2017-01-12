using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class main : System.Web.UI.Page
    {
        Boolean isOnOwnPage = false;
        Boolean isOnOtherPage = false;
        Boolean isOnEmptyPage = false;
        Boolean isOnInvalidPage = false;
        User currentUser;
        User profileUser;
        SiteMaster master;

        protected void Page_Load(object sender, EventArgs e)
        {
            master = (SiteMaster) this.Master;
            if (Session["user"] == null)
                return;
            setupMainPageMarkers();
            if (IsPostBack)
                return;
            setupFriendRequestButton();
            setupProfileTypeList();
        }

        //SETUP//

        protected void setupProfileTypeList()
        {
            if (isOnOwnPage)
            {
                ListItemCollection collection = ProfileTypeList.Items;
                IEnumerator e = collection.GetEnumerator();
                while (e.MoveNext())
                {
                    ListItem j = (ListItem)e.Current;
                    j.Selected = false;
                }
                TypeProfile typeP = currentUser.PROFILE_TYPE;
                ListItem i = collection.FindByValue(typeP.ToString());
                i.Selected = true;
            }
            else
            {
                ProfileTypeList.Visible = false;
            }
        }

        public void setupFriendRequestButton()
        {
            if (isOnEmptyPage)
            {
                friendRequestButton.Visible = false;
                acceptFriendRequestButton.Visible = false;
                deleteFriendRequestButton.Visible = false;
                friendRequestLabel.Visible = false;
                return;
            }
            else if (isOnInvalidPage)
            {
                profileUserLabel.Text = "NO USER";
                friendRequestButton.Visible = false;
                acceptFriendRequestButton.Visible = false;
                deleteFriendRequestButton.Visible = false;
                friendRequestLabel.Visible = false;
                return;
            }else
            {
                if(profileUser == null)
                {
                    profileUserLabel.Text = "GUEST";
                }else
                {
                    profileUserLabel.Text = profileUser.USERNAME;
                }
            }

            setupForPublic();


            if (isOnOtherPage && currentUser.USER_TYPE != TypeUser.GUEST)
            {
                FriendRequest fr = new FriendRequest(profileUser.ID, currentUser.ID, TypeResponse.NONE);
                if (fr.exists())
                {
                    if (fr.RESPONSE == TypeResponse.PENDING)
                    {
                        if (fr.SENDER_ID == profileUser.ID)
                        {
                            friendRequestButton.Visible = false;
                            friendRequestLabel.Visible = false;
                        }
                        else if (fr.SENDER_ID == currentUser.ID)
                        {
                            friendRequestButton.Visible = false;
                            acceptFriendRequestButton.Visible = false;
                            deleteFriendRequestButton.Visible = false;
                        }
                    }
                    else
                    {
                        profileUserLabel.ForeColor = System.Drawing.Color.Blue;
                        friendRequestButton.Visible = false;
                        acceptFriendRequestButton.Visible = false;
                        deleteFriendRequestButton.Visible = false;
                        friendRequestLabel.Visible = false;
                    }
                }
                else
                {
                    acceptFriendRequestButton.Visible = false;
                    deleteFriendRequestButton.Visible = false;
                    friendRequestLabel.Visible = false;
                }
            }
            else
            {
                profileUserLabel.ForeColor = System.Drawing.Color.Green;
                friendRequestButton.Visible = false;
                acceptFriendRequestButton.Visible = false;
                deleteFriendRequestButton.Visible = false;
                friendRequestLabel.Visible = false;
            }
        }

        protected void setupMainPageMarkers()
        {
            currentUser = (User)Session["user"];
            String profileUsername = Request.QueryString["profile"];
            if (profileUsername == null)
            {
                if (currentUser.USER_TYPE != TypeUser.GUEST && currentUser.USER_TYPE != TypeUser.ADMIN)
                {
                    isOnEmptyPage = true;
                }
            }
            else if (profileUsername.Equals(currentUser.USERNAME))
            {
                isOnOwnPage = true;
                profileUser = currentUser;
            }
            else
            {
                profileUser = (User)E_Pub.User.findUser(profileUsername);
                if (profileUser == null)
                {
                    isOnInvalidPage = true;
                }
                else
                {
                    isOnOtherPage = true;
                }
            }
        }

        public void setupForPublic()
        {
            if (profileUser == null)
            {
                ProfileTypeList.Visible = false;
                return;
            }
            if (isOnOwnPage || profileUser.PROFILE_TYPE == TypeProfile.PUBLIC || currentUser.USER_TYPE == TypeUser.ADMIN)
            {
                profileEmailLabel.Text = "Email: " + profileUser.EMAIL;
            }
            else
            {
                friendList.Visible = false;
                AlbumView.Visible = false;
            }
        }

        //EVENT HANDLERS//

        protected void friendRequestButton_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            User profile = (User)E_Pub.User.findUser(profileUserLabel.Text);
            FriendRequest friendRequest = new FriendRequest(user.ID,profile.ID,TypeResponse.PENDING);
            friendRequest = (FriendRequest) friendRequest.insert();
            Response.Redirect(Request.RawUrl);
        }

        protected void acceptFriendRequestButton_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            User profile = (User)E_Pub.User.findUser(profileUserLabel.Text);
            FriendRequest friendRequest = (FriendRequest)E_Pub.FriendRequest.find(profile.ID, user.ID);
            friendRequest.changeRequest(TypeResponse.ACCEPTED);
            Response.Redirect(Request.RawUrl);
        }

        protected void deleteFriendRequestButton_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            User profile = (User)E_Pub.User.findUser(profileUserLabel.Text);
            FriendRequest friendRequest = (FriendRequest)E_Pub.FriendRequest.find(profile.ID, user.ID);
            friendRequest.delete();
            Response.Redirect(Request.RawUrl);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("main.aspx?profile={0}", friendList.SelectedRow.Cells[1].Text), true);
        }

        protected void ProfileTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem i = ProfileTypeList.SelectedItem;
            TypeProfile typeP = (TypeProfile)Enum.Parse(typeof(TypeProfile), i.Value);
            currentUser.PROFILE_TYPE = typeP;
            currentUser.update();
        }

        protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e)
        {
            String albumName = AlbumView.SelectedRow.Cells[1].Text;
            int userId = profileUser.ID;
            Album album = Album.findAlbum(userId, albumName);
            Response.Redirect(string.Format("albums.aspx?albumId={0}",album.ID), true);
        }

        protected void CreateAlbumButton_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            if(user.USER_TYPE == TypeUser.GUEST)
            {
                WarningCreateAlbum.Text = "CAN'T CREATE ALBUM UNLESS YOU ARE REGISTERED!";
                WarningCreateAlbum.Visible = true;
                return;
            }
            String albumName = CreateAlbumText.Text;
            Album newAlbum = new Album(albumName, currentUser.ID);
            newAlbum = (Album)newAlbum.insert();
            Response.Redirect(string.Format("albums.aspx?albumId={0}", newAlbum.ID), true);
        }
    }
}