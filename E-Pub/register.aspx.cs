using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                Response.Redirect(string.Format("main.aspx?profile={0}", user.ID), true);
            }
        }

        protected void goToOriginalPage()
        {
            String url = (String)ViewState["originalPage"];
            if (url == null)
            {
                url = "index.aspx";
            }
            Response.Redirect(url, true);
        }

        protected String hashPassword(String original)
        {
            var provider = new SHA1CryptoServiceProvider();
            var encoding = new UnicodeEncoding();
            return Convert.ToBase64String(provider.ComputeHash(encoding.GetBytes(original)));
        }

        protected void registerButton_Click(object sender, EventArgs e)
        {
            bool succes = false;
            String username = regUserBox.Text;
            String password = hashPassword(regPassBox.Text);
            String password2 = hashPassword(regPass2Box.Text);
            String email = regEmailBox.Text;
            User user = E_Pub.User.findUser(username);
            if (user != null)
            {
                registerWarningLabel.Text = "USERNAME ALREADY TAKEN!";
                return;
            }
            user = new E_Pub.User(username, password, email, TypeUser.USER, TypeProfile.PUBLIC);
            if (!password.Equals(password2))
            {
                registerWarningLabel.Text = "PASSWORDS NOT MATCHING";
                return;
            }
            if (user.exists())
            {
                registerWarningLabel.Text = "USERNAME IS ALREADY TAKEN";
            }
            else
            {
                user = (User)user.insert();
                Session["user"] = user;
                Session["userId"] = user.ID;
                succes = true;
            }
            if (succes)
            {
                goToOriginalPage();
            }
        }
    }
}