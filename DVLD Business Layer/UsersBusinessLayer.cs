using System;
using System.Data;
using DVLDDataAccess;

namespace DVLDBusinessLayer
{
    public class clsUser
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public clsPerson PersonInfo;
        enum enMode { AddNew, UpdateMode };
        enMode Mode = enMode.UpdateMode;

        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.FullName = "";
            this.Username = "";
            this.Password = "";
            this.IsActive = true;
            this.Mode = enMode.AddNew;
        }

        private clsUser(int UserID, int PersonID, string FullName, string Username, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.FullName = FullName;
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;
            PersonInfo = clsPerson._FindPersonByID(this.PersonID);
            Mode = enMode.UpdateMode;
        }

        public static clsUser FindUserByUserID(int UserID)
        {
            int PersonID = -1;
            string FullName = "", Username = "", Password = "";
            bool IsActive = true;

            if (clsUsersDataAccess.GetUserInfoByUserID(UserID, ref PersonID, ref Username, ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, FullName, Username, Password, IsActive);
            }
            else
                return null;
        }

        public static clsUser FindUserByPersonID(int PersonID)
        {
            int UserID = -1;
            string FullName = "", Username = "", Password = "";
            bool IsActive = true;

            if (clsUsersDataAccess.GetUserInfoByPersonID(ref UserID, PersonID, ref Username, ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, FullName, Username, Password, IsActive);
            }
            else
                return null;
        }

        public static clsUser FindUserByUsernameAndPassword(string Username, string Password)
        {
            int UserID = -1, PersonID = -1; ;
            string FullName = "";
            bool IsActive = true;

            if (clsUsersDataAccess.GetUserInfoByUsernameAndPassword(ref UserID, ref PersonID, Username, Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, FullName, Username, Password, IsActive);
            }
            else
                return null;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUsersDataAccess.AddNewUser(this.PersonID, this.Username, this.Password, this.IsActive);

            if (this.UserID != -1)
            {
                clsPerson person = clsPerson._FindPersonByID(this.PersonID);
                if (person != null)
                {
                    this.FullName = person.FullName;
                }
            }

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return (clsUsersDataAccess.UpdateUser(this.UserID, this.Username, this.Password, this.IsActive));
        }

        public static bool _DeleteUser(int UserID)
        {
            return clsUsersDataAccess.DeleteUser(UserID);
        }

        public static DataTable _GetAllUsers()
        {
            return clsUsersDataAccess.GetAllUsers();
        }

        public static bool _DoesUserExistByUserID(int UserID)
        {
            return clsUsersDataAccess.DoesUserExistByUserID(UserID);
        }

        public static bool _DoesUserExistByPersonID(int PersonID)
        {
            return clsUsersDataAccess.DoesUserExistByPersonID(PersonID);
        }

        public static bool _DoesUserExistByUsername(string Username)
        {
            return clsUsersDataAccess.DoesUserExistByUsername(Username);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                        return false;
                case enMode.UpdateMode:
                    {
                        return _UpdateUser();
                    }
            }
            return false;
        }
    }
}