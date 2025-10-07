# 8. Adding Authentication (Optional)

For securing your API, add JWT-based authentication.

## Installing Packages

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package Microsoft.AspNetCore.Identity
```

## Configuring JWT in Program.cs

Add JWT settings to `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHere12345678901234567890",
    "Issuer": "MyApi",
    "Audience": "MyApiUsers"
  }
}
```

Update `Program.cs`:

```csharp
// ...existing code...

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
});

builder.Services.AddAuthorization();

// ...existing code...

app.UseAuthentication();
app.UseAuthorization();

// ...existing code...
```

## Protecting Endpoints

Add `[Authorize]` to controllers or actions:

```csharp
[HttpPost]
[Authorize]
public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
{
    // ...code...
}
```

## Creating a Login Endpoint

Add a simple auth controller for demo purposes.

This is a basic setup. For production, use proper user management with Identity.

## Summary

You've built a complete CRUD API! Key takeaways:

- ASP.NET Core for web APIs
- EF Core for data access
- DTOs and AutoMapper for clean architecture
- Migrations for schema management
- Optional auth for security
