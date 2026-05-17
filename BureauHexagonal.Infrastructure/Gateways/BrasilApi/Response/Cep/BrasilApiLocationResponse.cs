using System.Text.Json.Serialization;

namespace BureauHexagonal.Infrastructure.Gateways.BrasilApi.Response.Cep
{
    public sealed record BrasilApiLocationResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public BrasilApiCepResponse? Coordinates { get; set; }
    }
}
