using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{

    public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            return AuthenticateResult.NoResult();
        }

        var token = authHeader.Substring("Bearer ".Length);

        var auth = FirebaseAuth.DefaultInstance;
        if (auth != null)
        {
            try
            {
                var decodedToken = await auth.VerifyIdTokenAsync(token);
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid)
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                
                return AuthenticateResult.Success(ticket);
            }
            catch (FirebaseAuthException)
            {
                return AuthenticateResult.Fail("Invalid token");
            }
        }

        return AuthenticateResult.NoResult();
    }
}