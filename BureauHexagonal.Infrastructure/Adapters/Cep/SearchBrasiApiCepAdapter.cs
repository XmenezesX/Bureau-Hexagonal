using BureauHexagonal.Application.Dtos.Output.Cep;
using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Ports;
using BureauHexagonal.Infrastructure.Gateways.BrasilApi;
using BureauHexagonal.Infrastructure.Gateways.BrasilApi.Response;
using BureauHexagonal.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BureauHexagonal.Infrastructure.Adapters.Cep
{
    public sealed class SearchBrasiApiCepAdapter(ILogger<SearchViaCepAdapter> _logger,
                                                 IServiceProvider _serviceProvider) : ISearchCepPort
    {
        private const string NotSearch = "Não informado";

        public async Task<IOperation<CepSearchResponseDto>> SearchAsync(string zipCode)
        {
            var result = await _serviceProvider.GetRequiredService<IBrasilApiGateway>()
                                               .GetAddress(zipCode);

            if (result.IsErrorResponseApi())
            { 
                var notifcation = NotificationErrors.Create(nameof(result), message: result.Content ?? "Content vazio", DefaultMessagesErrors.ProviderError);
                return OperationFactory.CreateFail<CepSearchResponseDto>(notifcation, ErrorType.ProviderError);
            }

            var responseGateway = JsonSerializer.Deserialize<BrasilApiCepResponse>(result.Content!)!;

            var response = new CepSearchResponseDto
            {
                ZipCode = zipCode,
                State = responseGateway.State,
                City = responseGateway.City,
                Neighborhood = responseGateway.Neighborhood ?? NotSearch,
                Street = responseGateway.Street ?? NotSearch,
                Number = "S/N",
                ResponseProvider = result.Content!,
                ProviderType = ProviderType.BrasilApi,
                Complement = null,
                IbgeCode = null,
                DDD = null,
            };

            return response.ToSuccess();
        }
    }
}
