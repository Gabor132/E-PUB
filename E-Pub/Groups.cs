using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace E_Pub
{
    public class Group : Entity
    {
        private int id;
        private String groupName;
        private List<User> members;

        public Group(String group_name, List<User> members)
        {
            this.id = -1;
            this.groupName = group_name;
            this.members = members;
        }

        public Group(int id, String group_name, List<User> members)
        {
            this.id = id;
            this.groupName = group_name;
            this.members = members;
        }

        public bool exists()
        {
            bool value = false;
            string txt = string.Format("select * from GROUPS where id = {0} or group_name = '{1}'", this.id, this.groupName);
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
            string insertGroup = "insert into GROUPS(group_name) VALUES(@groupName);";
            string insertUserGroups = "insert into USER_GROUPS(user_id, group_id) VALUES(@uid, @gid);";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(insertGroup, conn);
            cmd.Parameters.Add(new SqlParameter("@groupName", TypeCode.String));
            cmd.Parameters["@groupName"].Value = this.groupName;
            cmd.ExecuteNonQuery();
            String getGroup = string.Format("select id from GROUPS where group_name = '{0}'", groupName);
            SqlCommand cmd2 = new SqlCommand(getGroup, conn);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            reader2.Read();
            this.id = reader2.GetInt32(0);
            reader2.Close();
            IEnumerator<User> enumerator = members.GetEnumerator();
            int i = 0;
            String fullInsert = "";
            while (enumerator.MoveNext())
            {
                String insert = insertUserGroups.Replace("uid", "uid" + i);
                fullInsert += insert;
                i++;
            }
            cmd = new SqlCommand(fullInsert, conn);
            enumerator.Reset();
            i = 0;
            while (enumerator.MoveNext())
            {
                User currentUser = enumerator.Current;
                cmd.Parameters.Add(new SqlParameter("@uid"+i, TypeCode.Int32));
                cmd.Parameters["@uid"+i].Value = currentUser.ID;
                i++;
            }
            cmd.Parameters.Add(new SqlParameter("@gid", TypeCode.Int32));
            cmd.Parameters["@gid"].Value = this.id;
            cmd.ExecuteNonQuery();
            conn.Close();
            return this;
        }

        static public Group findGroup(int id)
        {
            String findQuery = string.Format("SELECT * FROM GROUPS WHERE id = {0}", id);
            String findMembers = string.Format("SELECT user_id FROM USER_GROUPS group_id = {0}", id);
            return find(findQuery, findMembers);
        }

        static public Group findGroup(String groupName)
        {
            String findQuery = string.Format("SELECT * FROM GROUPS WHERE group_name = '{0}'", groupName);
            String findMembers = string.Format("SELECT ug.user_id FROM USER_GROUPS ug JOIN GROUPS g on (g.id = ug.group_id) WHERE g.group_name = '{0}'", groupName);
            return find(findQuery, findMembers);
        }

        static private Group find(String findQuery, String findMembers)
        {
            Group group = null;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(findMembers, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> members = new List<User>();
            if (reader.HasRows)
            {
                while (reader.NextResult())
                {
                    members.Add(User.findUser(reader.GetInt32(0)));
                }
            }
            reader.Close();
            cmd = new SqlCommand(findQuery, conn);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                group = new Group(reader.GetInt32(0), reader.GetString(1), members);
            }
            reader.Close();
            conn.Close();
            return group;
        }

        public int ID
        {
            get { return id; }
            set { this.id = value; }
        }

        public String GROUP_NAME
        {
            get { return groupName; }
            set { this.groupName = value; }
        }

        public List<User> MEMBERS
        {
            get { return members; }
            set { this.members = value; }
        }
    }
}