using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                return;
            User user = (User)(Session["user"]);
            if(user.USER_TYPE != TypeUser.ADMIN)
            {
                Session["user"] = null;
                return;
            }
        }

        protected void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            String groupName = GroupList.SelectedRow.Cells[1].Text;
            Group group = Group.findGroup(groupName);
            Session["groupName"] = group.GROUP_NAME;
            Response.Redirect(string.Format("chat.aspx?groupId={0}", group.ID), true);
        }

        protected void AlbumList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int albumId = int.Parse(AlbumList.SelectedRow.Cells[1].Text);
            Response.Redirect(string.Format("albums.aspx?albumId={0}", albumId), true);
        }
    }
}