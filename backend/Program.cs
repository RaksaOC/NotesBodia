using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var CorsOrigins = "AllowAllOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsOrigins,
                      policy =>
                      {
                          // allow all origins for now
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// default .net services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// db connection
builder.Services.AddSingleton<DbConnectionFactory>();

// main services
builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    var key = builder.Configuration["Jwt:Key"];

    if (string.IsNullOrEmpty(key))
    {
        throw new Exception("Jwt:Key is not set");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Use authentication and authorization (middlewares)
app.UseCors(CorsOrigins);

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();


// app start
app.Run();
