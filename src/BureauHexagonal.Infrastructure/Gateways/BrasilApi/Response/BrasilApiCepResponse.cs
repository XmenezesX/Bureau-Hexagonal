using BureauHexagonal.Infrastructure.Gateways.BrasilApi.Response.Cep;
using System.Text.Json.Serialization;

namespace BureauHexagonal.Infrastructure.Gateways.BrasilApi.Response
{
    public sealed class BrasilApiCepResponse
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("neighborhood")]
        public string? Neighborhood { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("timezoneName")]
        public string? TimezoneName { get; set; }

        [JsonPropertyName("location")]
        public BrasilApiLocationResponse? Location { get; set; }

    }
}
