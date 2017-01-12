using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace E_Pub
{
    public class Album : Entity
    {
        int id;
        String albumName;
        int userId;
        DateTime dateCreation;

        public Album(int id, String albumName, int userId, DateTime dateCreation)
        {
            this.id = id;
            this.albumName = albumName;
            this.userId = userId;
            this.dateCreation = dateCreation;
        }

        public Album(String albumName, int userId, DateTime dateCreation)
        {
            this.id = -1;
            this.albumName = albumName;
            this.userId = userId;
            this.dateCreation = dateCreation;
        }
        
        public Album(String albumName, int userId)
        {
            this.id = -1;
            this.albumName = albumName;
            this.userId = userId;
            this.dateCreation = DateTime.Now;
        }

        public static Album findAlbum(int userId, String albumName)
        {
            String query = string.Format("SELECT * FROM ALBUMS WHERE user_id = {0} AND album_name = '{1}'", userId, albumName);
            return find(query);
        }

        public static Album findAlbum(int albumId)
        {
            String query = string.Format("SELECT * FROM ALBUMS WHERE id = {0}", albumId);
            return find(query);
        }

        private static Album find(String findAlbum)
        {
            Album album = null;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(findAlbum, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                album = new Album(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDateTime(3));
            }
            reader.Close();
            conn.Close();
            return album;
        }

        public bool exists()
        {
            bool value = false;
            string txt = string.Format("select * from ALBUMS where id = {0}", this.id);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                value = true;
            }
            reader.Close();
            conn.Close();
            return value;
        }

        public object insert()
        {
            string insertPicture = "insert into ALBUMS(album_name, user_id, date_creation) VALUES(@albumName, @userId, @dateCreation);";
            String query = string.Format("SELECT * FROM ALBUMS WHERE user_id = {0} AND album_name = '{1}'", userId, albumName);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(insertPicture, conn);
            cmd.Parameters.Add(new SqlParameter("@albumName", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@userId", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@dateCreation", TypeCode.DateTime));
            cmd.Parameters["@albumName"].Value = this.albumName;
            cmd.Parameters["@userId"].Value = this.userId;
            cmd.Parameters["@dateCreation"].Value = this.dateCreation;
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                this.id = reader.GetInt32(0);
            }
            reader.Close();
            conn.Close();
            return this;
        }

        public int ID
        {
            get { return id; }
            set { this.id = value; }
        }
        public String ALBUM_NAME
        {
            get { return albumName; }
            set { this.albumName = value; }
        }
        public int USER_ID
        {
            get { return userId; }
            set { this.userId = value; }
        }
        public DateTime DATE_CREATION
        {
            get { return dateCreation; }
            set { this.dateCreation = value; }
        }
    }
}