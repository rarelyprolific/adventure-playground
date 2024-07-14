using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace SimpleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseAccessController : ControllerBase
    {
        private readonly ILogger<DatabaseAccessController> logger;
        private readonly IConfiguration configuration;

        public DatabaseAccessController(
            ILogger<DatabaseAccessController> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            using (SqlConnection connection = new SqlConnection(configuration.GetValue<string>("DATABASE_CONNECTION_STRING")))
            {
                string dbName = connection.ExecuteScalar<string>("SELECT DB_NAME(db_id())");
                int tableRowCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM SimpleTodos");

                return $"The database name is '{dbName}' and there are {tableRowCount} rows in the SimpleTodos table.";
            }
        }
    }
}
