using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class SiteMaster : System.Web.UI.MasterPage, LoginChecker
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLogin();
            setupCurrentUserLink();
        }

        public void CheckLogin()
        {
            Object redirectObject = Session["isRedirected"];
            bool isRedirected = false;
            if(redirectObject != null)
            {
                isRedirected = (bool)redirectObject;
            }

            if (Session["user"] == null)
            {
                Session["isRedirected"] = false;
                Response.Redirect("login.aspx", false);
                ViewState["originalPage"] = Request.Url;
            }
            else
            {
                if (isRedirected)
                {
                    return;
                }
                User user = (User)Session["user"];
                switch (user.USER_TYPE)
                {
                    case TypeUser.GUEST:
                        {
                            Session["isRedirected"] = true;
                            Response.Redirect("main.aspx", true);
                        }
                        break;
                    case TypeUser.USER:
                        {
                            Session["isRedirected"] = true;
                            Response.Redirect(string.Format("main.aspx?profile={0}", user.USERNAME), true);
                        }
                        break;
                    case TypeUser.ADMIN:
                        {
                            Session["isRedirected"] = true;
                            Response.Redirect("admin.aspx", true);
                        }
                        break;
                }
            }
        }
        
        //SETUP//

        public void setupCurrentUserLink()
        {
            if (Session["user"] == null)
                return;
            if (((User)Session["user"]).USER_TYPE == TypeUser.ADMIN)
            {
                currentUserLink.Text = ((User)Session["user"]).USERNAME + " - ADMIN";
                AdminButton.Visible = true;
            }
            else
            {
                currentUserLink.Text = ((User)Session["user"]).USERNAME;
                AdminButton.Visible = false;
            }
            currentUserLink.NavigateUrl = string.Format("main.aspx?profile={0}", ((User)Session["user"]).USERNAME);
        }

        //EVENT HANDLERS//

        protected void logoutButton_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            CheckLogin();
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            String searchedUser = searchBox.Text;
            searchBox.Text = "";
            User user = (User)E_Pub.User.findUser(searchedUser);
            if (user != null)
            {
                Response.Redirect(string.Format("main.aspx?profile={0}", searchedUser), true);
            }
            else
            {
                searchWarningLabel.Text = "THERE IS NO SUCH USER";
            }
        }

        protected void GroupButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("groups.aspx"), true);
        }

        protected void Admin_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("admin.aspx"), true);
        }
    }
}