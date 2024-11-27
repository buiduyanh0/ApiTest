﻿using ApiTest2;
using Microsoft.AspNetCore.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
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
            // Configure OAuth
            var oauthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true, // Use HTTPS in production
                TokenEndpointPath = new Microsoft.Owin.PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // Configure Web API
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }

    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); // Validate all clients (can be customized)
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Replace this with your user validation logic
            if (context.UserName != "test" || context.Password != "password")
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            context.Validated(identity); // Generate the token
        }
    }
}
