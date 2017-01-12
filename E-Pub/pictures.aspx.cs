using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class pictures : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                return;
            }
            if (IsPostBack)
            {
                return;
            }
        }

        [WebMethod]
        public static List<String> getPicture(int pictureId)
        {
            List<String> list = new List<String>();
            list.Add(Pictures.findPicture(pictureId).ToString());
            return list;
        }

        protected void CommentButton_Click(object sender, EventArgs e)
        {
            String content = CommentBox.Text;
            CommentBox.Text = "";
            User currentUser = (User)(Session["user"]);
            if(currentUser.USER_TYPE == TypeUser.GUEST)
            {
                WarningComment.Text = "CAN'T COMMENT IF YOU ARE A GUEST";
                WarningComment.Visible = true;
                return;
            }
            Comments comment = new Comments(currentUser.ID, content, int.Parse(Request.QueryString["pictureId"]));
            comment.insert();
        }

        [WebMethod]
        public static List<String> getComments(int pictureId)
        {
            List<Comments> listC = Comments.findPictureComments(pictureId);
            return commentsToString(listC);
        }

        private static List<String> commentsToString(List<Comments> comments)
        {
            List<String> list = new List<String>();
            foreach (Comments comment in comments)
            {
                list.Add(comment.ToString());
            }
            return list;
        }

        [WebMethod]
        public static void approveComment(int idComment)
        {
            Comments comment = Comments.findComment(idComment);
            comment.changeStatus(Comments.TypeComment.APPROVED);
        }

        [WebMethod]
        public static void negateComment(int idComment)
        {
            Comments comment = Comments.findComment(idComment);
            comment.delete();
        }

        [WebMethod]
        public static String getPictureOwner(int pictureId)
        {
            Pictures picture = Pictures.findPicture(pictureId);
            Album album = Album.findAlbum(picture.ALBUM_ID);
            User user = E_Pub.User.findUser(album.USER_ID);
            return user.USERNAME;
        }
        
        [WebMethod]
        public static Boolean isAdmin(String username)
        {
            username = username.Split('-')[0].Split(' ')[0];
            User user = E_Pub.User.findUser(username);
            return user!= null && user.USER_TYPE == TypeUser.ADMIN;
        }

    }
}