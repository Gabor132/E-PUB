using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pub
{
    public enum Visibility
    {
        PUBLIC, PRIVATE
    }

    public class Profile
    {
        private User user;
        private Visibility visibility;

        public Profile(User user, Visibility visibility)
        {
            this.user = user;
            this.visibility = visibility;
        }

        public User USER
        {
            get { return user; }
            set { user = value; }
        }
        public Visibility VISIBILITY
        {
            get { return visibility; }
            set { visibility = value; }
        }
    }
}