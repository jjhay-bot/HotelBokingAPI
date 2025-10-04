# Hotel Booking API

## Project Overview
A robust ASP.NET Core API for managing hotel bookings, users, rooms, room types, and galleries. Features include JWT authentication, CSRF protection, role-based access, advanced filtering, and more. Designed for modern web and mobile hotel management solutions.

## Tools and Frameworks Used
- **ASP.NET Core** (Web API)
- **Entity Framework Core** (ORM, PostgreSQL)
- **AutoMapper** (DTO mapping)
- **JWT Authentication** (Microsoft.IdentityModel.Tokens)
- **PasswordHasher** (ASP.NET Identity)
- **CORS** (Cross-Origin Resource Sharing)
- **Swagger/OpenAPI** (API documentation)
- **PlantUML** (ERD diagrams)
- **REST Client** (for .http file testing)

## Entity Relationship Diagram (ERD)

```
+-------------+       +----------------+       +-------------+
|   Users     |       |    Bookings    |       |    Rooms    |
+-------------+       +----------------+       +-------------+
| id (PK)     |----<--| id (PK)        |-->----| id (PK)     |
| firstName   |       | user_id (FK)   |       | roomType(FK)|
| lastName    |       | room_id (FK)   |       | pricePerNight|
| age         |       | check_in_date  |       | ...         |
+-------------+       | check_out_date |       +-------------+
                      | total_price    |              |
                      | status         |              |
                      +----------------+              |
                                                      |
                                              +-------------+
                                              |  RoomTypes  |
                                              +-------------+
                                              | value (PK)  |
                                              | label       |
                                              +-------------+
                                                      |
                                              +-------------+
                                              |   Gallery   |
                                              +-------------+
                                              | id (PK)     |
                                              | room_id(FK) |
                                              | ...         |
                                              +-------------+
  ```

## Main Entities

- **Users**: Hotel customers.
- **Bookings**: Reservations made by users for rooms.
- **Rooms**: Hotel rooms available for booking.
- **RoomTypes**: Types/categories of rooms (e.g., Single, Double, Suite).
- **Gallery**: Images associated with rooms.

## Getting Started

1. Clone the repository
2. Configure your environment variables and connection strings in `appsettings.Development.json`
3. Build and run the project:
   ```sh
   dotnet build
   dotnet run
   ```
4. API will be available at `https://localhost:5001` (or as configured)

## Progress
See `PROGRESS.md` for step-by-step project updates.

---

Feel free to contribute or suggest improvements!
