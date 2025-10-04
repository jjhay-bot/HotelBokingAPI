# Entity Relationship Diagram (ERD)

This file documents the current ERD for the Hotel Booking API project. Update this diagram as you add or modify entities and relationships.

## Current ERD (as of October 4, 2025)

```
+-----------+        +-----------+        +-----------+        +-----------+        +-----------+
|   User    |        |   Room    |        | RoomType  |        | Booking   |        | Gallery   |
+-----------+        +-----------+        +-----------+        +-----------+        +-----------+
| Id (PK)   |        | Id (PK)   |        | Id (PK)   |        | Id (PK)   |        | Id (PK)   |
| FirstName |        | RoomTypeId|        | Name      |        | UserId (FK)|       | RoomId (FK)|
| LastName  |        | RoomNumber|        | Description|        | RoomId (FK)|       | Title     |
| Email     |        | PricePerNight|     |           |        | StartDate  |       | Img       |
| Age       |        | Capacity   |        +-----------+        | EndDate    |       | Alt       |
| Role      |        | BedType    |                             | Status     |       +-----------+
| IsActive  |        | Size       |                             | TotalPrice |
| PasswordHash|      | Floor      |                             | Notes      |
| CreatedAt |        | Status     |                             | CreatedAt  |
| UpdatedAt |        | Amenities  |                             | UpdatedAt  |
| CurrentRoomId|     | CreatedAt  |                             +-----------+
| RefreshToken|      | UpdatedAt  |
| RefreshTokenExpiry| | OccupiedByUserId|
+-----------+        +-----------+
```

### Relationships
- User (1) --- (M) Booking (UserId FK)
- Room (1) --- (M) Booking (RoomId FK)
- RoomType (1) --- (M) Room (RoomTypeId FK)
- Room (1) --- (M) Gallery (RoomId FK)
- Room (1) --- (M) User (CurrentRoomId, OccupiedByUserId)

---

*Update this file whenever you change the data model or add new entities/relationships.*
