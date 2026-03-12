using Dapper;

public class UserService
{
    private readonly IConfiguration _configuration;
    private readonly DbConnectionFactory _factory;

    public UserService(IConfiguration configuration, DbConnectionFactory factory)
    {
        _configuration = configuration;
        _factory = factory;
    }

    public async Task<User> createUser(SignUpRequest request)
    {
        var user = new User
        {
            Email = request.Email.ToLower(),
            Password = HasherUtil.CreateHashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        using var conn = _factory.CreateConnection();
        var sql = $@"
INSERT INTO {Tables.Users.TableName}
    ({Tables.Users.Cols.Email}, {Tables.Users.Cols.Password}, {Tables.Users.Cols.CreatedAt}, {Tables.Users.Cols.UpdatedAt})
OUTPUT INSERTED.*
VALUES
    (@Email, @Password, @CreatedAt, @UpdatedAt);";

        return await conn.QuerySingleAsync<User>(sql, new
        {
            Email = user.Email,
            Password = user.Password,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        });
    }

    public async Task<User> getUserByEmail(string email)
    {
        using var conn = _factory.CreateConnection();
        var user = await conn.QueryFirstOrDefaultAsync<User>(
            $"SELECT * FROM {Tables.Users.TableName} WHERE {Tables.Users.Cols.Email} = @Email",
            new { Email = email }
        );
        return user;
    }
}