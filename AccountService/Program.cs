using Microsoft.Data.SqlClient;
using System;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Runtime.CompilerServices;

namespace AccountService
{
    class Program
    {
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
                while(await reader.ReadAsync())
                {
                    jsonString = program.SqlDataReaderToJson(reader);
                }
                Console.WriteLine(jsonString);

                
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
        public string SqlDataReaderToJson(SqlDataReader queryResult)
        {
            string jsonResult = "";

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            for (int i = 0; i < queryResult.FieldCount; i++)
            {
                jsonResult = JsonSerializer.Serialize(queryResult.GetString(i), options).ToString();
            }
            jsonResult = jsonResult.Replace(@"\", "");
            
            return jsonResult;
        }
    }
}