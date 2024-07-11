using Microsoft.Data.SqlClient;

namespace SimpleWebApi.Database
{
    public interface ISqlConnectionBuilder
    {
        SqlConnection Build();
    }
}