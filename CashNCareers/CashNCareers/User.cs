using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashNCareers
{
    public class User
    {
        public int UserID;
        public string UserEmail;
        public int CurrentHistID;
        public void SetUserID(int ID)
        {
            UserID = ID;
        }
        public int GetUserID()
        {
            return UserID;
        }
        public void SetUserEmail(string email)
        {
            UserEmail = email;
        }
        public string GetUserEmail()
        {
            return UserEmail;
        }

    }
}