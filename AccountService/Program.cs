using Microsoft.Data.SqlClient;
using System;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AccountService
{
    class Program
    {
        static async Task Main(string[] args)
        {
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

                var sql = "SELECT UserName FROM Users";
                //var sql = "CREATE TABLE Users(UserID int IDENTITY(1,1) NOT NULL,UserName varchar(50) NOT NULL,UserPassword varchar(50) NOT NULL, PRIMARY KEY (UserID));";
                //var sql = "ALTER TABLE Users ADD PRIMARY KEY (UserID)";
                //var sql = "INSERT INTO Users (UserName,UserPassword) VALUES ('AdminCortex','CortexAdmin00')";
                await using var command = new SqlCommand(sql,connection);
                await using var reader = await command.ExecuteReaderAsync();

                while(await reader.ReadAsync())
                {
                    Console.WriteLine(reader.GetString(0));
                    //Console.WriteLine("{0}{1}", reader.GetString(0), reader.GetString(1));
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