using DVLDDataAccess;
using System;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DVLDBusinessLayer
{
    public class clsLicense
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public byte IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public clsApplication ApplicationInfo;
        public clsDriver DriverInfo;
        public clsLicenseClass LicenseClassInfo;
        public clsUser UserInfo;

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = 1; // First Time
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate, string Notes,
            decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClassID = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            ApplicationInfo = clsApplication._FindApplicationByID(this.ApplicationID);
            DriverInfo = clsDriver._GetDriverInfoByDriverID(this.DriverID);
            LicenseClassInfo = clsLicenseClass._FindByID(this.LicenseClassID);
            UserInfo = clsUser.FindUserByUserID(CreatedByUserID);

            Mode = enMode.Update;
        }

        public static clsLicense _FindByLicenseID(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = 0;
            bool IsActive = true;
            byte IssueReason = 1;

            if (clsLicensesDataAccess.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate,
                ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes,
                    PaidFees, IsActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsLicense _FindByApplicationID(int ApplicationID)
        {
            int LicenseID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = 0;
            bool IsActive = true;
            byte IssueReason = 1;

            if (clsLicensesDataAccess.GetLicenseInfoByApplicationID(ref LicenseID, ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate,
                ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes,
                    PaidFees, IsActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsLicense _FindByUserID(int CreatedByUserID)
        {
            int LicenseID = -1, DriverID = -1, LicenseClass = -1, ApplicationID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = 0;
            bool IsActive = true;
            byte IssueReason = 1;

            if (clsLicensesDataAccess.GetLicenseInfoByUserID(ref LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate,
                ref Notes, ref PaidFees, ref IsActive, ref IssueReason, CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes,
                    PaidFees, IsActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsLicense _FindByDriverID(int DriverID)
        {
            int LicenseID = -1, CreatedByUserID = -1, LicenseClass = -1, ApplicationID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = 0;
            bool IsActive = true;
            byte IssueReason = 1;

            if (clsLicensesDataAccess.GetLicenseInfoByDriverID(ref LicenseID, ref ApplicationID, DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate,
                ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes,
                    PaidFees, IsActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicensesDataAccess.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, this.IssueReason, this.CreatedByUserID);

            return (this.LicenseID != -1);
        }

        public static DataTable _GetLocalLicensesHistory(int DriverID)
        {
            return clsLicensesDataAccess.GetLocalLicensesHistory(DriverID);
        }

        public bool IsExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }

        public static int _GetActiveLicenseIDByDriverID(int DriverID, int LicenseClassID)
        {
            return (clsLicensesDataAccess.GetActiveLicenseIDByDriverID(DriverID, LicenseClassID));
        }

        public bool _DeactivateLicense()
        {
            return clsLicensesDataAccess.DeactivateLicense(this.LicenseID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return false;
            }
            return false;
        }
    }
}