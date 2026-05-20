using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BureauHexagonal.Infrastructure.Utils
{
    public static class ApiResponseUtils
    {
        internal sealed record ErrorBodyResponse
        {
            [JsonPropertyName("erro")]
            public string? Erro { get; set; }
        }

        public static bool IsErrorResponseApi(this ApiResponse<string>? responseApi)
        {
            if (responseApi is null)
                return true;

            if (!responseApi.IsSuccessStatusCode)
                return true;

            if (responseApi.Content is not null)
            {
                try
                {
                    var errorInBody = JsonSerializer.Deserialize<ErrorBodyResponse>(responseApi.Content)!;
                    return string.Equals(errorInBody?.Erro, "true", StringComparison.OrdinalIgnoreCase); ;
                }
                catch
                {
                }
            }

            return false;
        }
    }
}
