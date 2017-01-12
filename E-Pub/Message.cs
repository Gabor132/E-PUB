using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace E_Pub
{
    public class Message : Entity
    {
        private int id;
        private int senderId;
        private int groupId;
        private String content;
        private DateTime datePost;

        public Message(int id, int senderId, int groupId, String content)
        {
            this.id = id;
            this.senderId = senderId;
            this.groupId = groupId;
            this.content = content;
            this.datePost = DateTime.Now;
        }

        public Message(int senderId, int groupId, String content)
        {
            this.id = -1;
            this.senderId = senderId;
            this.groupId = groupId;
            this.content = content;
            this.datePost = DateTime.Now;
        }

        public Message(int id, int senderId, int groupId, String content, DateTime datePost)
        {
            this.id = id;
            this.senderId = senderId;
            this.groupId = groupId;
            this.content = content;
            this.datePost = datePost;
        }

        public bool exists()
        {
            bool value = false;
            string txt = string.Format("select * from MESSAGES where id = {0}", this.id);
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
            string insertMessage = "insert into MESSAGES(sender_id, group_id, content, date_post) VALUES(@senderId, @groupId, @content, @datePost);";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(insertMessage, conn);
            cmd.Parameters.Add(new SqlParameter("@senderId", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@groupId", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@content", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@datePost", TypeCode.DateTime));
            cmd.Parameters["@senderId"].Value = this.senderId;
            cmd.Parameters["@groupId"].Value = this.groupId;
            cmd.Parameters["@content"].Value = this.content;
            cmd.Parameters["@datePost"].Value = this.datePost;
            cmd.ExecuteNonQuery();
            conn.Close();
            return this;
        }

        public void delete()
        {
            string txt = "delete from MESSAGES where id = @id";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
            cmd.Parameters["@id"].Value = this.id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        static public List<Message> findMessage(int groupId)
        {
            String query = string.Format("SELECT * FROM MESSAGES WHERE group_id = {0} ORDER BY date_post", groupId);
            return find(query);
        }

        static public List<Message> findMessage(int groupId, DateTime minDate)
        {
            String query = string.Format("SELECT * FROM MESSAGES WHERE group_id = {0} and date_post > CAST('{1}' as datetime) ORDER BY date_post", groupId, minDate.ToString());
            return find(query);
        }

        static public List<Message> findMessage(int groupId, int lastMessageId)
        {
            String query = string.Format("SELECT * FROM MESSAGES WHERE group_id = {0} and id > {1} ORDER BY date_post", groupId, lastMessageId);
            return find(query);
        }

        static public Message findOneMessage(int messageId)
        {
            String query = string.Format("SELECT * FROM MESSAGES WHERE id = {0}", messageId);
            return findMessage(query);
        }

        static private Message findMessage(String findMessage)
        {
            Message message = null;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(findMessage, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> members = new List<User>();
            if (reader.HasRows)
            {
                reader.Read();
                message = new Message(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetDateTime(4));
            }
            reader.Close();
            conn.Close();
            return message;
        }

        static private List<Message> find(String findMessage)
        {
            List<Message> messages = new List<Message>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(findMessage, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> members = new List<User>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    messages.Add(new Message(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2),reader.GetString(3), reader.GetDateTime(4)));
                }
            }
            reader.Close();
            conn.Close();
            return messages;
        }

        override
        public String ToString()
        {
            return id + "|" + User.findUser(senderId).USERNAME + "|" + content + "|" + datePost.ToString();
        }

        public int ID
        {
            get { return id; }
            set { this.id = value; }
        }
        public int SENDER_ID
        {
            get { return senderId; }
            set { this.senderId = value; }
        }
        public int GROUP_ID
        {
            get { return groupId; }
            set { this.groupId = value; }
        }
        public String CONTENT
        {
            get { return content; }
            set { this.content = value; }
        }
        public DateTime DATE_POST
        {
            get { return datePost; }
            set { this.datePost = value; }
        }
    }
}