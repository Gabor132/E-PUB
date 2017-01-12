using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace E_Pub
{
    public enum TypeUser
    {
        NONE, ADMIN, USER, GUEST
    }

    public enum TypeProfile
    {
        PUBLIC, PRIVATE
    }

    public class User : Entity
    {
        private int id;
        private String username, password, email;
        private TypeUser typeU;
        private TypeProfile typeP;

        public User(int id, String username, String password, String email, TypeUser typeU, TypeProfile typeP)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.email = email;
            this.typeU = typeU;
            this.typeP = typeP;
        }

        public User(String username, String password, String email, TypeUser typeU, TypeProfile typeP)
        {
            this.id = -1;
            this.username = username;
            this.password = password;
            this.email = email;
            this.typeU = typeU;
            this.typeP = typeP;
        }

        public bool exists()
        {
            bool value = false;
            string txt = string.Format("select * from USERS where id = {0} or (username = '{1}' and password = '{2}')", this.id, this.username, this.password);
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
            if(this.typeU == TypeUser.GUEST)
            {
                return null;
            }
            string txt = "insert into USERS(username,password,email,u_type, p_type) VALUES(@username,@password,@email,@type,@typeP)";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@username", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@password", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@email", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@type", TypeCode.String));
            cmd.Parameters.Add(new SqlParameter("@typeP", TypeCode.String));
            cmd.Parameters["@username"].Value = this.username;
            cmd.Parameters["@password"].Value = this.password;
            cmd.Parameters["@email"].Value = this.email;
            cmd.Parameters["@type"].Value = this.typeU.ToString();
            cmd.Parameters["@typeP"].Value = this.typeP.ToString();
            cmd.ExecuteNonQuery();
            String txt2 = string.Format("select id from USERS where username = '{0}'", username);
            SqlCommand cmd2 = new SqlCommand(txt2, conn);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            reader2.Read();
            this.id = reader2.GetInt32(0);
            reader2.Close();
            conn.Close();
            return this;
        }

        /**
         * Updates only the profile type (momentarily)
         **/
        public object update()
        {
            if (this.typeU == TypeUser.GUEST)
            {
                return null;
            }
            string txt = "update USERS set p_type = @typeP where id = @id";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            cmd.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
            cmd.Parameters.Add(new SqlParameter("@typeP", TypeCode.String));
            cmd.Parameters["@id"].Value = this.id;
            cmd.Parameters["@typeP"].Value = this.typeP.ToString();
            cmd.ExecuteNonQuery();
            conn.Close();
            return this;
        }

        static public User findUser(int id)
        {
            string txt = string.Format("select * from USERS where id='{0}'", id);
            return find(txt);
        }

        static public User findUser(string username)
        {
            string txt = string.Format("select * from USERS where username='{0}'", username);
            return find(txt);
        }

        static public User findUser(string username,string password)
        {
            string txt = string.Format("select * from USERS where username='{0}' and password='{1}'", username,password);
            return find(txt);
        }

        static private User find(string txt)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(txt, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            User user = null;
            if (reader.HasRows)
            {
                reader.Read();
                user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), (TypeUser)Enum.Parse(typeof(TypeUser), reader.GetString(4)), (TypeProfile)Enum.Parse(typeof(TypeProfile), reader.GetString(5)));
            }
            reader.Close();
            conn.Close();
            return user;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public String USERNAME
        {
            get { return username; }
            set { username = value; }
        }

        public String PASSWORD
        {
            get { return password; }
            set { password = value; }
        }

        public String EMAIL
        {
            get { return email; }
            set { email = value; }
        }

        public TypeUser USER_TYPE
        {
            get { return typeU; }
            set { typeU = value; }
        }

        public TypeProfile PROFILE_TYPE
        {
            get { return typeP; }
            set { typeP = value; }
        }
    }
}