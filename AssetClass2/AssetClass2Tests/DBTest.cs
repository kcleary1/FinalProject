using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace AssetClass2Tests
{
    public class DatabaseConnectionTests
    {
        private readonly string _connectionString;

        public DatabaseConnectionTests()
        {
            _connectionString = "Server=localhost;Database=assetclassanalyzer;Uid=root;Pwd=password;";
        }

        [Fact]
        public void CanConnectToDatabase()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                Exception ex = Record.Exception(() =>
                {
                    connection.Open();
                    // Perform a simple query to ensure the connection is working
                    using (var command = new MySqlCommand("SELECT 1", connection))
                    {
                        command.ExecuteScalar();
                    }
                });

                if (ex != null)
                {
                    Console.WriteLine($"Exception details: {ex}");
                }

                Assert.Null(ex);
            }
        }
    }
}