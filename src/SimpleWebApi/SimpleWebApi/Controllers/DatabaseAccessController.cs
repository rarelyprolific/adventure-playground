using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SimpleWebApi.Database;

namespace SimpleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseAccessController : ControllerBase
    {
        private readonly ILogger<DatabaseAccessController> logger;
        private readonly ISqlConnectionBuilder sqlConnectionBuilder;

        public DatabaseAccessController(
            ILogger<DatabaseAccessController> logger,
            ISqlConnectionBuilder sqlConnectionBuilder)
        {
            this.logger = logger;
            this.sqlConnectionBuilder = sqlConnectionBuilder;
        }

        [HttpGet]
        public string Get()
        {
            using (var connection = sqlConnectionBuilder.Build())
            {
                // Create a query that retrieves all books with an author name of "John Smith"    
                var sql = "SELECT * FROM Books WHERE Author = @authorName";

                // Use the Query method to execute the query and return a list of objects    
                var books = connection.Query<string>(sql, new { authorName = "John Smith" }).ToList();

                return "OK!";
            }
        }
    }
}
