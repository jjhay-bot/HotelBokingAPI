# Hotel Booking API

A simple .NET Core API for managing hotel bookings, users, rooms, and room types.

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
