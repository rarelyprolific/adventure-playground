using Microsoft.Data.SqlClient;

namespace SimpleWebApi.Database
{
    public class SimpleDatabaseBuilder : ISimpleDatabaseBuilder
    {
        private readonly ILogger<SqlConnectionBuilder> logger;
        private readonly IConfiguration configuration;

        public SimpleDatabaseBuilder(
            ILogger<SqlConnectionBuilder> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public void Build()
        {
            string sqlServerSaPassword = configuration.GetValue<string>("SQL_SERVER_SA_PASSWORD_FILE");

            string connectionString = $"Data Source=sqlserver;User id=SA;Password={sqlServerSaPassword};TrustServerCertificate=True;";

            var connection = new SqlConnection(connectionString);

            var createDatabaseCommand = new SqlCommand("CREATE DATABASE [SimpleDatabase];", connection);

            try
            {
                logger.LogInformation("Attempting to create SimpleDatabase");

                connection.Open();
                createDatabaseCommand.ExecuteNonQuery();
                connection.Close();

                logger.LogInformation("SimpleDatabase CREATED!");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // Empties all connection pools so we don't reuse the previously failed connection which caused this create database command to be invoked!
            SqlConnection.ClearAllPools();
        }
    }
}
