using Microsoft.Data.SqlClient;

namespace SimpleWebApi.Database
{
    public interface ISqlConnectionBuilder
    {
        Task<SqlConnection> BuildAsync();
    }
}