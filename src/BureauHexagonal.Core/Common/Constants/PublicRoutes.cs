namespace BureauHexagonal.Core.Common.Constants
{
    public static class PublicRoutes
    {
        public const string Swagger = "/swagger";
        public const string SwaggerIndex = "index.html";
        public const string HealthCheck = "/health";
        
        public static readonly string[] All = [Swagger, SwaggerIndex, HealthCheck];
    }
}
