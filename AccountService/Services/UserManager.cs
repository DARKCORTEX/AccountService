using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccountService.Services
{
    public class UserManager
    {
        public async Task<bool> LoginUser(string username, string password)
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
                await connection.OpenAsync();

                var sql = $"SELECT * FROM Users WHERE UserName = '{username}' AND UserPassword = '{password}' FOR JSON PATH, WITHOUT_ARRAY_WRAPPER";
                await using var command = new SqlCommand(sql, connection);
                await using var reader = await command.ExecuteReaderAsync();

                string jsonString = "";
                if (await reader.ReadAsync())
                {
                    jsonString = reader.GetString(0);
                }

                Models.Users? user = JsonSerializer.Deserialize<Models.Users>(jsonString);
                return user != null;
            }
            catch (SqlException e) when (e.Number == 1)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}