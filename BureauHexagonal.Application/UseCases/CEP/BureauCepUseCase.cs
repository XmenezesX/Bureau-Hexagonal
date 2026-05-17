using BureauHexagonal.Application.Dtos.Inputs;
using BureauHexagonal.Application.Dtos.Output.Cep;
using BureauHexagonal.Application.UnitOfWork;
using BureauHexagonal.Application.UseCases.CEP.Validation;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Entities;
using BureauHexagonal.Core.Ports;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace BureauHexagonal.Application.UseCases.CEP
{
    public sealed class BureauCepUseCase(IServiceProvider _serviceProvider) : IBureauCepUseCase
    {
        public async Task<IOperation> ExecAsync(BureauInputDto input)
        {
            IUnitOfWork? unitOfWork = null;
            try
            {
                var notification = _serviceProvider.GetRequiredService<IBureauCepUseCaseValidation>()
                                               .Validate(input);
                if (notification.HaveError())
                    return notification.ToFail(ErrorType.ValidationError);

                var bureauRepositry = _serviceProvider.GetRequiredService<IBureauRepositoryPort>();

                var operation = await bureauRepositry.GetByCodeAsync(input.Code);
                if (operation.IsSuccess())
                {
                    var bureauEntity = operation.SuccessAs<BureauEntity>();
                    var response = JsonSerializer.Deserialize<CepSearchResponseDto>(bureauEntity.DataBureau);
                    return response.ToSuccess();
                }

                unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
                await unitOfWork.BeginTransactionAsync();

                var operationSearch = await _serviceProvider.GetRequiredKeyedService<ISearchCepPort>(input.ProviderType)
                                                            .SearchAsync(input.Code);

                if (operationSearch.IsFail())
                {
                    await unitOfWork.RollbackAsync();
                    return operationSearch;
                }

                var result = operationSearch.SuccessAs<CepSearchResponseDto>();
                var operationConceptualCreate = BureauEntity.ConceptualCreate(
                    code: input.Code,
                    providerType: input.ProviderType,
                    bureauType: input.BureauType,
                    responseProviderJson: result.ResponseProvider,
                    dataBureau: JsonSerializer.Serialize(result),
                    synchronized: false
                );

                if (operationConceptualCreate.IsFail())
                {
                    await unitOfWork.RollbackAsync();
                    return operationConceptualCreate;
                }

                var operationCreate = await bureauRepositry.CreateAsync(operationConceptualCreate.SuccessAs<BureauEntity>());
                if (operationCreate.IsFail())
                {
                    await unitOfWork.RollbackAsync();
                    return operationCreate;
                }

                await unitOfWork.CommitAsync();

                return result.ToSuccess();
            }
            catch (Exception ex)
            {
                if (unitOfWork is not null)
                    await unitOfWork.RollbackAsync();

                return OperationFactory.CreateFail(ex);
            }
        }
    }
}
