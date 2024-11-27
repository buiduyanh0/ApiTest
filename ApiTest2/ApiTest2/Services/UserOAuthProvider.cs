using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

public class UserOAuthProvider : OAuthAuthorizationServerProvider
{
    public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        context.Validated(); // Accept all clients (you can customize this)
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        // Mock user validation - replace with database or external service
        if (context.UserName != "testuser" || context.Password != "password123")
        {
            context.SetError("invalid_grant", "The username or password is incorrect.");
            return;
        }

        // Create a claims identity
        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
        identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
        identity.AddClaim(new Claim(ClaimTypes.Role, "User")); // Add user role

        context.Validated(identity); // Generate and return the token
    }
}
