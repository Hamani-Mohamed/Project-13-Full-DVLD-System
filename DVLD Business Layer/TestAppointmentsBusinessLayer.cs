using System;
using System.Data;
using DVLDDataAccess;

namespace DVLDBusinessLayer
{
    public class clsTestAppointment
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public clsTestType TestTypeInfo;
        public clsLocalLicense LocalLicenseInfo;
        public clsUser UserInfo;
        public enum enMode { AddNew = 0, UpdateMode = 1 };
        public enMode Mode = enMode.AddNew;

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;

            Mode = enMode.AddNew;
        }

        private clsTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            TestTypeInfo = clsTestType._GetTestInfoByID(this.TestTypeID);
            LocalLicenseInfo = clsLocalLicense.FindLocalLicenseByID(this.LocalDrivingLicenseApplicationID);
            UserInfo = clsUser.FindUserByUserID(CreatedByUserID);

            Mode = enMode.UpdateMode;
        }

        public static clsTestAppointment _FindByTestAppointmentID(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsLocked = false;

            if (clsTestAppointmentsDataAccess.FindTestAppointmentByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
                ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;
        }

        public static clsTestAppointment _FindByLocalLicenseID(int LocalDrivingLicenseApplicationID)
        {
            int TestTypeID = -1, TestAppointmentID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsLocked = false;

            if (clsTestAppointmentsDataAccess.FindTestAppointmentByLocalLicenseID(ref TestAppointmentID, ref TestTypeID, LocalDrivingLicenseApplicationID,
                ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;
        }

        public static clsTestAppointment _FindByUserID(int CreatedByUserID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, TestAppointmentID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsLocked = false;

            if (clsTestAppointmentsDataAccess.FindTestAppointmentByUserID(ref TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
                ref AppointmentDate, ref PaidFees, CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;
        }

        public static DataTable _GetPersonTestAppointments(int LocalDrivingLicenseApplicationID)
        {
            return clsTestAppointmentsDataAccess.GetPersonTestAppointments(LocalDrivingLicenseApplicationID);
        }

        public static DataTable _GetPersonTestAppointmentsByTestTypeID(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentsDataAccess.GetPersonTestAppointmentsByTestTypeID(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool _AddNewTestAppointment()
        {
            this.TestAppointmentID = clsTestAppointmentsDataAccess.AddTestAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate,
                this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }

        public bool _UpdateTestAppointment()
        {
            return clsTestAppointmentsDataAccess.UpdateTestAppointment(this.TestAppointmentID, this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate,
                this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
        }

        public static int _GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestAppointmentsDataAccess.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static int _GetTotalTrialsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentsDataAccess.GetTotalTrialsPerTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool _IsAppointmentActive(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentsDataAccess.IsAppointmentActive(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool _DoesAppointmentExist(int TestAppointmentID)
        {
            return clsTestAppointmentsDataAccess.DoesAppointmentExist(TestAppointmentID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.UpdateMode:
                    return _UpdateTestAppointment();
            }

            return false;
        }
    }
}