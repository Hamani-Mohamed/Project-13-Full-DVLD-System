using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public class clsCountryDataAccess
    {
        public static DataTable GetAllCountries()
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
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

            catch (Exception)
            {

            }

            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
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

        public static bool GetCountryInfoByName(ref int ID, string CountryName)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
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
    }
}