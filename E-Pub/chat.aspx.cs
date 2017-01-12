using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Pub
{
    public partial class chat : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                return;
            if (IsPostBack)
                return;
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            String messageText = MessageBox.Text;
            MessageBox.Text = "";
            Message message = new Message((int)Session["userId"], Group.findGroup((String)Session["groupName"]).ID, messageText);
            message.insert();
        }

        [WebMethod]
        public static List<String> getNewMessages(int groupId, int lastMessageId)
        {
            List<Message> newMessages = Message.findMessage(groupId, lastMessageId);
            return messageToString(newMessages);
        }

        [WebMethod]
        public static List<String> getAllMessages(int groupId)
        {
            List<Message> messages = Message.findMessage(groupId);
            return messageToString(messages);
        }

        [WebMethod]
        public static void deleteMessage(int messageId)
        {
            Message message = Message.findOneMessage(messageId);
            message.delete();
        }

        private static List<String> messageToString(List<Message> newMessages )
        {
            List<String> sNewMessages = new List<string>();
            foreach(Message m in newMessages)
            {
                sNewMessages.Add(m.ToString());
            }
            return sNewMessages;
        }
        
        [WebMethod]
        public static Boolean isAdmin(String username)
        {
            username = username.Split('-')[0].Split(' ')[0];
            User user = E_Pub.User.findUser(username);
            return user.USER_TYPE == TypeUser.ADMIN;
        }
    }
}