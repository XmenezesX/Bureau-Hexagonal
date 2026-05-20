using BureauHexagonal.Core.Utils;
using FluentAssertions;

namespace BureauHexagonal.Test.Unit.Core.Utils
{
    public class UtilsExtendsTests
    {
        [Theory(DisplayName = "OnlyNumbers deve retornar apenas dígitos quando a entrada contiver caracteres não numéricos")]
        [InlineData("123.456.789-10", "12345678910")]
        [InlineData("12.345.678/0001-90", "12345678000190")]
        [InlineData("abc123def456", "123456")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void OnlyNumbers_WhenInputHasNonDigits_ShouldReturnOnlyDigits(string? input, string expected)
        {
            // Arrange & Act
            var result = input!.OnlyNumbers();

            // Assert
            result.Should().Be(expected);
        }

        [Theory(DisplayName = "IsValidCep deve validar se o CEP possui o formato de 8 dígitos correto")]
        [InlineData("12345678", true)]
        [InlineData("1234567", false)]
        [InlineData("123456789", false)]
        [InlineData("abcdefgh", false)]
        [InlineData("", false)]
        public void IsValidCep_WhenCalled_ShouldReturnExpectedResult(string input, bool expected)
        {
            // Arrange & Act
            var result = input.IsValidCep();

            // Assert
            result.Should().Be(expected);
        }

        [Theory(DisplayName = "IsValidCpf deve validar se o CPF possui dígitos verificadores corretos ou se é inválido")]
        [InlineData("52998224725", true)] // CPF Válido
        [InlineData("11111111111", false)] // CPF com dígitos repetidos
        [InlineData("12345678901", false)] // CPF Inválido
        [InlineData("123", false)] // CPF curto
        public void IsValidCpf_WhenCalled_ShouldReturnExpectedResult(string input, bool expected)
        {
            // Arrange & Act
            var result = input.IsValidCpf();

            // Assert
            result.Should().Be(expected);
        }

        [Theory(DisplayName = "IsValidCnpj deve validar se o CNPJ possui dígitos verificadores corretos ou se é inválido")]
        [InlineData("12345678000195", true)] // CNPJ Válido
        [InlineData("12345678000100", false)] // CNPJ Inválido
        [InlineData("123", false)] // CNPJ curto
        public void IsValidCnpj_WhenCalled_ShouldReturnExpectedResult(string input, bool expected)
        {
            // Arrange & Act
            var result = input.IsValidCnpj();

            // Assert
            result.Should().Be(expected);
        }
    }
}
