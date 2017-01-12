using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["user"] != null)
            {
                User user = (User)Session["user"];
                Response.Redirect(string.Format("main.aspx?profile={0}",user.ID), true);
            }
        }

        protected void goToOriginalPage()
        {
            String url = (String)ViewState["originalPage"];
            if(url == null)
            {
                url = "index.aspx";
            }
            Response.Redirect(url, true);
        }

        protected void guestButton_Click(object sender, EventArgs e)
        {
            User user = new E_Pub.User(-1, "guest", "guest", "", TypeUser.GUEST, TypeProfile.PRIVATE);
            Session["user"] = user;
            Session["userId"] = -1;
            goToOriginalPage();
        }

        protected String hashPassword(String original)
        {
            var provider = new SHA1CryptoServiceProvider();
            var encoding = new UnicodeEncoding();
            return Convert.ToBase64String(provider.ComputeHash(encoding.GetBytes(original)));
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            bool succes = false;
            String username = userBox.Text;
            String password = hashPassword(passBox.Text);
            User user = (User)E_Pub.User.findUser(username, password);
            if (user != null)
            {
                loginWarningLabel.Text = "";
                Session["user"] = user;
                Session["userId"] = user.ID;
                succes = true;
            }else
            {
                loginWarningLabel.Text = "USERNAME OR PASSWORD ARE WRONG";
            }
            if (succes)
            {
                goToOriginalPage();
            }
        }
    }
}