public class LogInRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LogInResponse
{
    public string Email { get; set; }
    public string Token { get; set; }
}

public class SignUpRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignUpResponse
{
    public string Email { get; set; }
    public string Token { get; set; }
}