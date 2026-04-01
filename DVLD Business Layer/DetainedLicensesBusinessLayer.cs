using DVLDBusinessLayer;
using DVLDDataAccess;
using System;
using System.Data;

public class clsDetainedLicense
{
    public int DetainID { get; set; }
    public int LicenseID { get; set; }
    public DateTime DetainDate { get; set; }
    public decimal FineFees { get; set; }
    public int CreatedByUserID { get; set; }
    public bool IsReleased { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int ReleasedByUserID { get; set; }
    public int ReleaseApplicationID { get; set; }
    public clsLicense LicenseInfo;
    public clsUser CreatedByUserInfo;
    public clsUser ReleasedByUserInfo;
    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;


    public clsDetainedLicense()
    {
        this.DetainID = -1;
        this.LicenseID = -1;
        this.DetainDate = DateTime.Now;
        this.FineFees = 0;
        this.CreatedByUserID = -1;
        this.IsReleased = false;
        this.ReleaseDate = DateTime.MaxValue;
        this.ReleasedByUserID = -1;
        this.ReleaseApplicationID = -1;

        Mode = enMode.AddNew;
    }

    private clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID, bool IsReleased,
        DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
    {
        this.DetainID = DetainID;
        this.LicenseID = LicenseID;
        this.DetainDate = DetainDate;
        this.FineFees = FineFees;
        this.CreatedByUserID = CreatedByUserID;
        this.IsReleased = IsReleased;
        this.ReleaseDate = ReleaseDate;
        this.ReleasedByUserID = ReleasedByUserID;
        this.ReleaseApplicationID = ReleaseApplicationID;
        LicenseInfo = clsLicense._FindByLicenseID(LicenseID);
        CreatedByUserInfo = clsUser.FindUserByUserID(this.CreatedByUserID);

        if (this.IsReleased)
            this.ReleasedByUserInfo = clsUser.FindUserByUserID(this.ReleasedByUserID);
        else
            this.ReleasedByUserInfo = null;

        Mode = enMode.Update;
    }

    public static clsDetainedLicense _FindByDetainID(int DetainID)
    {
        int LicenseID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
        DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
        decimal FineFees = 0;
        bool IsReleased = false;

        if (clsDetainedLicenseDataAccess.GetDetainedLicenseInfoByID(DetainID, ref LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))

            return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate,
                ReleasedByUserID, ReleaseApplicationID);
        else
            return null;
    }

    public static clsDetainedLicense _FindByLicenseID(int LicenseID)
    {
        int DetainID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
        DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
        decimal FineFees = 0;
        bool IsReleased = false;

        if (clsDetainedLicenseDataAccess.GetDetainedLicenseInfoByLicenseID(ref DetainID, LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))

            return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate,
                ReleasedByUserID, ReleaseApplicationID);
        else
            return null;
    }

    private bool _AddNewDetainedLicense()
    {
        this.DetainID = clsDetainedLicenseDataAccess.AddNewDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID,
            this.IsReleased, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);

        return (this.DetainID != -1);
    }

    private bool _ReleaseDetainedLicense()
    {
        return clsDetainedLicenseDataAccess.ReleaseDetainedLicense(this.DetainID, this.ReleasedByUserID, this.ReleaseApplicationID);
    }

    public static DataTable _GetAllDetainedLicenses()
    {
        return clsDetainedLicenseDataAccess.GetAllDetainedLicenses();
    }

    public static bool _IsLicenseDetained(int LicenseID)
    {
        return clsDetainedLicenseDataAccess.IsLicenseDetained(LicenseID);
    }

    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewDetainedLicense())
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:
                return _ReleaseDetainedLicense();
        }

        return false;
    }
}