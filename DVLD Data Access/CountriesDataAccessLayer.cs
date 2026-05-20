using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLDDataAccess
{
    public class clsCountryDataAccess
    {
        public static DataTable GetAllCountries()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = "select * from Countries";

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

        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = "select * from Countries where CountryID = @CountryID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", ID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    CountryName = (string)reader["CountryName"];
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

        public static bool GetCountryInfoByName(ref int ID, string CountryName)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = "select * from Countries where CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ID = (int)reader["CountryID"];
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
    }
}
