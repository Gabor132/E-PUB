using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace E_Pub
{

    public enum TypeResponse
    {
        NONE, PENDING, ACCEPTED, REFUSED
    }

    public class FriendRequest : Entity
    {
        private int id, sender_id, receiver_id;
        private TypeResponse response;

        public FriendRequest(int id, int sender_id, int receiver_id, TypeResponse response)
        {
            this.id = id;
            this.sender_id = sender_id;
            this.receiver_id = receiver_id;
            this.response = response;
        }

        public FriendRequest(int sender_id, int receiver_id, TypeResponse response)
        {
            this.id = -1;
            this.sender_id = sender_id;
            this.receiver_id = receiver_id;
            this.response = response;
        }


        public bool exists()
        {
            bool value = false;
            string txt = string.Format("select * from FRIEND_REQUESTS where id = {0} or (sender_id = {1} and receiver_id = {2}) or (sender_id = {2} and receiver_id = {1})", this.id, this.sender_id, this.receiver_id);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                this.id = reader.GetInt32(0);
                this.sender_id = reader.GetInt32(1);
                this.receiver_id = reader.GetInt32(2);
                this.response = (TypeResponse) Enum.Parse(typeof(TypeResponse),reader.GetString(3));
                value = true;
            }
            reader.Close();
            conn.Close();
            return value;
        }

        public object insert()
        {
            string txt = "insert into FRIEND_REQUESTS(sender_id,receiver_id,response) VALUES(@sender,@receiver,@response)";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@sender", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@receiver", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@response", TypeCode.String));
            cmd.Parameters["@sender"].Value = this.sender_id;
            cmd.Parameters["@receiver"].Value = this.receiver_id;
            cmd.Parameters["@response"].Value = this.response.ToString();
            cmd.ExecuteNonQuery();
            String txt2 = string.Format("select id from FRIEND_REQUESTS where sender_id = {0} and receiver_id = {1}", sender_id, receiver_id);
            SqlCommand cmd2 = new SqlCommand(txt2, conn);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            reader2.Read();
            this.id = reader2.GetInt32(0);
            reader2.Close();
            conn.Close();
            return this;
        }

        public void delete()
        {
            string txt = "delete from FRIEND_REQUESTS where sender_id = @sender and receiver_id = @receiver";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@sender", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@receiver", TypeCode.Int32));
            cmd.Parameters["@sender"].Value = this.sender_id;
            cmd.Parameters["@receiver"].Value = this.receiver_id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void changeRequest(TypeResponse response)
        {
            if (!exists())
            {
                insert();
            }
            string txt = "update FRIEND_REQUESTS SET response = @response where id = @id";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@response", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
            cmd.Parameters["@response"].Value = response.ToString();
            cmd.Parameters["@id"].Value = this.id;
            cmd.ExecuteNonQuery();
            this.response = response;
            if(response == TypeResponse.ACCEPTED)
            {
                createPairGroup();
            }
            conn.Close();
        }

        private void createPairGroup()
        {
            List<User> groupMembers = new List<User>();
            groupMembers.Add(User.findUser(sender_id));
            groupMembers.Add(User.findUser(receiver_id));
            Group group = new Group(groupMembers[0].USERNAME + "-" + groupMembers[1].USERNAME, groupMembers);
            group.insert();
        }

        static public object find(int sender_id, int receiver_id)
        {
            string txt = string.Format("select * from FRIEND_REQUESTS where sender_id={0} and receiver_id={1}", sender_id, receiver_id);
            return findQuery(txt);
        }

        static private object findQuery(string txt)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            FriendRequest fr = null;
            if (reader.HasRows)
            {
                reader.Read();
                fr = new FriendRequest(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), (TypeResponse)Enum.Parse(typeof(TypeResponse), reader.GetString(3)));
            }
            reader.Close();
            conn.Close();
            return fr;
        }

        public int SENDER_ID
        {
            get { return sender_id; }
        }

        public int RECEIVER_ID
        {
            get { return receiver_id; }
        }

        public TypeResponse RESPONSE
        {
            get { return response; }
        }
    }
}