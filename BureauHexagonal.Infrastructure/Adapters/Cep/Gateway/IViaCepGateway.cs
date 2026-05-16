using Refit;

namespace BureauHexagonal.Infrastructure.Adapters.Cep.Gateway
{
    public interface IViaCepGateway
    {
        [Get("/ws/{zipCode}/json/")]
        Task<ApiResponse<string>> GetAddress([AliasAs("zipCode")] string zipCode);
    }
}
