using DVLDDataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDBusinessLayer
{
    public class clsApplicationType
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }

        public clsApplicationType()
        {
            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
            this.ApplicationFees = -1;
        }

        public clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationFees = ApplicationFees;
        }

        public static clsApplicationType _GetApplicationInfoByID(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";
            decimal ApplicationFees = -1;

            if (clsApplicationTypesDataAccess.GetApplicationInfoByID(ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationFees))
            {
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
            }
            else
                return null;
        }

        public static clsApplicationType _GetApplicationInfoByName(string ApplicationTypeTitle)
        {
            int ApplicationTypeID = -1;
            decimal ApplicationFees = -1;

            if (clsApplicationTypesDataAccess.GetApplicationInfoByName(ref ApplicationTypeID, ApplicationTypeTitle, ref ApplicationFees))
            {
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
            }
            else
                return null;
        }

        public static DataTable _GetAllApplicationTypes()
        {
            return clsApplicationTypesDataAccess.GetAllApplicationTypes();
        }

        public bool UpdateApplicationType()
        {
            return (clsApplicationTypesDataAccess.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees));
        }
    }
}