using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public class clsApplicationsDataAccess
    {
        public static bool GetApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus,
            ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

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
                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }

                reader.Close();
            }

            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetApplicationInfoByPersonID(ref int ApplicationID, int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus,
            ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT * FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ApplicationID = (int)reader["ApplicationID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }

                reader.Close();
            }

            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus,
            DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"insert into Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                            values (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                            select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int ApplicationID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationID = insertedID;
                }
            }

            catch (Exception)
            {

            }

            finally
            {
                connection.Close();
            }

            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus,
            DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"Update  Applications  
                            set ApplicantPersonID = @ApplicantPersonID, 
                                ApplicationDate = @ApplicationDate, 
                                ApplicationTypeID = @ApplicationTypeID,
                                ApplicationStatus = @ApplicationStatus,
                                LastStatusDate = @LastStatusDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID
                                where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool DeleteApplication(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"delete from Applications
                        where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int RowsAffected = 0;

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }

            catch (Exception)
            {
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static bool CancelApplication(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"Update  Applications 
                            set ApplicationStatus = 2
                            where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int RowsAffected = 0;

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }

            catch (Exception)
            {
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static DataTable GetAllApplications()
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT * FROM Applications";

            SqlCommand command = new SqlCommand(query, connection);

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

        public static bool DoesApplicationExistByID(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select found = 1 from Applications where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                object reader = command.ExecuteScalar();

                if (reader != null)
                {
                    return true;
                }
            }

            catch (Exception)
            {
                return false;
            }

            finally
            {
                connection.Close();
            }

            return false;
        }

        public static bool DoesApplicationExistByPersonID(int ApplicantPersonID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select found = 1 from Applications where ApplicantPersonID = @ApplicantPersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

            try
            {
                connection.Open();

                object reader = command.ExecuteScalar();

                if (reader != null)
                {
                    return true;
                }
            }

            catch (Exception)
            {
                return false;
            }

            finally
            {
                connection.Close();
            }

            return false;
        }

        public static bool DoesPersonHaveActiveApplication(int ApplicantPersonID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"select found = 1 from LocalDrivingLicenseFullApplications_View
                            where ApplicantPersonID = @ApplicantPersonID and (ApplicationStatus = 1 or ApplicationStatus = 3)
                            and LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object reader = command.ExecuteScalar();

                if (reader != null)
                {
                    return true;
                }
            }

            catch (Exception)
            {
                return false;
            }

            finally
            {
                connection.Close();
            }

            return false;
        }

        public static bool SetComplete(int ApplicationID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsSettings.connectionString))
            {
                string query = @"UPDATE Applications 
                         SET ApplicationStatus = 3, 
                             LastStatusDate = @LastStatusDate 
                         WHERE ApplicationID = @ApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception) { return false; }
                }
            }
            return (rowsAffected > 0);
        }
    }
}