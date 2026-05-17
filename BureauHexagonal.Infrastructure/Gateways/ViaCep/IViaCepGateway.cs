using Refit;

namespace BureauHexagonal.Infrastructure.Gateways.ViaCep
{
    public interface IViaCepGateway
    {
        [Get("/ws/{zipCode}/json/")]
        Task<ApiResponse<string>> GetAddress([AliasAs("zipCode")] string zipCode);
    }
}
