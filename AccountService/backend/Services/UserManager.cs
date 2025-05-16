using System;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Services
{
    public class UserManager
    {
        private readonly string _connectionString = new SqlConnectionStringBuilder
        {
            DataSource = "server-cortex.database.windows.net",
            UserID = "AdminCortex",
            Password = "CortexAdmin00",
            InitialCatalog = "database-cortex",
        }.ConnectionString;

        public async Task<Users?> LoginUserAsync(string username, string password)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"SELECT UserID, UserName, UserPassword 
                            FROM Users 
                            WHERE UserName = @username AND UserPassword = @password 
                            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER";

                await using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    string jsonString = reader.GetString(0);
                    return JsonSerializer.Deserialize<Users>(jsonString);
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Login Error: {e.Message}");
                return null;
            }
        }
    }
}