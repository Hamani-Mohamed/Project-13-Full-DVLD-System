using System;
using System.Data;
using DVLDDataAccess;

namespace DVLDBusinessLayer
{
    public class clsApplication
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsPerson PersonInfo;
        public clsApplicationType ApplicationTypeInfo;
        public clsUser UserInfo;

        public enum enMode { AddNew, UpdateMode };
        public enMode Mode = enMode.UpdateMode;

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = 0;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;
            this.Mode = enMode.AddNew;
        }

        protected clsApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate,
            decimal PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            PersonInfo = clsPerson._FindPersonByID(this.ApplicantPersonID);
            ApplicationTypeInfo = clsApplicationType._GetApplicationInfoByID(this.ApplicationTypeID);
            UserInfo = clsUser.FindUserByUserID(CreatedByUserID);

            Mode = enMode.UpdateMode;
        }

        public static clsApplication _FindApplicationByID(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = 0;
            decimal PaidFees = -1;

            if (clsApplicationsDataAccess.GetApplicationInfoByID(ApplicationID, ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID,
                ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsApplication _FindApplicationByPersonID(int ApplicantPersonID)
        {
            int ApplicationID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = 0;
            decimal PaidFees = -1;

            if (clsApplicationsDataAccess.GetApplicationInfoByPersonID(ref ApplicationID, ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID,
                ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }
            else
                return null;
        }

        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationsDataAccess.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
                this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return (clsApplicationsDataAccess.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
                this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID));
        }

        public static bool _DeleteApplication(int ApplicationID)
        {
            return clsApplicationsDataAccess.DeleteApplication(ApplicationID);
        }

        public static bool _CancelApplication(int ApplicationID)
        {
            return clsApplicationsDataAccess.CancelApplication(ApplicationID);
        }

        public static DataTable _GetAllApplications()
        {
            return clsApplicationsDataAccess.GetAllApplications();
        }

        public static bool _DoesApplicationExistByApplicationID(int ApplicationID)
        {
            return clsApplicationsDataAccess.DoesApplicationExistByID(ApplicationID);
        }

        public static bool _DoesApplicationExistByPersonID(int ApplicantPersonID)
        {
            return clsApplicationsDataAccess.DoesApplicationExistByPersonID(ApplicantPersonID);
        }

        public static bool _DoesPersonHaveActiveApplication(int ApplicantPersonID, int LicenseClassID)
        {
            return clsApplicationsDataAccess.DoesPersonHaveActiveApplication(ApplicantPersonID, LicenseClassID);
        }

        public bool _SetComplete()
        {
            if (clsApplicationsDataAccess.SetComplete(this.ApplicationID))
            {
                this.ApplicationStatus = 3;
                this.LastStatusDate = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                        return false;
                case enMode.UpdateMode:
                    {
                        return _UpdateApplication();
                    }
            }
            return false;
        }
    }
}