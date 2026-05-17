using BureauHexagonal.Application.Dtos.Output.Cep;
using BureauHexagonal.Core.Common.Operation;

namespace BureauHexagonal.Core.Ports
{
    public interface ISearchCepPort
    {
        Task<IOperation<CepSearchResponseDto>> SearchAsync(string zipCode);
    }
}
