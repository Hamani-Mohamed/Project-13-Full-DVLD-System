using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace DVLDDataAccess
{
    public class clsLicenseClassesDataAccess
    {
        public static DataTable GetAllLicenseClasses()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = "select * from LicenseClasses";

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

        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref decimal ClassFees)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = "select * from LicenseClasses where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = (decimal)reader["ClassFees"];
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

        public static bool GetLicenseClassInfoByName(ref int LicenseClassID, string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref decimal ClassFees)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            string query = "select * from LicenseClasses where ClassName = @ClassName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClassName", ClassName);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    isFound = true;
                    LicenseClassID = (int)reader["LicenseClassID"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = (decimal)reader["ClassFees"];
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
