public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserService _userService;
    public AuthService(IConfiguration configuration, UserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }


    public async Task<SignUpResponse> SignUp(SignUpRequest request)
    {
        var isExist = await _userService.getUserByEmail(request.Email.ToLower());
        if (isExist != null)
        {
            throw new Exception("User already exists");
        }
        var user = await _userService.createUser(request);
        return new SignUpResponse
        {
            Email = user.Email,
            Token = new JWTUtil(_configuration).GenerateToken(user),
        };
    }

    public async Task<LogInResponse> Login(LogInRequest request)
    {
        var user = await _userService.getUserByEmail(request.Email.ToLower());
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");
        if (!HasherUtil.VerifyPassword(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid email or password");
        return new LogInResponse
        {
            Email = user.Email,
            Token = new JWTUtil(_configuration).GenerateToken(user),
        };
    }
}