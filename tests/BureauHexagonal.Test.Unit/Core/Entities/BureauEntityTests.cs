using BureauHexagonal.Core.Entities;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Common.Operation;
using FluentAssertions;

namespace BureauHexagonal.Test.Unit.Core.Entities
{
    public class BureauEntityTests
    {
        [Fact(DisplayName = "ConceptualCreate com dados válidos deve criar a entidade com sucesso e formatar o código")]
        public void ConceptualCreate_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var code = "01001-000";
            var providerType = ProviderType.ViaCep;
            var bureauType = BureauType.CEP;
            var responseProviderJson = "{\"zip\": \"01001000\"}";
            var dataBureau = "{\"ZipCode\": \"01001000\"}";
            var synchronized = false;

            // Act
            var result = BureauEntity.ConceptualCreate(code, providerType, bureauType, responseProviderJson, dataBureau, synchronized);

            // Assert
            OperationExtends.IsSuccess(result).Should().BeTrue();
            var entity = OperationExtends.SuccessAs<BureauEntity>(result);
            entity.Code.Should().Be("01001000"); // Formatted by OnlyNumbers
            entity.ProviderType.Should().Be(providerType);
            entity.BureauType.Should().Be(bureauType);
            entity.ResponseProviderJson.Should().Be(responseProviderJson);
            entity.DataBureau.Should().Be(dataBureau);
            entity.Synchronized.Should().Be(synchronized);
            entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Theory(DisplayName = "ConceptualCreate com código vazio ou nulo deve retornar erro de validação correspondente")]
        [InlineData("", "Code")]
        [InlineData(" ", "Code")]
        [InlineData(null, "Code")]
        public void ConceptualCreate_WithInvalidCode_ShouldReturnValidationError(string? code, string errorField)
        {
            // Arrange
            var providerType = ProviderType.ViaCep;
            var bureauType = BureauType.CEP;
            var responseProviderJson = "{}";
            var dataBureau = "{}";

            // Act
            var result = BureauEntity.ConceptualCreate(code!, providerType, bureauType, responseProviderJson, dataBureau, false);

            // Assert
            OperationExtends.IsFail(result).Should().BeTrue();
            OperationExtends.GetErrors(result)._errors.Should().Contain(e => e.PropertyName == errorField);
        }

        [Fact(DisplayName = "ConceptualCreate com JSON de resposta do provedor vazio deve retornar erro de validação")]
        public void ConceptualCreate_WithInvalidResponseProviderJson_ShouldReturnValidationError()
        {
            // Arrange
            var code = "01001000";
            var providerType = ProviderType.ViaCep;
            var bureauType = BureauType.CEP;
            var responseProviderJson = "";
            var dataBureau = "{}";

            // Act
            var result = BureauEntity.ConceptualCreate(code, providerType, bureauType, responseProviderJson, dataBureau, false);

            // Assert
            OperationExtends.IsFail(result).Should().BeTrue();
            OperationExtends.GetErrors(result)._errors.Should().Contain(e => e.PropertyName == "ResponseProviderJson");
        }

        [Fact(DisplayName = "ConceptualCreate com dados de bureau vazios deve retornar erro de validação")]
        public void ConceptualCreate_WithInvalidDataBureau_ShouldReturnValidationError()
        {
            // Arrange
            var code = "01001000";
            var providerType = ProviderType.ViaCep;
            var bureauType = BureauType.CEP;
            var responseProviderJson = "{}";
            var dataBureau = "";

            // Act
            var result = BureauEntity.ConceptualCreate(code, providerType, bureauType, responseProviderJson, dataBureau, false);

            // Assert
            OperationExtends.IsFail(result).Should().BeTrue();
            OperationExtends.GetErrors(result)._errors.Should().Contain(e => e.PropertyName == "DataBureau");
        }

        [Fact(DisplayName = "ConceptualCreate com tipo de provedor inválido deve retornar erro de validação")]
        public void ConceptualCreate_WithInvalidProviderType_ShouldReturnValidationError()
        {
            // Arrange
            var code = "01001000";
            var providerType = (ProviderType)999;
            var bureauType = BureauType.CEP;
            var responseProviderJson = "{}";
            var dataBureau = "{}";

            // Act
            var result = BureauEntity.ConceptualCreate(code, providerType, bureauType, responseProviderJson, dataBureau, false);

            // Assert
            OperationExtends.IsFail(result).Should().BeTrue();
            OperationExtends.GetErrors(result)._errors.Should().Contain(e => e.PropertyName == "ProviderType");
        }

        [Fact(DisplayName = "ConceptualCreate com tipo de bureau inválido deve retornar erro de validação")]
        public void ConceptualCreate_WithInvalidBureauType_ShouldReturnValidationError()
        {
            // Arrange
            var code = "01001000";
            var providerType = ProviderType.ViaCep;
            var bureauType = (BureauType)999;
            var responseProviderJson = "{}";
            var dataBureau = "{}";

            // Act
            var result = BureauEntity.ConceptualCreate(code, providerType, bureauType, responseProviderJson, dataBureau, false);

            // Assert
            OperationExtends.IsFail(result).Should().BeTrue();
            OperationExtends.GetErrors(result)._errors.Should().Contain(e => e.PropertyName == "BureauType");
        }

        [Fact(DisplayName = "Restore com dados válidos deve recriar a entidade preenchendo todas as propriedades")]
        public void Restore_WithValidData_ShouldReturnEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var code = "01001000";
            var providerType = ProviderType.ViaCep;
            var bureauType = BureauType.CEP;
            var responseProviderJson = "{}";
            var dataBureau = "{}";
            var synchronized = true;
            var createdAt = DateTime.UtcNow.AddDays(-1);
            var updatedAt = DateTime.UtcNow;
            var deletedAt = (DateTimeOffset?)null;

            // Act
            var entity = BureauEntity.Restore(id, code, providerType, bureauType, responseProviderJson, dataBureau, synchronized, createdAt, updatedAt, deletedAt);

            // Assert
            entity.Id.Should().Be(id);
            entity.Code.Should().Be(code);
            entity.ProviderType.Should().Be(providerType);
            entity.BureauType.Should().Be(bureauType);
            entity.ResponseProviderJson.Should().Be(responseProviderJson);
            entity.DataBureau.Should().Be(dataBureau);
            entity.Synchronized.Should().Be(synchronized);
            entity.CreatedAt.Should().Be(createdAt);
            entity.UpdatedAt.Should().Be(updatedAt);
            entity.DeletedAt.Should().BeNull();
        }

        [Fact(DisplayName = "SetUpdatedAt deve definir o campo de atualização para o horário de UTC atual")]
        public void SetUpdatedAt_WhenCalled_ShouldSetUpdatedAtToCurrentTime()
        {
            // Arrange
            var entity = BureauEntity.Restore(Guid.NewGuid(), "01001000", ProviderType.ViaCep, BureauType.CEP, "{}", "{}", true, DateTime.UtcNow, null, null);

            // Act
            entity.SetUpdatedAt();

            // Assert
            entity.UpdatedAt.Should().NotBeNull();
            entity.UpdatedAt!.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact(DisplayName = "SetDeletedAt deve definir o campo de exclusão lógica para o horário de UTC atual")]
        public void SetDeletedAt_WhenCalled_ShouldSetDeletedAtToCurrentTime()
        {
            // Arrange
            var entity = BureauEntity.Restore(Guid.NewGuid(), "01001000", ProviderType.ViaCep, BureauType.CEP, "{}", "{}", true, DateTime.UtcNow, null, null);

            // Act
            entity.SetDeletedAt();

            // Assert
            entity.DeletedAt.Should().NotBeNull();
            entity.DeletedAt!.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
