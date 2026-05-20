using Refit;

namespace BureauHexagonal.Infrastructure.Gateways.BrasilApi
{
    public interface IBrasilApiGateway
    {
        [Get("/api/cep/v2/{zipCode}")]
        Task<ApiResponse<string>> GetAddress([AliasAs("zipCode")] string zipCode);
    }
}
