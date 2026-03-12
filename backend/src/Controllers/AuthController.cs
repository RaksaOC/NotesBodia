using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var response = await _authService.SignUp(request);
        return Ok(ApiResponseUtil.Success(response));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LogInRequest request)
    {
        var response = await _authService.Login(request);
        return Ok(ApiResponseUtil.Success(response));
    }
}