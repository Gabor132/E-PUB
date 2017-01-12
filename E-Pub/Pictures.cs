using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace E_Pub
{
    public class Pictures : Entity
    {
        int id;
        byte[] chunk;
        int albumId;
        string name;

        public Pictures(byte[] chunk, int albumId, string name)
        {
            this.id = -1;
            this.chunk = chunk;
            this.albumId = albumId;
            this.name = name;
        }

        public Pictures(int id, byte[] chunk, int albumId, string name)
        {
            this.id = id;
            this.chunk = chunk;
            this.albumId = albumId;
            this.name = name;
        }

        public static Pictures findPicture(int pictureId)
        {
            String query = string.Format("SELECT * FROM PICTURES WHERE id = {0}", pictureId);
            return find(query);
        }

        public static List<Pictures> findAllPictures(int albumId)
        {
            String query = string.Format("SELECT * FROM PICTURES WHERE album_id = {0}", albumId);
            return findAll(query);
        }

        private static List<Pictures> findAll(String findPicture)
        {
            List<Pictures> pictures = new List<Pictures>();
            Pictures picture = null;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(findPicture, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                long sizeBuffer = reader.GetBytes(1, 0, null, 0, 0);
                byte[] buffer = new byte[sizeBuffer];
                reader.GetBytes(1, 0, buffer, 0, (int)sizeBuffer);
                picture = new Pictures(reader.GetInt32(0), buffer, reader.GetInt32(2), reader.GetString(3));
                pictures.Add(picture);
            }
            reader.Close();
            conn.Close();
            return pictures;
        }

        private static Pictures find(String findPicture)
        {
            Pictures picture = null;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(findPicture, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                long sizeBuffer = reader.GetBytes(1, 0, null, 0, 0);
                byte[] buffer = new byte[sizeBuffer];
                reader.GetBytes(1, 0, buffer, 0, (int)sizeBuffer);
                picture = new Pictures(reader.GetInt32(0), buffer, reader.GetInt32(2), reader.GetString(3));
            }
            reader.Close();
            conn.Close();
            return picture;
        }

        public bool exists()
        {
            bool value = false;
            string txt = string.Format("select * from PICTURES where id = {0}", this.id);
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
            string insertPicture = "insert into PICTURES(file_chunk, album_id, name) VALUES(@fileChunk, @albumId, @name);";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(insertPicture, conn);
            cmd.Parameters.Add(new SqlParameter("@fileChunk", TypeCode.SByte));
            cmd.Parameters.Add(new SqlParameter("@albumId", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@name", TypeCode.String));
            cmd.Parameters["@fileChunk"].Value = this.chunk;
            cmd.Parameters["@albumId"].Value = this.albumId;
            cmd.Parameters["@name"].Value = this.name;
            cmd.ExecuteNonQuery();
            conn.Close();
            return this;
        }

        public void delete()
        {
            string txt = "delete from PICTURES where id = @id";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
            cmd.Parameters["@id"].Value = this.id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        override
        public String ToString()
        {
            return id+ "|" + name + "|" + Convert.ToBase64String(chunk);
        }

        public int ID
        {
            get { return id; }
            set { this.id = value; }
        }
        public byte[] FILE_CHUNK
        {
            get { return chunk; }
            set { this.chunk = value; }
        }
        public int ALBUM_ID
        {
            get { return albumId; }
            set { this.albumId = value; }
        }
        public string NAME
        {
            get { return name; }
            set { this.name = value; }
        }

    }
}