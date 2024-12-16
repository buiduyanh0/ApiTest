using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using System;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Filters;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ApiTest2
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new GlobalExceptionFilter());
        }
        public class GlobalExceptionFilter : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext context)
            {
                if (context.Exception is UnauthorizedAccessException)
                {
                    System.Diagnostics.Debug.WriteLine("Unauthorized access detected");
                }
                base.OnException(context);
            }
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var authHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Validate the token (example using JWT)
                var handler = new JwtSecurityTokenHandler();
                try
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var identity = new ClaimsIdentity(jwtToken.Claims, "Bearer");
                    HttpContext.Current.User = new ClaimsPrincipal(identity);
                }
                catch
                {
                    // Log token validation error
                }
            }
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception is System.Net.Http.HttpRequestException httpRequestException)
            {
                // Check if the error is due to a connection reset (ECONNRESET)
                if (httpRequestException.InnerException is System.Net.Sockets.SocketException socketException &&
                    socketException.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset)
                {
                    // Log the error details for further debugging
                    LogConnectionResetError(httpRequestException, socketException);

                    // Optional: Provide a custom response
                    Response.StatusCode = 500;
                    Response.Write("Connection was reset by the remote host.");
                    Response.End();
                }
                else
                {
                    // Handle other HttpRequestExceptions here
                    LogError(exception);
                }
            }
            else
            {
                // Handle non-http request errors
                LogError(exception);
            }

            // Clear the error and continue
            Server.ClearError();
        }

        // Method to log ECONNRESET errors
        private void LogConnectionResetError(Exception httpRequestException, System.Net.Sockets.SocketException socketException)
        {
            // Here you can log the error to a file, database, or a logging framework
            string logMessage = $"ECONNRESET Error: {httpRequestException.Message}\n" +
                                $"SocketException: {socketException.Message}\n" +
                                $"StackTrace: {httpRequestException.StackTrace}\n" +
                                $"Timestamp: {DateTime.Now}";

            // Example: Log to a file (ensure you have permission to write to the file path)
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt"), logMessage + Environment.NewLine);
        }

        // Generic error logging method
        private void LogError(Exception exception)
        {
            string logMessage = $"Error: {exception.Message}\nStackTrace: {exception.StackTrace}\nTimestamp: {DateTime.Now}";

            // Example: Log to a file (ensure you have permission to write to the file path)
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt"), logMessage + Environment.NewLine);
        }
    }
}
