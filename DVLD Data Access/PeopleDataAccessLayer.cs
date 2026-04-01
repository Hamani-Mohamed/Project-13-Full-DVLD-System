using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public class clsPeopleDataAccess
    {
        public static bool GetPersonInfoByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
            ref byte Gender, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select * from People where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = reader["SecondName"] != DBNull.Value ? reader["SecondName"].ToString() : "";
                    ThirdName = reader["ThirdName"] != DBNull.Value ? reader["ThirdName"].ToString() : "";
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gender = (byte)reader["Gender"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];
                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : "";
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

        public static bool GetPersonInfoByNationalNo(ref int PersonID, string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
            ref byte Gender, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select * from People where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = reader["SecondName"] != DBNull.Value ? reader["SecondName"].ToString() : "";
                    ThirdName = reader["ThirdName"] != DBNull.Value ? reader["ThirdName"].ToString() : "";
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gender = (byte)reader["Gender"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];
                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : "";
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

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"insert into People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath)
                            values (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                            select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            if (SecondName == "")
                command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@SecondName", SecondName);
            if (ThirdName == "")
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            if (Email == "")
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            if (ImagePath == "")
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

            int PersonID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }
            }

            catch (Exception)
            {

            }

            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"Update  People  
                            set NationalNo = @NationalNo,
                                FirstName = @FirstName, 
                                SecondName = @SecondName, 
                                ThirdName = @ThirdName, 
                                LastName = @LastName,
                                DateOfBirth = @DateOfBirth,
                                Gender = @Gender,
                                Address = @Address,
                                Phone = @Phone, 
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath = @ImagePath
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            if (SecondName == "")
                command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@SecondName", SecondName);
            if (ThirdName == "")
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            if (Email == "")
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            if (ImagePath == "")
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

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

        public static bool DeletePerson(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"delete from People
                        where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static DataTable GetAllPeople()
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT
                            People.PersonID,
                            People.NationalNo,
                            People.FirstName,
                            People.SecondName,
                            People.ThirdName,
                            People.LastName,
                            CASE
                            WHEN People.Gender = 0 THEN 'Male'
                            ELSE 'Female'
                            END AS Gender,
                            People.DateOfBirth,
                            Countries.CountryName AS Country,
                            People.Phone,
                            People.Email
                            FROM People
                            INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID";

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

        public static bool DoesPersonExist(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select found = 1 from People where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool DoesPersonExist(string NationalNo)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select found = 1 from People where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

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