using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public class clsTestTypesDataAccess
    {
        public static bool GetTestInfoByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT * from TestTypes
                                WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeDescription = (string)reader["TestTypeDescription"];
                    TestTypeFees = (decimal)reader["TestTypeFees"];
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

        public static DataTable GetAllTestTypes()
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"select * from TestTypes";

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

        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"Update  TestTypes  
                            set TestTypeTitle = @TestTypeTitle, 
                                TestTypeDescription = @TestTypeDescription, 
                                TestTypeFees = @TestTypeFees
                                where TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

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
    }
}