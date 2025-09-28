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

9. **Set Up UsersController with Dependency Injection**
   - Created `UsersController` and injected `ApiContext` via the constructor.
   - Added explanatory comments about dependency injection and how the controller communicates with the database using Entity Framework Core.
   - Next: Implement CRUD endpoints for users.

10. *(Add your next steps here as you progress!)*

---

## Notes on Uniqueness, Indexes, and Database Communication (Sept 28, 2025)

### EF Core Unique Index/Constraint vs. Stored Procedure

- **EF Core Unique Index/Constraint:**
  - Enforced in `ApiContext` using `OnModelCreating`:
    ```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
    ```
  - Guarantees uniqueness at the database level for the `Email` column.
  - Only one definition needed (in C# context), and enforced for all inserts/updates.
  - Fast, safe, and automatic—no risk of duplicates, even with concurrent requests.
  - On insert/update, the DB checks the index; on GET, the index is not used for uniqueness.

- **Stored Procedure Approach:**
  - Logic is in the database as a procedure, e.g.:
    ```sql
    DELIMITER //
    CREATE PROCEDURE RegisterUser(IN p_email VARCHAR(255), IN p_password VARCHAR(255))
    BEGIN
        IF EXISTS (SELECT 1 FROM Users WHERE Email = p_email) THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Email already registered.';
        ELSE
            INSERT INTO Users (Email, Password) VALUES (p_email, p_password);
        END IF;
    END //
    DELIMITER ;
    ```
  - Must always use the procedure for inserts, or risk duplicates.
  - Not as safe unless combined with a unique index.
  - Slightly more overhead (SELECT + INSERT), and more maintenance.

### Where is the Unique Index Stored?
- The unique index is a data structure in the database, not in your C# code or as a visible column.
- You can see it in MySQL with:
  ```sql
  SHOW INDEX FROM Users;
  ```
- The index is created by EF Core migrations and is managed by the DB engine.

### Example: Enforcing Unique Email in EF Core

**ApiContext.cs:**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
}
```

**Migration:**
```sh
dotnet ef migrations add AddUniqueIndexToUserEmail
dotnet ef database update
```

**User Creation (C#):**
```csharp
var user = new User { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Age = 30 };
_context.Users.Add(user);
await _context.SaveChangesAsync(); // Throws if email is duplicate
```

### Summary Table
| Method   | Unique Index Effect                |
|----------|------------------------------------|
| Create   | Enforces uniqueness on insert      |
| Get      | No effect (may speed up lookups)   |
| Update   | Enforces uniqueness on email change|
| Delete   | No effect (index is updated)       |

---

## Index & Query Quick Reference

| Field      | Indexed? | Type         | Notes                                 |
|------------|----------|--------------|---------------------------------------|
| Id         | Yes      | Primary Key  | Always indexed by default             |
| Email      | Yes      | Unique Index | Fast lookups, enforces uniqueness     |
| Age        | Yes      | Index        | Fast filtering/sorting by age         |
| FirstName  | No       | -            | Slower for search/sort (no index)     |
| LastName   | No       | -            | Slower for search/sort (no index)     |

- Only indexed fields provide fast search/sort for large tables.
- Adding an index speeds up reads but adds slight overhead to writes.
- Unique indexes (like on Email) prevent duplicate values.
- Non-indexed fields are fine for small tables or infrequent queries.

---

## Quick Reference: EF Core Migration Workflow

> **Typical steps for updating your database schema:**
>
> 1. **Make changes** to your models or context (e.g., add a property, index, or entity).
> 2. **Create a migration** with a descriptive name:
>    ```sh
>    dotnet ef migrations add <MigrationName>
>    ```
>    - Replace `<MigrationName>` with a name that describes your change (e.g., `AddAgeIndexToUser`).
> 3. **Apply the migration** to your database:
>    ```sh
>    dotnet ef database update
>    ```
>
> - The migration name is up to you—choose something meaningful for your team and future self.
> - You can view all migrations with `dotnet ef migrations list`.

---

## Tips for Easier Reading

- **Skim headings and code blocks** for quick context.
- **Look for callouts and bullet points** for key info.
- **Use the Quick Reference above** for common commands and workflow.
- **Refer to the Important Notes section** for best practices and gotchas.

---

## MySQL Quick Reference

- **Open MySQL CLI:**
  ```sh
  mysql -u root -p
  ```
  (Enter your password when prompted)

- **Connect to your database:**
  ```sql
  USE hotelbooking;
  ```

- **Show all databases:**
  ```sql
  SHOW DATABASES;
  ```

- **Show all tables:**
  ```sql
  SHOW TABLES;
  ```

- **Show table structure:**
  ```sql
  DESCRIBE Users;
  ```

- **Find duplicate emails:**
  ```sql
  SELECT Email, COUNT(*) as Count FROM Users GROUP BY Email HAVING Count > 1;
  ```

- **Delete duplicate emails (keep lowest Id):**
  ```sql
  DELETE u1 FROM Users u1 JOIN Users u2 ON u1.Email = u2.Email AND u1.Id > u2.Id;
  ```

- **Show all indexes on a table:**
  ```sql
  SHOW INDEX FROM Users;
  ```

---

## Deployment Procedure: Neon PostgreSQL & Azure Web App

### 1. Deploying Database to Neon PostgreSQL

1. **Create a Neon PostgreSQL project** (if not already done) at [Neon.tech](https://neon.tech/).
2. **Get your connection string** from the Neon dashboard (format: `Host=...;Username=...;Password=...;Database=...`).
3. **Update your local `appsettings.Development.json`** with the Neon connection string for development/testing.
4. **Set the connection string as an environment variable** in Azure (see below for details).
5. **Apply EF Core migrations to Neon:**
   - Locally, run:
     ```sh
     dotnet ef database update --connection "<your-neon-connection-string>"
     ```
   - Or, enable automatic migrations on app startup in `Program.cs` (for small projects):
     ```csharp
     using (var scope = app.Services.CreateScope())
     {
         var db = scope.ServiceProvider.GetRequiredService<ApiContext>();
         db.Database.Migrate();
     }
     ```

### 2. Deploying API to Azure Web App

1. **Create an Azure App Service** (Web App) for your API.
2. **Publish your .NET API** to Azure (via Visual Studio, VS Code, or CLI):
   - Example using CLI:
     ```sh
     dotnet publish -c Release
     # Deploy using Azure CLI or from the Azure Portal
     ```
3. **Set environment variables in Azure:**
   - Go to your App Service → Configuration → Application settings.
   - Add a new setting:
     - Name: `ConnectionStrings__DefaultConnection`
     - Value: (your Neon PostgreSQL connection string)
   - Save and restart the app.
4. **Verify deployment:**
   - Test `/health` and `/api/v1/users` endpoints on your Azure URL.
   - If you see errors, check logs in Azure Portal (App Service → Log stream).

### 3. Tips
- Never commit secrets or connection strings to GitHub.
- Use environment variables for production secrets.
- Document any manual steps or gotchas here for future reference.

---

## JWT Claims: Important Notes (Simple)

- Only add user info to the JWT if the client or other services really need it.
- More claims = bigger tokens (slower network, more data sent).
- Never put sensitive info (like passwords) in the token.
- If user data changes (like name), the token won't update until the user logs in again.
- Use claims for things like user id, email, and role. For other info, fetch from the API if needed.

---

## Auth & JWT Implementation Flow (Technical)

1. **Registration Endpoint (`/api/v1/auth/register`)**
   - Accepts user info (email, password, etc.) in a POST request.
   - Checks if the email already exists.
   - Hashes the password using ASP.NET Core Identity's `PasswordHasher<T>` (PBKDF2, salted).
   - Saves the new user (with hashed password) to the database.

2. **Login Endpoint (`/api/v1/auth/login`)**
   - Accepts email and password in a POST request.
   - Looks up the user by email.
   - Verifies the password using `PasswordHasher<T>.VerifyHashedPassword`.
   - If valid, generates a JWT token with claims (user id, email, role, etc.).
   - Returns the JWT to the client.

3. **JWT Token Generation**
   - Uses a secret key from configuration (never hard-coded).
   - Sets claims: user id (`sub`), email, role, and any other needed info.
   - Sets expiry (e.g., 60 minutes).
   - Signs the token with HMAC SHA256.

4. **Protecting Endpoints**
   - Add `[Authorize]` or `[Authorize(Roles = "Admin")]` to controllers/actions.
   - The API checks for a valid JWT in the `Authorization: Bearer <token>` header.
   - If valid, the user's claims are available in `User.Claims`.
   - If not valid or missing, returns 401 Unauthorized.

5. **Security Best Practices**
   - Passwords are always hashed, never stored as plain text.
   - JWT secret key is stored in environment variables/config, not in code.
   - Only include non-sensitive info in JWT claims.
   - Use HTTPS in production to protect tokens in transit.

---

## Auth & JWT: Blocked User Handling

- Blocked users (IsActive = false) cannot perform mutation requests (POST, PUT, PATCH, DELETE), even if their JWT is valid.
- Blocked users can still access GET/read-only endpoints.
- This is enforced by custom middleware, which checks the user's status for each mutation request.
- To block a user instantly, set IsActive = false in the database; they will be denied on their next mutation request.

---

## Common Pitfalls & Important Notes

- Always add `app.UseAuthorization();` after `app.UseAuthentication();` in `Program.cs` to enable role-based and policy-based authorization.
- If `app.UseAuthorization();` is missing, all `[Authorize]` attributes will be ignored and protected endpoints will not work as expected (will always return 401).
- Make sure your JWT config (key, issuer, audience) matches between your appsettings and the token generator.
- Always use a fresh JWT after changing the signing key or restarting the app.
- Double-check the Authorization header format: `Authorization: Bearer <token>`.
- Use logging in JWT middleware to debug authentication failures.

---

## Major Issues & Solutions (PostgreSQL, EF Core, API)

### Amenities Type Migration (text[] → string)
- **Problem:**
  - Originally, the `Amenities` property in the `Room` model was a `List<string>`, which mapped to a `text[]` column in PostgreSQL.
  - For EF Core seeding and simpler API design, we switched `Amenities` to a `string` (comma-separated values) in the model and database.
  - This required a migration to change the column type from `text[]` to `text`.
- **Migration Steps:**
  1. Changed the model property to `public string Amenities { get; set; }`.
  2. Created a migration to alter the column type.
  3. Fixed seeding to use static `DateTime` values (no `DateTime.UtcNow` in `HasData`).
  4. Dropped and recreated the database to resolve duplicate key and type mismatch errors.
  5. Reapplied all migrations and seed data.
- **API Impact:**
  - API accepts and stores amenities as a comma-separated string.
  - API responses expose amenities as a `List<string>` (split in the DTO mapping).
  - PATCH endpoint accepts amenities as a list and converts to string for storage.

### RoomType in Room API Responses
- **Current:**
  - The API returns `roomTypeId` in each room response.
  - The full `RoomType` object is not included by default.
  - The frontend should fetch `/api/v1/roomtypes` and join by `roomTypeId` as needed.
- **Alternative:**
  - To embed the full `RoomType` in each room response, update the DTO and controller to include and map the related entity.

### Common Migration Pitfalls
- Using dynamic values (e.g., `DateTime.UtcNow`) in `HasData` causes EF Core to see the model as always changing. Always use static values for seeding.
- If you get duplicate key errors on seeding, drop and recreate the database in dev, or clean up the data before applying migrations.
- After dropping the database, you may need to manually recreate it in managed services (e.g., Neon) before running `dotnet ef database update`.

### HTTP API Sample Notes
- For Room creation, `amenities` must be a comma-separated string in the request body.
- For PATCH, you can send amenities as a list; the backend will convert it to a string for storage.
- API responses always return amenities as a list for frontend convenience.

---

_Continue to update this file as you make progress in your project._
