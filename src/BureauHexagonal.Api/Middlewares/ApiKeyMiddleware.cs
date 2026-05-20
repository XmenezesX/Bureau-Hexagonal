using BureauHexagonal.Core.Common.Constants;
using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Common.Operation;
using System.Net;
using System.Text.Json;

namespace BureauHexagonal.Api.Middlewares
{
    public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private const string APIKEYNAME = "x-api-key";

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsPublicRoute(context))
            {
                await next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                await ReturnUnauthorized(context, "API Key não fornecida.");
                return;
            }

            var apiKey = configuration.GetValue<string>("Authentication:ApiKey");

            if (string.IsNullOrWhiteSpace(apiKey) || !apiKey.Equals(extractedApiKey))
            {
                await ReturnUnauthorized(context, "API Key inválida.");
                return;
            }

            await next(context);
        }

        private static bool IsPublicRoute(HttpContext context)
        {
            var path = context.Request.Path;
            return path.StartsWithSegments(PublicRoutes.Swagger) ||
                   path.Value!.EndsWith(PublicRoutes.SwaggerIndex) ||
                   path.StartsWithSegments(PublicRoutes.HealthCheck);
        }

        private static async Task ReturnUnauthorized(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = HttpStatusCode.Unauthorized.GetHashCode();

            var notificationErrors = NotificationErrors.Create("ApiKey", message, message);
            var operationFail = OperationFactory.CreateFail(notificationErrors, ErrorType.BusinessError);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var result = JsonSerializer.Serialize(operationFail, options);
            await context.Response.WriteAsync(result);
        }
    }
}
