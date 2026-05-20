using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLDDataAccess
{
    public class clsApplicationTypesDataAccess
    {
        public static bool GetApplicationInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = @"SELECT * from ApplicationTypes
                                WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationFees = (decimal)reader["ApplicationFees"];
                }

                reader.Close();
            }

            catch (Exception ex)
            {
                clsSettings.LogError(ex);

                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetApplicationInfoByName(ref int ApplicationTypeID, string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = @"SELECT * from ApplicationTypes
                                WHERE ApplicationTypeTitle = @ApplicationTypeTitle";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationFees = (decimal)reader["ApplicationFees"];
                }

                reader.Close();
            }

            catch (Exception ex)
            {
                clsSettings.LogError(ex);

                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllApplicationTypes()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = @"select * from ApplicationTypes";

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

            catch (Exception ex)
            {
                clsSettings.LogError(ex);
            }

            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @ApplicationTypeTitle, 
                                ApplicationFees = @ApplicationFees
                                where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsSettings.LogError(ex);
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
