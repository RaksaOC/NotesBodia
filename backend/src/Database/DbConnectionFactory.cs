using Microsoft.Data.SqlClient;
using System.Data;

public class DbConnectionFactory
{
    private readonly IConfiguration _config;

    public DbConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = _config.GetConnectionString("DefaultConnection");
        return new SqlConnection(connectionString);
    }
}