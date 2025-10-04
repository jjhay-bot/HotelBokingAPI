# JWT Cookie Authentication: Important Notes

## Cookie Security
- Use `HttpOnly = true` so JavaScript cannot access the cookie.
- Use `Secure = true` in production (only send cookies over HTTPS).
- Use `SameSite = None` for cross-site authentication (frontend and backend on different domains).
- Always set `Secure = true` when using `SameSite = None`.

## CORS Configuration
- Backend must allow credentials in CORS (`AllowCredentials = true`).
- Frontend must send requests with credentials (`credentials: 'include'` for fetch, `withCredentials: true` for Axios).

## Logout Endpoint
- Implement an endpoint to clear the JWT cookie (e.g., `Response.Cookies.Delete("jwt")`).

## Token Expiry Handling
- Handle token expiration gracefully on the frontend (e.g., redirect to login if unauthorized).

## Cookie Domain and Path
- If deploying to multiple subdomains, set the cookie domain appropriately.

## Frontend
- The frontend does not need to manage the token, but should handle 401 responses and trigger re-authentication.

## Authentication Middleware
- Configure JWT authentication to read the token from the cookie (using `OnMessageReceived` event).

## Cross-Site Cookies
- Cross-site cookies are sent with requests from one domain to another (e.g., frontend.com to api.com).
- Browsers restrict cross-site cookies for security; use `SameSite = None` and `Secure = true` to allow them.

## Environment Variable Usage for Cookie Security

- The `Secure` property of authentication cookies is controlled by the `COOKIE_SECURE` environment variable.
- For local development, set `COOKIE_SECURE=false` (allows cookies over HTTP).
- For production, set `COOKIE_SECURE=true` (requires HTTPS).
- On macOS/Linux, set in terminal: `export COOKIE_SECURE=false`
- On Windows, set in terminal: `set COOKIE_SECURE=false`
- You can also use a `.env` file with `COOKIE_SECURE=false` for local dev (use a library like DotNetEnv to load it).
- In Azure App Service, set `COOKIE_SECURE=true` in Application Settings.
- To verify the value, run `echo $COOKIE_SECURE` in your terminal or log it in your app.

---

# Refresh Token & JWT Cookie Authentication: Key Implementation Notes

## Backend Responsibilities
- The backend generates, stores, rotates, and validates refresh tokens as HTTP-only cookies.
- The backend issues new JWTs when the `/refresh` endpoint is called with a valid refresh token cookie.
- The backend does not automatically refresh tokens; it only responds to requests.

## Frontend Responsibilities
- The frontend does not need to read, store, or manage the refresh token value.
- The frontend should make API calls as usual and handle 401 Unauthorized responses.
- When a 401 is received (JWT expired), the frontend should call the `/refresh` endpoint to obtain a new JWT. The refresh token cookie is sent automatically by the browser/client.
- If `/refresh` also returns 401 (refresh token expired/invalid), prompt the user to log in again.

## Security Best Practices
- Use short-lived JWTs (e.g., 5–15 minutes) since refresh tokens provide session continuity.
- Invalidate all refresh tokens for a user on password change or account compromise.
- Always use `HttpOnly` and `Secure` cookies for refresh tokens.
- Rotate refresh tokens on every use to prevent replay attacks.

## Migration Reminder
- If you add `RefreshToken` and `RefreshTokenExpiry` to your user model, run a new migration and update your database schema.

## Testing
- Use tools that support cookies (Postman, VS Code REST Client) for manual API testing.
- The refresh token is set as a cookie after login and sent automatically with requests.

## Error Handling
- Return clear error messages for invalid or expired tokens.
- Frontend should handle these errors and prompt for re-authentication if needed.

## Example Flow
1. User logs in: backend sets JWT and refresh token cookies.
2. JWT expires: frontend receives 401, calls `/refresh`.
3. Backend validates refresh token, issues new JWT and refresh token cookies.
4. If refresh token is invalid/expired, frontend prompts user to log in again.

---

# Secure JWT Cookie Handling Guide

## Setting JWT and Refresh Token as HttpOnly Cookies
- Always set cookies with `HttpOnly`, `Secure`, and `SameSite=None` for cross-site authentication.
- Example in ASP.NET Core:

```csharp
var cookieOptions = new CookieOptions {
    HttpOnly = true,
    Secure = true, // or use env var
    SameSite = SameSiteMode.None,
    Path = "/",
    Expires = DateTime.UtcNow.AddMinutes(jwtExpiryMinutes)
};
Response.Cookies.Append("jwt", token, cookieOptions);
Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
```

## Deleting JWT and Refresh Token Cookies
- To delete cookies in the browser, use the **same attributes** as when setting them (especially `Path`, `SameSite`, and `Secure`).
- Example in ASP.NET Core:

```csharp
var cookieOptions = new CookieOptions {
    HttpOnly = true,
    Secure = true, // or use env var
    SameSite = SameSiteMode.None,
    Path = "/"
};
Response.Cookies.Delete("jwt", cookieOptions);
Response.Cookies.Delete("refreshToken", cookieOptions);
```

## Why Matching Attributes Matter
- If you set cookies with custom attributes, you must use the same attributes to delete them. Otherwise, the browser will not remove the cookie.
- This is especially important for authentication cookies in cross-site scenarios.

## Frontend Notes
- HttpOnly cookies are **not accessible via JavaScript**. The browser sends them automatically with requests.
- To ensure cookies are sent, use `credentials: 'include'` in fetch/XHR.

---

**Always match cookie attributes for secure authentication and proper logout!**

---

See above for additional cookie, CORS, and environment variable notes.

**Reference:**
- [MDN: SameSite cookies](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie/SameSite)
- [ASP.NET Core Docs: Cookie authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie)

## CSRF Middleware Logic (Double-Submit Cookie)

- CSRF validation is enforced for all state-changing requests (POST, PUT, PATCH, DELETE).
- The middleware checks that the `XSRF-TOKEN` cookie matches the `X-CSRF-Token` header.
- CSRF validation is skipped for `/api/v1/auth/login` and `/api/v1/auth/register` endpoints to allow authentication without a CSRF token.
- If the tokens are missing or do not match, a 403 Forbidden response is returned.
- JWT authentication is handled via the `jwt` cookie.

Example code:
```csharp
app.Use(async (context, next) =>
{
    // Only check CSRF for state-changing requests and exclude auth endpoints
    if ((context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "PATCH" || context.Request.Method == "DELETE")
        && !(context.Request.Path.StartsWithSegments("/api/v1/auth/login") || context.Request.Path.StartsWithSegments("/api/v1/auth/register")))
    {
        var cookieToken = context.Request.Cookies["XSRF-TOKEN"];
        var headerToken = context.Request.Headers["X-CSRF-Token"].ToString();
        if (string.IsNullOrEmpty(cookieToken) || string.IsNullOrEmpty(headerToken) || cookieToken != headerToken)
        {
            context.Response.StatusCode = 403;
            await context.Response.CompleteAsync();
            return;
        }
    }
    await next();
});
```
