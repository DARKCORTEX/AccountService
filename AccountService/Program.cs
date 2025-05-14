using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text.Encodings.Web;


namespace AccountService
{
    class Program
    {
        public class Users
        {
            public int? UserID { get; set; }
            public string? UserName {get; set; }
            public string? UserPassword {get; set; }
        }
        static async Task Main(string[] args)
        {
            var program = new Program();
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "server-cortex.database.windows.net",
                UserID = "AdminCortex",
                Password = "CortexAdmin00",
                InitialCatalog = "database-cortex",
            };

            var connectionString = builder.ConnectionString;
            try
            {
                await using var connection = new SqlConnection(connectionString);
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=============================\n");

                await connection.OpenAsync();

                var sql = "SELECT * FROM Users FOR JSON PATH";
                //var sql = "CREATE TABLE Users(UserID int IDENTITY(1,1) NOT NULL,UserName varchar(50) NOT NULL,UserPassword varchar(50) NOT NULL, PRIMARY KEY (UserID));";
                //var sql = "ALTER TABLE Users ADD PRIMARY KEY (UserID)";
                //var sql = "INSERT INTO Users (UserName,UserPassword) VALUES ('new User','New Password')";
                await using var command = new SqlCommand(sql,connection);
                await using var reader = await command.ExecuteReaderAsync();

                string jsonString = "";
                if(await reader.ReadAsync())
                {
                    jsonString = reader.GetString(0);
                }
                Console.WriteLine(jsonString);
 
                List<Users>? users = JsonSerializer.Deserialize<List<Users>>(jsonString);
                if(users != null)
                {
                    foreach (var user in users)
                    {
                        Console.WriteLine($"UserID: {user.UserID}, UserName: {user.UserName}, UserPassword: {user.UserPassword}");
                    }
                }else
                {
                    Console.WriteLine("No users found or deserialization failed.");
                }
            }
            catch (SqlException e) when (e.Number == 1)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nDone. Press Enter.");
            Console.ReadLine();
        }
    }
}