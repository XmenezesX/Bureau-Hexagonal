using BureauHexagonal.Core.Enums;
using System.Text.Json.Serialization;

namespace BureauHexagonal.Application.Dtos.Output.Cep
{
    public sealed record CepSearchResponseDto
    {

        [JsonPropertyName("providerType")]
        public ProviderType ProviderType { get; init; }

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; init; }

        [JsonPropertyName("number")]
        public string? Number { get; init; }

        [JsonPropertyName("street")]
        public string Street { get; init; }

        [JsonPropertyName("complement")]
        public string? Complement { get; init; }

        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; init; }

        [JsonPropertyName("city")]
        public string City { get; init; }

        [JsonPropertyName("state")]
        public string State { get; init; }

        [JsonPropertyName("ddd")]
        public string? DDD { get; init; }

        [JsonPropertyName("ibgeCode")]
        public string? IbgeCode { get; init; }

        [JsonIgnore]
        public string ResponseProvider { get; init; }
    }
}
