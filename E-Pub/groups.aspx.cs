using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class groups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                return;
        }

        protected void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            String groupName = GroupList.SelectedRow.Cells[1].Text;
            Group group = Group.findGroup(groupName);
            Session["groupName"] = group.GROUP_NAME;
            Response.Redirect(string.Format("chat.aspx?groupId={0}",group.ID), true);
        }

        protected void CreateGroupButton_Click(object sender, EventArgs e)
        {
            User user = (User)(Session["user"]);
            if (user.USER_TYPE == TypeUser.GUEST)
            {
                WarningCreateGroup.Visible = true;
                WarningCreateGroup.Text = "CAN'T CREATE GROUP UNLESS YOU ARE REGISTERED!";
                return;
            }
            Response.Redirect(string.Format("groupCreate.aspx"), true);
        }
    }
}