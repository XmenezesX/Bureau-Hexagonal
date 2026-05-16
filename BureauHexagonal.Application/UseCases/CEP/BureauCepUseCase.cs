using BureauHexagonal.Application.Dtos.Inputs;
using BureauHexagonal.Application.UseCases.CEP.Validation;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace BureauHexagonal.Application.UseCases.CEP
{
    public sealed class BureauCepUseCase(IServiceProvider _serviceProvider) : IBureauCepUseCase
    {
        public async Task<IOperation> ExecAsync(BureauInputDto input)
        {
            var notification = _serviceProvider.GetRequiredService<IBureauCepUseCaseValidation>()
                                               .Validate(input);
            if (notification.HaveError())
                return notification.ToFail(ErrorType.ValidationError);
           
            var searchCepAdapters = _serviceProvider.GetRequiredService<IEnumerable<ISearchCepPort>>();
            var adapter = searchCepAdapters.FirstOrDefault(x => x.CanHandle(input.ProviderType));
            if (adapter is null)
            {
                notification.AddError(nameof(input.ProviderType), "Nenhum provedor de busca de CEP compatível foi encontrado.");
                return notification.ToFail(ErrorType.ValidationError);
            }

            return await adapter.SearchAsync(input.ZipCode);
        }
    }
}
