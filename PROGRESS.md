# Project Progress Log

This markdown file will be used to document the step-by-step progress of the project, including major changes, decisions, and milestones.

## Progress Steps

1. **Project Initialization**
   - Repository created
   - .gitignore configured for .NET Core microservices

2. **Added Health Check Endpoint**
   - Implemented a basic `/health` endpoint returning `{ status: "Healthy" }` for service monitoring and readiness checks.
   - Added a sample request to `HotelBooking.Api.http` for easy testing.

3. **Set Up MySQL Database Connection**
   - Installed Pomelo.EntityFrameworkCore.MySql NuGet package for MySQL support.
   - Added a MySQL connection string to `appsettings.Development.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;port=3306;database=hotelbooking;user=root;password=yourpassword"
     }
     ```
   - Registered the DbContext in `Program.cs` using the connection string.
   - Ensured MySQL server is running locally (e.g., with `brew services start mysql` on macOS).

4. **Created User Entity and Added to DbContext**
   - Created a `User` class with properties: `Id`, `FirstName`, `LastName`, and `Age`.
   - Added `public DbSet<User> Users { get; set; }` to `ApiContext` for EF Core to manage the Users table.
   - Used the standard `{ get; set; }` property pattern for compatibility with EF Core.

5. **Created Initial Migration for Users Table**
   - Ran `dotnet ef migrations add InitialCreate` to scaffold the first migration for the Users table.
   - Ran `dotnet ef database update` to apply the migration and create the Users table in the MySQL database.
   - **Note:**
     - Migrations are used to incrementally update the database schema as your models change.
     - The first migration sets up the initial database structure based on your current DbContext and entities.
     - Future migrations will track and apply changes as you add or modify entities.

6. **Applying Migrations to the Database**
   - After creating or updating migrations, always run `dotnet ef database update` to apply changes to the database.
   - This command creates the database (if it doesn't exist) and updates the schema to match your current models and migrations.
   - If you add new entities or change your models, repeat this process: create a migration, then update the database.

7. **Added RoomType Entity**
   - Created a `RoomType` class with properties: `Value` (PK) and `Label`.
   - Added `public DbSet<RoomType> RoomTypes { get; set; }` to `ApiContext`.
   - Next: Create a migration and update the database to add the RoomTypes table.

8. **Implemented User CRUD Endpoints**
   - Created `UsersController` with endpoints for:
     - Get all users (`GET /api/users`)
     - Get user by ID (`GET /api/users/{id}`)
     - Create user (`POST /api/users`)
     - Update user (`PUT /api/users/{id}`)
     - Delete user (`DELETE /api/users/{id}`)
   - Used EF Core async methods for database operations.
   - Next: Test endpoints and add DTOs/validation as needed.

9. *(Add your next steps here as you progress!)*

---

_Continue to update this file as you make progress in your project._
