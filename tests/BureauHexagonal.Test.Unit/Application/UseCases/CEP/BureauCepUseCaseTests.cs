using BureauHexagonal.Application.Dtos.Inputs;
using BureauHexagonal.Application.Dtos.Output.Cep;
using BureauHexagonal.Application.UnitOfWork;
using BureauHexagonal.Application.UseCases.CEP;
using BureauHexagonal.Application.UseCases.CEP.Validation;
using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Entities;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Ports;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text.Json;

namespace BureauHexagonal.Test.Unit.Application.UseCases.CEP
{
    public class BureauCepUseCaseTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IBureauCepUseCaseValidation> _validationMock;
        private readonly Mock<IBureauRepositoryPort> _repositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ISearchCepPort> _searchCepPortMock;
        private readonly BureauCepUseCase _useCase;

        public BureauCepUseCaseTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _validationMock = new Mock<IBureauCepUseCaseValidation>();
            _repositoryMock = new Mock<IBureauRepositoryPort>();
            _uowMock = new Mock<IUnitOfWork>();
            _searchCepPortMock = new Mock<ISearchCepPort>();

            _serviceProviderMock.Setup(x => x.GetService(typeof(IBureauCepUseCaseValidation))).Returns(_validationMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IBureauRepositoryPort))).Returns(_repositoryMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IUnitOfWork))).Returns(_uowMock.Object);
            
            // Mock para IKeyedServiceProvider (usado pelo GetRequiredKeyedService)
            var keyedServiceProviderMock = _serviceProviderMock.As<IKeyedServiceProvider>();
            keyedServiceProviderMock.Setup(x => x.GetKeyedService(typeof(ISearchCepPort), It.IsAny<object>()))
                                    .Returns(_searchCepPortMock.Object);
            keyedServiceProviderMock.Setup(x => x.GetRequiredKeyedService(typeof(ISearchCepPort), It.IsAny<object>()))
                                    .Returns(_searchCepPortMock.Object);

            _useCase = new BureauCepUseCase(_serviceProviderMock.Object);
        }

        [Fact(DisplayName = "ExecAsync quando a validação falhar deve retornar erro de validação")]
        public async Task ExecAsync_WhenValidationFails_ShouldReturnValidationError()
        {
            // Arrange
            var input = new BureauInputDto("12345678", ProviderType.ViaCep, BureauType.CEP);
            var notification = NotificationErrors.Create("Test", "Error Message");
            _validationMock.Setup(x => x.Validate(input)).Returns(notification);

            // Act
            var result = await _useCase.ExecAsync(input);

            // Assert
            result.IsFail().Should().BeTrue();
            var failResult = result as IOperationFail;
            failResult!.ErrorType.Should().Be(ErrorType.ValidationError);
        }

        [Fact(DisplayName = "ExecAsync quando o CEP existe no banco de dados deve retornar sucesso com dados do banco e não consultar a API externa")]
        public async Task ExecAsync_WhenCepExistsInDatabase_ShouldReturnSuccessFromDatabase()
        {
            // Arrange
            var input = new BureauInputDto("01001000", ProviderType.ViaCep, BureauType.CEP);
            var cepResponse = new CepSearchResponseDto { ZipCode = "01001000", City = "São Paulo", Street = "Praça da Sé" };
            var bureauEntity = BureauEntity.Restore(
                Guid.NewGuid(), 
                input.Code, 
                input.ProviderType, 
                input.BureauType, 
                "{}", 
                JsonSerializer.Serialize(cepResponse), 
                true, 
                DateTime.UtcNow, 
                null, 
                null);

            _validationMock.Setup(x => x.Validate(input)).Returns(NotificationErrors.Empty);
            _repositoryMock.Setup(x => x.GetByCodeAndProviderAsync(input.Code, input.ProviderType))
                           .ReturnsAsync(OperationFactory.CreateSuccess(bureauEntity));

            // Act
            var result = await _useCase.ExecAsync(input);

            // Assert
            result.IsSuccess().Should().BeTrue();
            var successResult = result as IOperationSuccess<CepSearchResponseDto>;
            successResult!.Data.ZipCode.Should().Be(input.Code);
            _searchCepPortMock.Verify(x => x.SearchAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "ExecAsync quando o CEP não existe no banco deve consultar o provedor externo, salvar na base e comitar a transação")]
        public async Task ExecAsync_WhenCepDoesNotExistInDatabase_ShouldSearchAndSave()
        {
            // Arrange
            var input = new BureauInputDto("01001000", ProviderType.ViaCep, BureauType.CEP);
            var cepResponse = new CepSearchResponseDto { ZipCode = "01001000", City = "São Paulo", Street = "Praça da Sé", ResponseProvider = "{}" };

            _validationMock.Setup(x => x.Validate(input)).Returns(NotificationErrors.Empty);
            _repositoryMock.Setup(x => x.GetByCodeAndProviderAsync(input.Code, input.ProviderType))
                           .ReturnsAsync(OperationFactory.CreateFail<BureauEntity>());
            
            _searchCepPortMock.Setup(x => x.SearchAsync(input.Code))
                              .ReturnsAsync(OperationFactory.CreateSuccess(cepResponse));

            _repositoryMock.Setup(x => x.CreateAsync(It.IsAny<BureauEntity>()))
                           .ReturnsAsync(OperationFactory.CreateSuccess());

            // Act
            var result = await _useCase.ExecAsync(input);

            // Assert
            result.IsSuccess().Should().BeTrue();
            _uowMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _uowMock.Verify(x => x.CommitAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CreateAsync(It.IsAny<BureauEntity>()), Times.Once);
        }

        [Fact(DisplayName = "ExecAsync quando a busca externa falhar deve efetuar rollback da transação e retornar erro")]
        public async Task ExecAsync_WhenSearchFails_ShouldRollbackAndReturnFail()
        {
            // Arrange
            var input = new BureauInputDto("01001000", ProviderType.ViaCep, BureauType.CEP);
            
            _validationMock.Setup(x => x.Validate(input)).Returns(NotificationErrors.Empty);
            _repositoryMock.Setup(x => x.GetByCodeAndProviderAsync(input.Code, input.ProviderType))
                           .ReturnsAsync(OperationFactory.CreateFail<BureauEntity>());
            
            _searchCepPortMock.Setup(x => x.SearchAsync(input.Code))
                              .ReturnsAsync(OperationFactory.CreateFail<CepSearchResponseDto>());

            // Act
            var result = await _useCase.ExecAsync(input);

            // Assert
            result.IsFail().Should().BeTrue();
            _uowMock.Verify(x => x.RollbackAsync(), Times.Once);
            _uowMock.Verify(x => x.CommitAsync(), Times.Never);
        }
    }
}
