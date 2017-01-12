using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class albums : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["user"] == null)
            {
                return;
            }
            if (IsPostBack)
            {
                return;
            }
            setupAlbum();
        }

        public void setupAlbum()
        {
            Album currentAlbum;
            currentAlbum = Album.findAlbum(int.Parse((Request.QueryString["albumId"])));
            AlbumLabel.Text = currentAlbum.ALBUM_NAME;
            Session["currentAlbum"] = currentAlbum;
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            Album album = Album.findAlbum(int.Parse(Request.QueryString["albumId"]));
            if(album.USER_ID != user.ID)
            {
                WarningUpdate.Text = "YOU CAN ONLY UPLOAD PHOTOS ON YOUR OWN ALBUM!";
                WarningUpdate.Visible = true;
                return;
            }
            Boolean isFileOK = false;
            String path = Server.MapPath("~/UploadedImages/");
            if (PictureUpload.HasFile)
            {
                String fileExtension =
                    System.IO.Path.GetExtension(PictureUpload.FileName).ToLower();
                String[] allowedExtensions =
                    {".gif", ".png", ".jpeg", ".jpg"};
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        isFileOK = true;
                    }
                }
            }

            if (isFileOK)
            {
                try
                {
                    if (Session["currentAlbum"] == null)
                    {
                        setupAlbum();
                    }
                    PictureUpload.PostedFile.SaveAs(path
                        + PictureUpload.FileName);
                    byte[] chunk = File.ReadAllBytes(path + PictureUpload.FileName);
                    File.Delete(path + PictureUpload.FileName);
                    UploadLabel.Text = "File uploaded!";
                    Album currentAlbum = (Album) Session["currentAlbum"];
                    Pictures newPicture = new Pictures(chunk, currentAlbum.ID, PictureUpload.FileName);
                    newPicture.insert();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    UploadLabel.Text = "File could not be uploaded.";
                }
            }
            else
            {
                UploadLabel.Text = "Cannot accept files of this type.";
            }
        }

        [WebMethod]
        public static List<String> getPictures(int albumId)
        {
            List<Pictures> pictures = Pictures.findAllPictures(albumId);
            return picturesToString(pictures);
        }

        [WebMethod]
        public static Boolean isAdmin(String username)
        {
            username = username.Split('-')[0].Split(' ')[0];
            User user = E_Pub.User.findUser(username);
            return user != null && user.USER_TYPE == TypeUser.ADMIN;
        }

        private static List<String> picturesToString(List<Pictures> pictures)
        {
            List<String> listS = new List<String>();
            foreach(Pictures p in pictures)
            {
                listS.Add(p.ToString());
            }
            return listS;
        }

        [WebMethod]
        public static void deletePicture(int pictureId)
        {
            Pictures pic = Pictures.findPicture(pictureId);
            List<Comments> comments = Comments.findPictureComments(pictureId);
            foreach (Comments c in comments)
            {
                c.delete();
            }
            pic.delete();
        }
    }
}