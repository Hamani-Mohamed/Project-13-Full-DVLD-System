using DVLDDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLDBusinessLayer
{
    public class clsLocalLicense : clsApplication
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo;

        public clsLocalLicense()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;
            Mode = enMode.AddNew;
        }

        private clsLocalLicense(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID, int LicenseClassID)
            : base(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.LicenseClassID = LicenseClassID;
            LicenseClassInfo = clsLicenseClass._FindByID(this.LicenseClassID);

            Mode = enMode.UpdateMode;
        }

        public static clsLocalLicense FindLocalLicenseByID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            if (clsLocalLicensesDataAccess.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID))
            {
                clsApplication BassApp = clsApplication._FindApplicationByID(ApplicationID);

                if (BassApp != null)
                {
                    return new clsLocalLicense(LocalDrivingLicenseApplicationID, ApplicationID, BassApp.ApplicantPersonID, BassApp.ApplicationDate, BassApp.ApplicationTypeID,
                    BassApp.ApplicationStatus, BassApp.LastStatusDate, BassApp.PaidFees, BassApp.CreatedByUserID, LicenseClassID);
                }
            }

            return null;
        }

        private bool _AddNewLocalLicense()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalLicensesDataAccess.AddNewLocalLicense(this.ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalLicense()
        {
            return (clsLocalLicensesDataAccess.UpdateLocalLicense(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID));
        }

        public static bool _DeleteLocalLicense(int LocalDrivingLicenseApplicationID)
        {
            clsLocalLicense localApp = clsLocalLicense.FindLocalLicenseByID(LocalDrivingLicenseApplicationID);

            if (localApp == null)
                return false;

            if (clsLocalLicensesDataAccess.DeleteLocalLicense(LocalDrivingLicenseApplicationID))
            {
                return clsApplication._DeleteApplication(localApp.ApplicationID);
            }

            return false;
        }

        public static bool _DoesLocalLicenseExistByID(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalLicense._DoesLocalLicenseExistByID(LocalDrivingLicenseApplicationID);
        }

        public static DataTable _GetAllLocalLicenses()
        {
            return clsLocalLicensesDataAccess.GetAllLocalDrivingLicenseApplications();
        }

        public new bool Save()
        {
            enMode CurrentMode = Mode;

            if (!base.Save())
                return false;

            switch (CurrentMode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalLicense())
                    {
                        return true;
                    }
                    else
                        return false;
                case enMode.UpdateMode:
                    {
                        return _UpdateLocalLicense();
                    }
            }
            return false;
        }
    }
}