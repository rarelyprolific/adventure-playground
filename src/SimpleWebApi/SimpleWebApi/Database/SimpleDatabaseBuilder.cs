using Microsoft.Data.SqlClient;

namespace SimpleWebApi.Database
{
    public class SimpleDatabaseBuilder : ISimpleDatabaseBuilder
    {
        private readonly ILogger<SimpleDatabaseBuilder> logger;
        private readonly IConfiguration configuration;

        public SimpleDatabaseBuilder(
            ILogger<SimpleDatabaseBuilder> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task BuildAsync()
        {
            string sqlServerSaPassword = configuration.GetValue<string>("SQL_SERVER_SA_PASSWORD");

            string connectionString = $"Data Source=sqlserver;User id=SA;Password={sqlServerSaPassword};TrustServerCertificate=True;";

            var connection = new SqlConnection(connectionString);

            var createDatabaseCommand = new SqlCommand("CREATE DATABASE [SimpleDatabase];", connection);

            try
            {
                connection.Open();
                await createDatabaseCommand.ExecuteNonQueryAsync();
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

            // This is a GOTCHA! If you attempt to log into a database which does not exist, then create it on the fly and immediately try to log in it
            // will likely fail. This is due to ADO.NET connection pooling remembering the failed connection. The fix (I think) is to clear the pools.
            SqlConnection.ClearAllPools();
        }
    }
}
