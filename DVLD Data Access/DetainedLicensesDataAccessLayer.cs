using DVLDDataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

public class clsDetainedLicenseDataAccess
{
    public static bool GetDetainedLicenseInfoByID(int DetainID, ref int LicenseID, ref DateTime DetainDate, ref decimal FineFees,
        ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);
        string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@DetainID", DetainID);

        bool isFound = false;

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;
                LicenseID = (int)reader["LicenseID"];
                DetainDate = (DateTime)reader["DetainDate"];
                FineFees = (decimal)reader["FineFees"];
                CreatedByUserID = (int)reader["CreatedByUserID"];
                IsReleased = (bool)reader["IsReleased"];
                ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"];
                ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? -1 : (int)reader["ReleasedByUserID"];
                ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? -1 : (int)reader["ReleaseApplicationID"];
            }
            reader.Close();
        }

        catch (Exception)
        {
            isFound = false;
        }

        finally
        {
            connection.Close();
        }

        return isFound;
    }

    public static bool GetDetainedLicenseInfoByLicenseID(ref int DetainID, int LicenseID, ref DateTime DetainDate, ref decimal FineFees,
        ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
    {
        bool isFound = false;
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);

        string query = "SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID ORDER BY DetainID DESC";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@LicenseID", LicenseID);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;
                DetainID = (int)reader["DetainID"];
                DetainDate = (DateTime)reader["DetainDate"];
                FineFees = (decimal)reader["FineFees"];
                CreatedByUserID = (int)reader["CreatedByUserID"];
                IsReleased = (bool)reader["IsReleased"];

                ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"];
                ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? -1 : (int)reader["ReleasedByUserID"];
                ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? -1 : (int)reader["ReleaseApplicationID"];
            }
            reader.Close();
        }

        catch (Exception)
        {
            isFound = false;
        }

        finally
        {
            connection.Close();
        }

        return isFound;
    }

    public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID, bool IsReleased,
        DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);

        string query = @"INSERT INTO DetainedLicenses 
                         (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, 
                         ReleaseDate, ReleasedByUserID, ReleaseApplicationID)
                         VALUES 
                         (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, 
                         @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID);
                         SELECT SCOPE_IDENTITY();";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@LicenseID", LicenseID);
        command.Parameters.AddWithValue("@DetainDate", DetainDate);
        command.Parameters.AddWithValue("@FineFees", FineFees);
        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        command.Parameters.AddWithValue("@IsReleased", IsReleased);

        if (ReleaseDate == DateTime.MaxValue)
            command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
        else
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);

        if (ReleasedByUserID == -1)
            command.Parameters.AddWithValue("@ReleasedByUserID", DBNull.Value);
        else
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);

        if (ReleaseApplicationID == -1)
            command.Parameters.AddWithValue("@ReleaseApplicationID", DBNull.Value);
        else
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

        int DetainID = -1;

        try
        {
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out int insertedID))
            {
                DetainID = insertedID;
            }
        }

        catch (Exception)
        {

        }

        finally
        {
            connection.Close();
        }

        return DetainID;
    }

    public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);

        string query = @"UPDATE DetainedLicenses 
                         SET IsReleased = 1, 
                             ReleaseDate = @ReleaseDate, 
                             ReleasedByUserID = @ReleasedByUserID, 
                             ReleaseApplicationID = @ReleaseApplicationID 
                         WHERE DetainID = @DetainID";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@DetainID", DetainID);
        command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
        command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
        command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

        int rowsAffected = 0;

        try
        {
            connection.Open();
            rowsAffected = command.ExecuteNonQuery();
        }

        catch (Exception)
        {
            return false;
        }

        finally
        {
            connection.Close();
        }

        return (rowsAffected > 0);
    }

    public static bool IsLicenseDetained(int LicenseID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);

        string query = "SELECT TOP 1 Found = 1 FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@LicenseID", LicenseID);

        bool isDetained = false;

        try
        {
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null)
                isDetained = true;
        }

        catch (Exception)
        {

        }

        finally
        {
            connection.Close();
        }

        return isDetained;
    }

    public static DataTable GetAllDetainedLicenses()
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);

        string query = @"SELECT * FROM DetainedLicenses_View";

        SqlCommand command = new SqlCommand(query, connection);

        DataTable dt = new DataTable();

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) dt.Load(reader);
            reader.Close();
        }

        catch (Exception)
        {

        }

        finally
        {
            connection.Close();
        }

        return dt;
    }
}