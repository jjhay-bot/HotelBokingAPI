using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
    );

// Add controller support
builder.Services.AddControllers();

// JWT Authentication configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
    // Read JWT from cookie named "jwt"
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt"))
            {
                context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://bedderdeals.fun-at.work"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // <-- Add this line
    });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.Use(async (context, next) =>
{
    // Only check CSRF for state-changing requests and exclude auth endpoints
    if ((context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "PATCH" || context.Request.Method == "DELETE")
        && !(context.Request.Path.StartsWithSegments("/api/v1/auth/login") || context.Request.Path.StartsWithSegments("/api/v1/auth/register")))
    {
        var cookieToken = context.Request.Cookies["XSRF-TOKEN"];
        var headerToken = context.Request.Headers["X-CSRF-Token"].ToString();
        if (string.IsNullOrEmpty(cookieToken) || string.IsNullOrEmpty(headerToken) || cookieToken != headerToken)
        {
            context.Response.StatusCode = 403;
            await context.Response.CompleteAsync();
            return;
        }
    }
    await next();
});
app.UseAuthorization(); // <-- Ensure this is present and after UseAuthentication
// Map controller endpoints
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.Run();
