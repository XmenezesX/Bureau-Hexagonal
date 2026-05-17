using BureauHexagonal.Application.Dtos.Output.Cep;
using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Ports;
using BureauHexagonal.Infrastructure.Adapters.Cep.Gateway;
using BureauHexagonal.Infrastructure.Adapters.Cep.Gateway.Response;
using BureauHexagonal.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BureauHexagonal.Infrastructure.Adapters.Cep
{
    public sealed class SearchViaCepAdapter(ILogger<SearchViaCepAdapter> _logger,
                                            IServiceProvider _serviceProvider) : ISearchCepPort
    {
        public async Task<IOperation<CepSearchResponseDto>> SearchAsync(string zipCode)
        {
            var viaCepGateway = _serviceProvider.GetRequiredService<IViaCepGateway>();
            var result = await viaCepGateway.GetAddress(zipCode);

            if (result.IsErrorResponseApi())
            { 
                var notifcation = NotificationErrors.Create(nameof(result), message: result.Content ?? "Content vazio", DefaultMessagesErrors.ProviderError);
                return OperationFactory.CreateFail<CepSearchResponseDto>(notifcation, ErrorType.ProviderError);
            }

            var responseGateway = JsonSerializer.Deserialize<ViaCepResponse>(result.Content!)!;

            var response = new CepSearchResponseDto
            {
                ZipCode = zipCode,
                DDD = responseGateway.DDD,
                IbgeCode = responseGateway.Ibge,
                Neighborhood = responseGateway.Bairro,
                City = responseGateway.Localidade,
                Complement = responseGateway.Complemento,
                State = responseGateway.UF,
                Street = responseGateway.Logradouro,
                Number = "S/N",
                ResponseProvider = result.Content!,
                ProviderType = ProviderType.ViaCep,
            };

            return response.ToSuccess();
        }
    }
}
