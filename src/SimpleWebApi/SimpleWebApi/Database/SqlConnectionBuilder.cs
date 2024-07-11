using Microsoft.Data.SqlClient;
using SimpleWebApi.Controllers;

namespace SimpleWebApi.Database
{
    public class SqlConnectionBuilder : ISqlConnectionBuilder
    {
        private readonly ILogger<SqlConnectionBuilder> logger;
        private readonly IConfiguration configuration;
        private readonly ISimpleDatabaseBuilder databaseBuilder;

        public SqlConnectionBuilder(
            ILogger<SqlConnectionBuilder> logger,
            IConfiguration configuration,
            ISimpleDatabaseBuilder databaseBuilder)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.databaseBuilder = databaseBuilder;
        }

        public SqlConnection Build()
        {
            string sqlServerSaPassword = configuration.GetValue<string>("SQL_SERVER_SA_PASSWORD_FILE");

            string connectionString = $"Data Source=sqlserver;Initial Catalog=SimpleDatabase;User id=SA;Password={sqlServerSaPassword};TrustServerCertificate=True;";

            var connection = new SqlConnection(connectionString);

            try
            {
                logger.LogInformation("Attempting to connect to SimpleDatabase!");
                connection.Open();
            }
            catch(SqlException sqlEx) when (sqlEx.Message.Contains("Cannot open database"))
            {
                logger.LogWarning("An error occurred whilst attempting to connect to SimpleDatabase!");
                databaseBuilder.Build();
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return new SqlConnection(connectionString);
        }
    }
}
