using Npgsql;
using System.Data;

public class DbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public DbConnectionFactory(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("ConnectionStrings:DefaultConnection is not set");

        // one pooled data source for the whole app
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public IDbConnection CreateConnection() => _dataSource.CreateConnection();

    /// Creates tables/trigger if they don't exist yet (DDL.sql is idempotent).
    public async Task EnsureSchemaAsync()
    {
        var ddlPath = Path.Combine(AppContext.BaseDirectory, "src", "Database", "sql", "DDL.sql");
        var ddl = await File.ReadAllTextAsync(ddlPath);
        await using var cmd = _dataSource.CreateCommand(ddl);
        await cmd.ExecuteNonQueryAsync();
    }
}
