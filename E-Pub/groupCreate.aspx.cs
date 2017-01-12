using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class groupCreate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                return;
            if (IsPostBack)
                return;
        }

        protected void CreateGroupButton_Click(object sender, EventArgs e)
        {
            String groupName = GroupNameTextBox.Text;
            List<User> members = new List<User>();
            foreach(ListItem item in FriendList.Items)
            {
                if (item.Selected)
                {
                    members.Add(E_Pub.User.findUser(item.Text));
                }
            }
            members.Add((User)Session["user"]);
            Group newGroup = new Group(groupName, members);
            newGroup.insert();
            Session["groupName"] = newGroup.GROUP_NAME;
            Response.Redirect(string.Format("chat.aspx?groupId={0}",newGroup.ID), true);
        }
    }
}