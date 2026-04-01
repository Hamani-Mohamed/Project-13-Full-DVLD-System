using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public class clsTestAppointmentsDataAccess
    {
        public static bool FindTestAppointmentByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT * from TestAppointments
                                WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TestTypeID = (int)reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];
                    RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : (int)reader["RetakeTestApplicationID"];
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

        public static bool FindTestAppointmentByUserID(ref int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref decimal PaidFees, int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT * from TestAppointments
                                WHERE CreatedByUserID = @CreatedByUserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestTypeID = (int)reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    IsLocked = (bool)reader["IsLocked"];
                    RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : (int)reader["RetakeTestApplicationID"];
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

        public static bool FindTestAppointmentByLocalLicenseID(ref int TestAppointmentID, ref int TestTypeID, int LocalDrivingLicenseApplicationID,
        ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT * from TestAppointments
                                WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestTypeID = (int)reader["TestTypeID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    IsLocked = (bool)reader["IsLocked"];
                    RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : (int)reader["RetakeTestApplicationID"];
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

        public static int AddTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"insert into TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID,
                            IsLocked, RetakeTestApplicationID)
                            values (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
                            select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            int TestAppointmentID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                }
            }

            catch (Exception)
            {

            }

            finally
            {
                connection.Close();
            }

            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"Update  TestAppointments  
                            set TestTypeID = @TestTypeID, 
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID,
                                IsLocked = @IsLocked,
                                RetakeTestApplicationID = @RetakeTestApplicationID
                                where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

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

        public static DataTable GetPersonTestAppointments(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"select TestAppointmentID, AppointmentDate, PaidFees, IsLocked from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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

        public static DataTable GetPersonTestAppointmentsByTestTypeID(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"select TestAppointmentID, AppointmentDate, PaidFees, IsLocked from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            and TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

        public static int GetTotalTrialsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int TotalTrials = 0;
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);

            string query = @"SELECT COUNT(*)
                    FROM TestAppointments 
                    WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                    AND TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int count))
                {
                    TotalTrials = count;
                }
            }
            catch (Exception)
            {
                // log later
            }
            finally { connection.Close(); }

            return TotalTrials;
        }

        public static int GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);

            string query = @"SELECT COUNT(*) 
                    FROM TestAppointments
                    INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                    WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                    AND TestResult = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            int Count = 0;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int resultCount))
                {
                    Count = resultCount;
                }
            }
            catch (Exception)
            {
                // might log later
            }
            finally
            {
                connection.Close();
            }

            return Count;
        }

        public static bool DoesAppointmentExist(int TestAppointmentID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select found = 1 from TestAppointments where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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

        public static bool IsAppointmentActive(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"select found = 1 from TestAppointments where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            and TestTypeID = @TestTypeID and IsLocked = 0";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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
    }
}