using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public class clsUsersDataAccess
    {
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string Username, ref string Password, ref bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT Users.*, 
                                LTRIM(RTRIM(REPLACE(REPLACE(ISNULL(People.FirstName, '') + ' ' + ISNULL(People.SecondName, '') + ' ' + ISNULL(People.ThirdName, '') + ' ' + ISNULL(People.LastName, ''), '  ', ' '), '  ', ' '))) as FullName 
                                FROM Users 
                                INNER JOIN People ON Users.PersonID = People.PersonID 
                                WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
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

        public static bool GetUserInfoByPersonID(ref int UserID, int PersonID, ref string Username, ref string Password, ref bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT Users.*, 
                                LTRIM(RTRIM(REPLACE(REPLACE(ISNULL(People.FirstName, '') + ' ' + ISNULL(People.SecondName, '') + ' ' + ISNULL(People.ThirdName, '') + ' ' + ISNULL(People.LastName, ''), '  ', ' '), '  ', ' '))) as FullName 
                                FROM Users 
                                INNER JOIN People ON Users.PersonID = People.PersonID 
                                WHERE Users.PersonID = @PersonID";

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
                    UserID = (int)reader["UserID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
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

        public static bool GetUserInfoByUsernameAndPassword(ref int UserID, ref int PersonID, string Username, string Password, ref bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT Users.*, 
                                LTRIM(RTRIM(REPLACE(REPLACE(ISNULL(People.FirstName, '') + ' ' + ISNULL(People.SecondName, '') + ' ' + ISNULL(People.ThirdName, '') + ' ' + ISNULL(People.LastName, ''), '  ', ' '), '  ', ' '))) as FullName 
                                FROM Users 
                                INNER JOIN People ON Users.PersonID = People.PersonID 
                                WHERE Users.Username = @Username AND Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);

            bool isFound = false;


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    IsActive = (bool)reader["IsActive"];
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

        public static int AddNewUser(int PersonID, string Username, string Password, bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"insert into Users (PersonID, Username, Password, IsActive)
                            values (@PersonID, @Username, @Password, @IsActive);
                            select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            int UserID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }

            catch (Exception)
            {

            }

            finally
            {
                connection.Close();
            }

            return UserID;
        }

        public static bool UpdateUser(int UserID, string Username, string Password, bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"Update  Users  
                            set Username = @Username, 
                                Password = @Password, 
                                IsActive = @IsActive
                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

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

        public static bool DeleteUser(int UserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"delete from Users
                        where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

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

        public static DataTable GetAllUsers()
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = @"SELECT
                                Users.UserID,
                                Users.PersonID,
                                LTRIM(RTRIM(REPLACE(REPLACE(ISNULL(People.FirstName, '') + ' ' + ISNULL(People.SecondName, '') + ' ' + ISNULL(People.ThirdName, '') + ' ' + ISNULL(People.LastName, ''), '  ', ' '), '  ', ' '))) AS FullName,
                                Users.Username,
                                CASE
                                    WHEN Users.IsActive = 0 THEN 'Inactive'
                                    ELSE 'Active'
                                END AS IsActive
                                FROM Users
                                INNER JOIN People ON Users.PersonID = People.PersonID";

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

        public static bool DoesUserExistByUserID(int UserID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select found = 1 from Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

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

        public static bool DoesUserExistByPersonID(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsSettings.connectionString);
            string query = "select found = 1 from Users where PersonID = @PersonID";

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

        public static bool DoesUserExistByUsername(string Username)
        {
            {
                SqlConnection connection = new SqlConnection(clsSettings.connectionString);
                string query = @"SELECT found = 1 from Users 
                                WHERE Username = @Username";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", Username);

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
}