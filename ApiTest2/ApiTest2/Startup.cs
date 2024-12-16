using ApiTest2;
using Microsoft.AspNetCore.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(ApiTest2.Startup))]
namespace ApiTest2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Web API configuration and services
            HttpConfiguration config = new HttpConfiguration();

            // Configure JWT Authentication
            ConfigureAuth(config, app);

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Use Web API with Owin
            app.UseWebApi(config);
        }

        public void ConfigureAuth(HttpConfiguration config, IAppBuilder app)
        {
            var issuer = "your-issuer"; // Set your JWT issuer
            var audience = "your-audience"; // Set your JWT audience
            var secretKey = "L40/p+SbAVQGchV1pOY2qxaydhYfB0MNvZlSfwB9Kd2vxbdfHrGuPfEHRkD0CeIy"; // The secret key used to sign JWT tokens

            // Define JWT Bearer Authentication options
            var jwtBearerAuthenticationOptions = new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audience },
                IssuerSecurityKeyProviders = new[] { new SymmetricKeyIssuerSecurityKeyProvider(issuer, secretKey) }
            };

            // Enable JWT Bearer Authentication
            app.UseJwtBearerAuthentication(jwtBearerAuthenticationOptions);
        }
    }
}
