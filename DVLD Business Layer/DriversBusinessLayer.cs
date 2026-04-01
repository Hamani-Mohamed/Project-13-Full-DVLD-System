using DVLDDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsDriver
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreationDate { get; set; }
        public clsPerson PersonInfo;
        public clsUser UserInfo;

        enum enMode { AddNew, UpdateMode };
        enMode Mode = enMode.UpdateMode;

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreationDate = DateTime.Now;
            this.Mode = enMode.AddNew;
        }

        private clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreationDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreationDate = CreationDate;
            PersonInfo = clsPerson._FindPersonByID(this.PersonID);
            UserInfo = clsUser.FindUserByUserID(CreatedByUserID);

            Mode = enMode.UpdateMode;
        }

        public static clsDriver _GetDriverInfoByDriverID(int DriverID)
        {
            int PersonID = -1, CreatedByUserID = -1;
            DateTime CreationDate = DateTime.Now;

            if (clsDriversDataAccess.GetDriverInfoByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreationDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreationDate);
            }
            else
                return null;
        }

        public static clsDriver _GetDriverInfoByPersonID(int PersonID)
        {
            int DriverID = -1, CreatedByUserID = -1;
            DateTime CreationDate = DateTime.Now;

            if (clsDriversDataAccess.GetDriverInfoByPersonID(ref DriverID, PersonID, ref CreatedByUserID, ref CreationDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreationDate);
            }
            else
                return null;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = clsDriversDataAccess.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreationDate);

            return (this.DriverID != -1);
        }

        public static bool DeleteDriver(int DriverID)
        {
            return clsDriversDataAccess.DeleteDriver(DriverID);
        }

        public static DataTable _GetAllDrivers()
        {
            return clsDriversDataAccess.GetAllDrivers();
        }

        public static bool _DoesDriverExistByDriverID(int DriverID)
        {
            return clsDriversDataAccess.DoesDriverExistByDriverID(DriverID);
        }

        public static bool _DoesDriverExistByPersonID(int PersonID)
        {
            return clsDriversDataAccess.DoesDriverExistByPersonID(PersonID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                        return false;
                    //case enMode.UpdateMode:
                    //    {
                    //        return _UpdateDriver();
                    //    }
            }
            return false;
        }
    }
}