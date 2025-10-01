using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Api.Middleware
{
  public class BlockedUserMiddleware
  {
    private readonly RequestDelegate _next;

    public BlockedUserMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApiContext dbContext)
    {
      // Only check for authenticated users and mutation requests
      var mutationMethods = new[] { "POST", "PUT", "PATCH", "DELETE" };
      if (context.User.Identity?.IsAuthenticated == true && mutationMethods.Contains(context.Request.Method))
      {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                          context.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
          var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
          if (user == null || !user.IsActive)
          {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("User is blocked or does not exist.");
            return;
          }
        }
      }
      await _next(context);
    }
  }
}
