using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public class clsTestDataAccess
    {
        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : "";
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

        public static bool GetTestInfoByUserID(ref int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "SELECT * FROM Tests WHERE CreatedByUserID = @CreatedByUserID";

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
                    TestID = (int)reader["TestID"];
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : "";
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

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                            VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);

            if (Notes == "" || Notes == null)
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", Notes);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int TestID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }

            catch (Exception)
            {
                // could log later
            }

            finally
            {
                connection.Close();
            }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"UPDATE Tests 
                            SET TestAppointmentID = @TestAppointmentID, 
                                TestResult = @TestResult, 
                                Notes = @Notes, 
                                CreatedByUserID = @CreatedByUserID
                                WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);

            if (Notes == "" || Notes == null)
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", Notes);

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

        public static bool IsTestPassed(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"select * from Tests 
                           inner join TestAppointments on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                           where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                           and TestTypeID = @TestTypeID
                           and TestResult = 1";

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