# 7. Running and Testing the API

## Running the API

1. **Build and Run**:

   ```bash
   dotnet build
   dotnet run
   ```

2. **Access Swagger UI**:
   Open `https://localhost:5001/swagger` (or the URL shown in console).

## Testing Endpoints

Use Swagger or tools like Postman/cURL:

- **GET /api/books**: List all books
- **POST /api/books**: Create book (send JSON body)
- **GET /api/books/1**: Get book by ID
- **PUT /api/books/1**: Update book
- **DELETE /api/books/1**: Delete book

Example POST body:

```json
{
  "title": "New Book",
  "author": "Author Name",
  "isbn": "1234567890123",
  "publishedDate": "2023-01-01T00:00:00Z",
  "price": 29.99
}
```

## Debugging

- Check console for errors.
- Use breakpoints in VS Code.
- Verify database connection.

## Next Step

[Adding Authentication (Optional)](08-Authentication.md)
