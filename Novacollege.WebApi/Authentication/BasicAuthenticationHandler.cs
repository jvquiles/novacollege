using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Novacollege.CrossCuttings;
using Novacollege.Data.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Novacollege.WebApi.Authentication;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    NovacollegeDbContext context) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly NovacollegeDbContext _context = context;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            return AuthenticateResult.Fail("Missing Authorization header");

        var authHeaderValue = authHeader.ToString();
        if (!authHeaderValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.Fail("Invalid Authorization header format");

        var credentials = Encoding.UTF8.GetString(
            Convert.FromBase64String(authHeaderValue[6..])).Split(':', 2);

        if (credentials.Length != 2)
            return AuthenticateResult.Fail("Invalid credentials format");

        var username = credentials[0];
        var password = credentials[1];

        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return AuthenticateResult.Fail("Invalid username or password");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        var hash = PasswordHelper.HashPassword(password);
        return hash == passwordHash;
    }
}