using DVLDBusinessLayer;
using DVLDDataAccess;
using System;
using System.Data;
using System.IO;

public class clsInternationalLicense
{
    public int InternationalLicenseID { get; set; }
    public int ApplicationID { get; set; }
    public int DriverID { get; set; }
    public int IssuedUsingLocalLicenseID { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public int CreatedByUserID { get; set; }
    public clsApplication ApplicationInfo;
    public clsDriver DriverInfo;
    public clsUser UserInfo;
    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;

    public clsInternationalLicense()
    {
        this.InternationalLicenseID = -1;
        this.ApplicationID = -1;
        this.DriverID = -1;
        this.IssuedUsingLocalLicenseID = -1;
        this.IssueDate = DateTime.Now;
        this.ExpirationDate = DateTime.Now;
        this.IsActive = true;
        this.CreatedByUserID = -1;

        Mode = enMode.AddNew;
    }

    private clsInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
        DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
    {
        this.InternationalLicenseID = InternationalLicenseID;
        this.ApplicationID = ApplicationID;
        this.DriverID = DriverID;
        this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
        this.IssueDate = IssueDate;
        this.ExpirationDate = ExpirationDate;
        this.IsActive = IsActive;
        this.CreatedByUserID = CreatedByUserID;
        ApplicationInfo = clsApplication._FindApplicationByID(ApplicationID);
        DriverInfo = clsDriver._GetDriverInfoByDriverID(DriverID);
        UserInfo = clsUser.FindUserByUserID(CreatedByUserID);
        Mode = enMode.Update;
    }

    public static clsInternationalLicense _FindByID(int InternationalLicenseID)
    {
        int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
        DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
        bool IsActive = true;

        if (clsInternationalLicensesDataAccess.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID, ref DriverID,
            ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))

            return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate,
                IsActive, CreatedByUserID);
        else
            return null;
    }

    public static clsInternationalLicense _FindByApplicationID(int ApplicationID)
    {
        int InternationalLicenseID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
        DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
        bool IsActive = true;

        if (clsInternationalLicensesDataAccess.GetInternationalLicenseInfoByApplicationID(ref InternationalLicenseID, ApplicationID, ref DriverID,
            ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))

            return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate,
                IsActive, CreatedByUserID);
        else
            return null;
    }

    public static clsInternationalLicense _FindByDriverID(int DriverID)
    {
        int InternationalLicenseID = -1, ApplicationID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
        DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
        bool IsActive = true;

        if (clsInternationalLicensesDataAccess.GetInternationalLicenseInfoByDriverID(ref InternationalLicenseID, ref ApplicationID, DriverID,
            ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))

            return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate,
                IsActive, CreatedByUserID);
        else
            return null;
    }

    private bool _AddNewInternationalLicense()
    {
        this.InternationalLicenseID = clsInternationalLicensesDataAccess.AddNewInternationalLicense(
            this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID,
            this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);

        return (this.InternationalLicenseID != -1);
    }

    public static bool _DeleteInternationalLicense(int InternationalLicenseID)
    {
        return clsInternationalLicensesDataAccess.DeleteInternationalLicense(InternationalLicenseID);
    }

    public static DataTable _GetAllInternationalLicenses()
    {
        return clsInternationalLicensesDataAccess.GetAllInternationalLicenses();
    }

    public static DataTable _GetInternationalLicensesHistory(int DriverID)
    {
        return clsInternationalLicensesDataAccess.GetInternationalLicensesHistory(DriverID);
    }

    public bool IsExpired()
    {
        return (this.ExpirationDate < DateTime.Now);
    }

    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewInternationalLicense())
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }
        }

        return false;
    }
}