using BureauHexagonal.Application.Dtos.Inputs;
using BureauHexagonal.Application.UseCases.CEP.Validation;
using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Enums;
using FluentAssertions;

namespace BureauHexagonal.Test.Unit.Application.UseCases.CEP.Validation
{
    public class BureauCepUseCaseValidationTests
    {
        private readonly BureauCepUseCaseValidation _validation;

        public BureauCepUseCaseValidationTests()
        {
            _validation = new BureauCepUseCaseValidation();
        }

        [Fact(DisplayName = "Validate com entrada nula deve retornar erro indicando requisição nula")]
        public void Validate_WhenInputIsNull_ShouldReturnValidationErrorWithRequestIsNull()
        {
            // Arrange
            BureauInputDto? input = null;

            // Act
            var result = _validation.Validate(input!);

            // Assert
            result.HaveError().Should().BeTrue();
            result._errors.Should().Contain(e => e.PropertyName == "input" && e.Message == DefaultMessagesErrors.RequestIsNull);
        }

        [Theory(DisplayName = "Validate com CEP vazio, em branco ou nulo deve retornar erro indicando campo obrigatório")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_WhenCodeIsEmpty_ShouldReturnValidationErrorWithFieldIsRequired(string? code)
        {
            // Arrange
            var input = new BureauInputDto(code!, ProviderType.ViaCep, BureauType.CEP);

            // Act
            var result = _validation.Validate(input);

            // Assert
            result.HaveError().Should().BeTrue();
            result._errors.Should().Contain(e => e.PropertyName == "Code" && e.Message == DefaultMessagesErrors.FieldIsRequired("Code"));
        }

        [Theory(DisplayName = "Validate com CEP com tamanho diferente de 8 caracteres deve retornar erro de tamanho exato")]
        [InlineData("1234567")]
        [InlineData("123456789")]
        public void Validate_WhenCodeLengthIsInvalid_ShouldReturnValidationErrorWithExactLength(string code)
        {
            // Arrange
            var input = new BureauInputDto(code, ProviderType.ViaCep, BureauType.CEP);

            // Act
            var result = _validation.Validate(input);

            // Assert
            result.HaveError().Should().BeTrue();
            result._errors.Should().Contain(e => e.PropertyName == "Code" && e.Message == DefaultMessagesErrors.ExactLength("Code", 8));
        }

        [Fact(DisplayName = "Validate com tipo de provedor inválido deve retornar erro de validação correspondente")]
        public void Validate_WhenProviderTypeIsInvalid_ShouldReturnValidationError()
        {
            // Arrange
            var input = new BureauInputDto("12345678", (ProviderType)999, BureauType.CEP);

            // Act
            var result = _validation.Validate(input);

            // Assert
            result.HaveError().Should().BeTrue();
            result._errors.Should().Contain(e => e.PropertyName == "ProviderType" && e.Message == DefaultMessagesErrors.EnumIsInvalid);
            result._friendlyMessages.Should().Contain("É necessário informar um provedor válido");
        }

        [Fact(DisplayName = "Validate com tipo de bureau inválido deve retornar erro de validação correspondente")]
        public void Validate_WhenBureauTypeIsInvalid_ShouldReturnValidationError()
        {
            // Arrange
            var input = new BureauInputDto("12345678", ProviderType.ViaCep, (BureauType)999);

            // Act
            var result = _validation.Validate(input);

            // Assert
            result.HaveError().Should().BeTrue();
            result._errors.Should().Contain(e => e.PropertyName == "BureauType" && e.Message == DefaultMessagesErrors.EnumIsInvalid);
            result._friendlyMessages.Should().Contain("É necessário informar um bureau válido");
        }

        [Fact(DisplayName = "Validate com tipo de bureau diferente de CEP deve retornar erro indicando tipo incompatível")]
        public void Validate_WhenBureauTypeIsNotCEP_ShouldReturnValidationError()
        {
            // Arrange
            var input = new BureauInputDto("12345678", ProviderType.ViaCep, BureauType.None);

            // Act
            var result = _validation.Validate(input);

            // Assert
            result.HaveError().Should().BeTrue();
            result._errors.Should().Contain(e => e.PropertyName == "BureauType" && e.Message == DefaultMessagesErrors.BureauTypeInvalid);
        }

        [Fact(DisplayName = "Validate com entrada válida deve passar sem nenhum erro de validação")]
        public void Validate_WhenInputIsValid_ShouldReturnEmptyNotification()
        {
            // Arrange
            var input = new BureauInputDto("12345678", ProviderType.ViaCep, BureauType.CEP);

            // Act
            var result = _validation.Validate(input);

            // Assert
            result.HaveError().Should().BeFalse();
        }
    }
}
