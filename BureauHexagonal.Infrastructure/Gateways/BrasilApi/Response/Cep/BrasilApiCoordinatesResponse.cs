using System.Text.Json.Serialization;

namespace BureauHexagonal.Infrastructure.Gateways.BrasilApi.Response.Cep
{
    public sealed record BrasilApiCoordinatesResponse
    {
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }
    }
}
