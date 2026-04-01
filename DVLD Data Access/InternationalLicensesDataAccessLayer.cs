using DVLDDataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

public class clsInternationalLicensesDataAccess
{
    public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID,
        ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);
        string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

        bool isFound = false;

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;
                ApplicationID = (int)reader["ApplicationID"];
                DriverID = (int)reader["DriverID"];
                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                IssueDate = (DateTime)reader["IssueDate"];
                ExpirationDate = (DateTime)reader["ExpirationDate"];
                IsActive = (bool)reader["IsActive"];
                CreatedByUserID = (int)reader["CreatedByUserID"];
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

    public static bool GetInternationalLicenseInfoByApplicationID(ref int InternationalLicenseID, int ApplicationID, ref int DriverID,
        ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);
        string query = "SELECT * FROM InternationalLicenses WHERE ApplicationID = @ApplicationID";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

        bool isFound = false;

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;
                InternationalLicenseID = (int)reader["InternationalLicenseID"];
                DriverID = (int)reader["DriverID"];
                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                IssueDate = (DateTime)reader["IssueDate"];
                ExpirationDate = (DateTime)reader["ExpirationDate"];
                IsActive = (bool)reader["IsActive"];
                CreatedByUserID = (int)reader["CreatedByUserID"];
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

    public static bool GetInternationalLicenseInfoByDriverID(ref int InternationalLicenseID, ref int ApplicationID, int DriverID,
        ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);
        string query = "SELECT * FROM InternationalLicenses WHERE DriverID = @DriverID";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@DriverID", DriverID);

        bool isFound = false;

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;
                InternationalLicenseID = (int)reader["InternationalLicenseID"];
                ApplicationID = (int)reader["ApplicationID"];
                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                IssueDate = (DateTime)reader["IssueDate"];
                ExpirationDate = (DateTime)reader["ExpirationDate"];
                IsActive = (bool)reader["IsActive"];
                CreatedByUserID = (int)reader["CreatedByUserID"];
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

    public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate,
        DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);

        string query = @"INSERT INTO InternationalLicenses (ApplicationID, DriverID, 
                         IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID)
                         VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, 
                         @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);
                         SELECT SCOPE_IDENTITY();";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        command.Parameters.AddWithValue("@DriverID", DriverID);
        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
        command.Parameters.AddWithValue("@IssueDate", IssueDate);
        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
        command.Parameters.AddWithValue("@IsActive", IsActive);
        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

        int InternationalLicenseID = -1;


        try
        {
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out int insertedID))
            {
                InternationalLicenseID = insertedID;
            }
        }
        catch (Exception)
        {
            // log later
        }
        finally
        {
            connection.Close();
        }

        return InternationalLicenseID;
    }

    public static bool DeleteInternationalLicense(int InternationalLicenseID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);
        string query = "DELETE FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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

    public static DataTable GetAllInternationalLicenses()
    {
        DataTable dt = new DataTable();
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);

        string query = @"SELECT 
                               InternationalLicenseID, 
                               ApplicationID, 
                               DriverID, 
                               IssuedUsingLocalLicenseID, 
                               -- Custom format: Day/Month/Year Hour:Minute
                               FORMAT(IssueDate, 'dd/MM/yyyy HH:mm') AS IssueDate, 
                               FORMAT(ExpirationDate, 'dd/MM/yyyy HH:mm') AS ExpirationDate,
                               -- Bit to String transformation
                               CASE 
                                   WHEN IsActive = 1 THEN 'Active'
                                   ELSE 'Inactive'
                               END AS IsActive
                           FROM InternationalLicenses";

        SqlCommand command = new SqlCommand(query, connection);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                dt.Load(reader);
            }

            reader.Close();
        }
        catch (Exception)
        {
            // log later
        }
        finally
        {
            connection.Close();
        }

        return dt;
    }

    public static DataTable GetInternationalLicensesHistory(int DriverID)
    {
        SqlConnection connection = new SqlConnection(clsSettings.connectionString);
        string query = @"select InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate,
                        IsActive from InternationalLicenses
                        where DriverID = @DriverID
                        order by IsActive desc, InternationalLicenseID desc";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@DriverID", DriverID);

        DataTable dt = new DataTable();

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                dt.Load(reader);
            }
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