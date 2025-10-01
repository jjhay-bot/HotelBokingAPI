# Entity Relationship Diagram (ERD)

This file documents the current ERD for the Hotel Booking API project. Update this diagram as you add or modify entities and relationships.

## Current ERD (as of September 28, 2025)

```
+-----------+        +-----------+
|   User    |        | RoomType  |
+-----------+        +-----------+
| Id (PK)   |        | Id (PK)   |
| FirstName |        | Name      |
| LastName  |        | Description|
| Age       |        +-----------+
+-----------+
```

- `User` and `RoomType` are currently independent tables.
- Add relationships here as you expand the data model (e.g., Bookings, Rooms, etc.).

---

*Update this file whenever you change the data model or add new entities/relationships.*
