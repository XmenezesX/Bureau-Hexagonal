using BureauHexagonal.Application.Dtos.Cep;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Enums;

namespace BureauHexagonal.Core.Ports
{
    public interface ISearchCepPort
    {
        bool CanHandle(ProviderType providerType);

        Task<IOperation<CepSearchResponseDto>> SearchAsync(string zipCode);
    }
}
