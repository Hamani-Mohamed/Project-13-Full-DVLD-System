using System;
using System.Data;
using DVLDDataAccess;

namespace DVLDBusinessLayer
{
    public class clsTest
    {
        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get; }
        public clsTestAppointment TestAppointmentInfo;
        public clsUser UserInfo;
        public enum enMode { AddNew = 0, UpdateMode = 1 };
        public enMode Mode = enMode.AddNew;

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
            TestAppointmentInfo = clsTestAppointment._FindByTestAppointmentID(this.TestAppointmentID);
            UserInfo = clsUser.FindUserByUserID(CreatedByUserID);

            Mode = enMode.UpdateMode;
        }

        public static clsTest _FindByID(int TestID)
        {
            int TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Notes = "";

            if (clsTestDataAccess.GetTestInfoByID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            else
                return null;
        }

        public static clsTest _FindByUserID(int CreatedByUserID)
        {
            int TestAppointmentID = -1, TestID = -1;
            bool TestResult = false;
            string Notes = "";

            if (clsTestDataAccess.GetTestInfoByUserID(ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, CreatedByUserID))
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            else
                return null;
        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestDataAccess.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            return clsTestDataAccess.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public static bool _IsTestPassed(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestDataAccess.IsTestPassed(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.UpdateMode:
                    return _UpdateTest();
            }

            return false;
        }
    }
}