using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDDataAccess
{
    public static class clsLicensesDataAccess
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass, ref DateTime IssueDate,
            ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsSettings.connectionString))
            {
                string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                                PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch (Exception) { isFound = false; }
                }
            }
            return isFound;
        }

        public static bool GetLicenseInfoByApplicationID(ref int LicenseID, int ApplicationID, ref int DriverID, ref int LicenseClass, ref DateTime IssueDate,
            ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsSettings.connectionString))
            {
                string query = "SELECT * FROM Licenses WHERE ApplicationID = @ApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                LicenseID = (int)reader["LicenseID"];
                                DriverID = (int)reader["DriverID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                                PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch (Exception) { isFound = false; }
                }
            }
            return isFound;
        }

        public static bool GetLicenseInfoByUserID(ref int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass, ref DateTime IssueDate,
           ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, ref bool IsActive, ref byte IssueReason, int CreatedByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsSettings.connectionString))
            {
                string query = "SELECT * FROM Licenses WHERE CreatedByUserID = @CreatedByUserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                LicenseID = (int)reader["LicenseID"];
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                                PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
                            }
                        }
                    }
                    catch (Exception) { isFound = false; }
                }
            }
            return isFound;
        }

        public static bool GetLicenseInfoByDriverID(ref int LicenseID, ref int ApplicationID, int DriverID, ref int LicenseClass, ref DateTime IssueDate,
           ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsSettings.connectionString))
            {
                string query = "SELECT * FROM Licenses WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                LicenseID = (int)reader["LicenseID"];
                                ApplicationID = (int)reader["ApplicationID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                                PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch (Exception) { isFound = false; }
                }
            }
            return isFound;
        }

        public static DataTable GetLocalLicensesHistory(int DriverID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"select LicenseID, ApplicationID, ClassName, IssueDate, ExpirationDate, IsActive from Licenses
                                inner join LicenseClasses on Licenses.LicenseClass = LicenseClasses.LicenseClassID
                                where DriverID = @DriverID
                                order by IsActive desc, LicenseID desc";

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

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
            string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            using (SqlConnection connection = new SqlConnection(clsSettings.connectionString))
            {
                string query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, 
                                 IssueDate, ExpirationDate, Notes, PaidFees, IsActive, 
                                 IssueReason, CreatedByUserID)
                                 VALUES (@ApplicationID, @DriverID, @LicenseClass, 
                                 @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, 
                                 @IssueReason, @CreatedByUserID);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                    if (string.IsNullOrWhiteSpace(Notes))
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", Notes);

                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                    catch (Exception)
                    {
                        // log later
                    }
                }
            }
            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);

            string query = @"UPDATE Licenses 
                     SET IsActive = 0 
                     WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch
            {
                // log later
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static int GetActiveLicenseIDByDriverID(int DriverID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);

            string query = @"SELECT LicenseID FROM Licenses 
                            INNER JOIN LicenseClasses on Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            WHERE DriverID = @DriverID
                            AND LicenseClassID = @LicenseClassID
                            AND IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            int LicenseID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }

            catch
            {
                // log later
            }

            finally
            {
                connection.Close();
            }

            return LicenseID;
        }
    }
}