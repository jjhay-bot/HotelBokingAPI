# 1. Project Setup

## Creating a New ASP.NET Core Web API Project

1. **Install .NET SDK**: Ensure you have .NET 8 installed. Check with `dotnet --version`.

2. **Create the Project**:

   ```bash
   dotnet new webapi -n MyApi
   cd MyApi
   ```

   This creates a basic Web API project with:
   - `MyApi.csproj`: Project file
   - `Program.cs`: Entry point
   - `Controllers/WeatherForecastController.cs`: Sample controller
   - `appsettings.json`: Configuration

3. **Add Entity Framework Core**:

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

4. **Add Other Useful Packages**:

   ```bash
   dotnet add package AutoMapper
   dotnet add package Swashbuckle.AspNetCore
   ```

5. **Project Structure**:
   After setup, your project should look like:

   ```text
   MyApi/
   ├── Controllers/
   ├── Models/
   ├── DTOs/
   ├── Data/
   ├── Properties/
   ├── appsettings.json
   ├── MyApi.csproj
   ├── Program.cs
   └── README.md
   ```

## Key Concepts

- **Program.cs**: Uses the "minimal API" style in .NET 6+. Services are registered here.
- **Controllers**: Handle HTTP requests and return responses.
- **Models**: Represent database entities.
- **DTOs**: Data Transfer Objects for API requests/responses.

## Next Step

[Creating Models](02-Creating-Models.md)
