# .NET API Improvements Checklist

This checklist outlines recommended improvements for the Hotel Booking API to align with .NET standards and best practices. We'll tackle them one by one.

## 1. Input Validation

- [ ] Add validation attributes to DTOs (e.g., `[Required]`, `[EmailAddress]`, `[MinLength]`)
  - `LoginRequestDto.cs`: Email and Password
  - `RegisterRequestDto.cs`: All fields
  - `RoomDto.cs`, `UserDto.cs`, etc.
- [ ] Ensure `[ApiController]` enables automatic 400 responses for invalid models

## 2. Global Error Handling

- [ ] Implement global exception middleware in `Program.cs`
- [ ] Add consistent error response format (e.g., `{ "error": "message", "code": 500 }`)
- [ ] Handle specific exceptions (e.g., DbUpdateException, UnauthorizedAccessException)

## 3. Logging

- [ ] Integrate structured logging (e.g., Serilog or Microsoft.Extensions.Logging)
- [ ] Add logging in controllers and services for requests, errors, and key events
- [ ] Configure log levels and outputs (console, file, etc.)

## 4. Unit Tests

- [ ] Set up xUnit or NUnit in `Api.Tests/`
- [ ] Write tests for controllers (e.g., AuthController login/register)
- [ ] Add tests for services, DTO mapping, and business logic
- [ ] Integrate test coverage tools (e.g., Coverlet)

## 5. API Versioning

- [ ] Add Microsoft.AspNetCore.Mvc.Versioning package
- [ ] Implement versioning (e.g., `/api/v1/...` and `/api/v2/...`)
- [ ] Update controllers with `[ApiVersion]` attributes

## 6. Security Enhancements

- [ ] Add rate limiting (AspNetCoreRateLimit package)
- [ ] Implement input sanitization for user inputs
- [ ] Review and strengthen CORS policy if needed
- [ ] Consider API keys for sensitive admin endpoints

## 7. Performance Optimizations

- [ ] Enable response compression (`app.UseResponseCompression()`)
- [ ] Add caching (e.g., in-memory or Redis) for frequently accessed data
- [ ] Optimize EF Core queries (e.g., use `.AsNoTracking()` where appropriate)

## 8. Code Quality & Standards

- [ ] Add code analysis tools (StyleCop, Roslyn analyzers)
- [ ] Ensure consistent naming conventions (PascalCase, camelCase)
- [ ] Add XML documentation comments to public methods
- [ ] Review and refactor for SOLID principles

## 9. Additional Features (Optional)

- [ ] Add pagination metadata in responses
- [ ] Implement HATEOAS links in API responses
- [ ] Add API monitoring/metrics (e.g., Application Insights)

## Progress Tracking

- **Started**: [Date]
- **Completed**: [List items as done]
- **Next Priority**: [Current focus item]

Last Updated: October 7, 2025
