using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace E_Pub
{
    public class Comments : Entity
    {

        public enum TypeComment
        {
            PENDING, APPROVED
        }

        int id;
        int userId;
        String content;
        DateTime dateCreation;
        TypeComment status;
        int pictureId;
        
        public Comments(int id, int userId, String content, DateTime dateCreation, TypeComment status, int pictureId)
        {
            this.id = id;
            this.userId = userId;
            this.content = content;
            this.dateCreation = dateCreation;
            this.status = status;
            this.pictureId = pictureId;
        }

        public Comments(int userId, String content, int pictureId)
        {
            this.id = -1;
            this.userId = userId;
            this.content = content;
            this.dateCreation = DateTime.Now;
            this.status = TypeComment.PENDING;
            this.pictureId = pictureId;
        }
        
        public bool exists()
        {
            bool value = false;
            string txt = string.Format("select * from COMMENTS where id = {0}", this.id);
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
            string insertComment = "insert into COMMENTS(user_id, content, date_creation, approved, picture_id) VALUES(@userId, @content, @dateCreation, @approved, @pictureId);";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(insertComment, conn);
            cmd.Parameters.Add(new SqlParameter("@content", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@userId", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@dateCreation", TypeCode.DateTime));
            cmd.Parameters.Add(new SqlParameter("@approved", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@pictureId", TypeCode.Int32));
            cmd.Parameters["@content"].Value = this.content;
            cmd.Parameters["@userId"].Value = this.userId;
            cmd.Parameters["@dateCreation"].Value = this.dateCreation;
            cmd.Parameters["@approved"].Value = this.status.ToString();
            cmd.Parameters["@pictureId"].Value = this.pictureId;
            cmd.ExecuteNonQuery();
            conn.Close();
            return this;
        }

        public void delete()
        {
            string txt = "delete from Comments where id = @id";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
            cmd.Parameters["@id"].Value = this.id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void changeStatus(TypeComment approve)
        {
            if (!exists())
            {
                insert();
            }
            string txt = "update COMMENTS SET approved = @approved where id = @id";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@approved", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
            cmd.Parameters["@approved"].Value = approve.ToString();
            cmd.Parameters["@id"].Value = this.id;
            cmd.ExecuteNonQuery();
            this.status = approve;
            conn.Close();
        }

        static public List<Comments> findPictureComments(int pictureId)
        {
            string txt = string.Format("select * from COMMENTS where picture_id={0} order by date_creation", pictureId);
            return findAllComments(txt);
        }

        static public Comments findComment(int commentId)
        {
            string txt = string.Format("SELECT * from COMMENTS WHERE id = {0}", commentId);
            return find(txt);
        }

        static private Comments find(String txt)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            Comments comment = null;
            if (reader.HasRows)
            {
                reader.Read();
                comment = new Comments(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3), (TypeComment)Enum.Parse(typeof(TypeComment), reader.GetString(4)), reader.GetInt32(5));
            }
            reader.Close();
            conn.Close();
            return comment;
        }

        static private List<Comments> findAllComments(string txt)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Comments> list = new List<Comments>();
            while (reader.Read())
            {
                Comments comment = new Comments(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3), (TypeComment)Enum.Parse(typeof(TypeComment), reader.GetString(4)),reader.GetInt32(5));
                list.Add(comment);
            }
            reader.Close();
            conn.Close();
            return list;
        }

        override
        public String ToString()
        {
            return id + "|" + User.findUser(userId).USERNAME + "|" + content + "|" + pictureId + "|" + dateCreation.ToString() + "|" + status.ToString();
        }

        public int ID
        {
            get { return id; }
            set { this.id = value; }
        }
        public int USER_ID
        {
            get { return userId; }
            set { this.userId = value; }
        }
        public String CONTENT
        {
            get { return content; }
            set { this.content = value; }
        }
        public DateTime DATE_CREATION
        {
            get { return dateCreation; }
            set { this.dateCreation = value; }
        }
        public TypeComment STATUS
        {
            get { return status; }
            set { this.status = value; }
        }
        public int PICTURE_ID
        {
            get { return pictureId; }
            set { this.pictureId = value; }
        }
    }
}