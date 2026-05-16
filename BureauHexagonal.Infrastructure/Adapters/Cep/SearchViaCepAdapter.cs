using BureauHexagonal.Application.Dtos.Cep;
using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Ports;
using BureauHexagonal.Infrastructure.Adapters.Cep.Gateway;
using BureauHexagonal.Infrastructure.Adapters.Cep.Gateway.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BureauHexagonal.Infrastructure.Adapters.Cep
{
    public sealed class SearchViaCepAdapter(ILogger<SearchViaCepAdapter> _logger,
                                            IServiceProvider _serviceProvider) : ISearchCepPort
    {
        public bool CanHandle(ProviderType providerType) => ProviderType.ViaCep == providerType;

        public async Task<IOperation<CepSearchResponseDto>> SearchAsync(string zipCode)
        {
            var viaCepGateway = _serviceProvider.GetRequiredService<IViaCepGateway>();
            var result = await viaCepGateway.GetAddress(zipCode);

            if (!result.IsSuccessStatusCode)
            {
                var notifcation = NotificationErrors.Create(nameof(result), message: result.Content ?? "Content vazio");
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
            };

            return response.ToSuccess();
        }
    }
}
