using System.Text.Json.Serialization;

namespace BureauHexagonal.Core.Common.NotificationError
{
    public sealed class Errors
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("propertyName")]
        public string PropertyName { get; set; }
    }
}
