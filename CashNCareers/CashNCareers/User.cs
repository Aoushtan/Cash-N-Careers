using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashNCareers
{
    //Class that holds a specific instance of the User object that is used to maintain their data across the entire app.
    public class User
    {
        //These variables exist for every User object and use the methods below to modify them
        public int UserID;
        public string UserEmail;
        public int CurrentHistID = -1; //Will be used for the edit part of the app
        //This method sets a User object's ID
        public void SetUserID(int ID)
        {
            UserID = ID;
        }
        //This method returns the User object's ID
        public int GetUserID()
        {
            return UserID;
        }
        //This method sets a User object's email
        public void SetUserEmail(string email)
        {
            UserEmail = email;
        }
        //This method returns the user object's email
        public string GetUserEmail()
        {
            return UserEmail;
        }
        //This method sets the current scenario ID (used in the history portion)
        public void SetCurrentSituation(int ID)
        {
            CurrentHistID = ID;
        }
    }
}