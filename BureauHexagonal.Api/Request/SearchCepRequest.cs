using BureauHexagonal.Core.Enums;

namespace BureauHexagonal.Api.Request
{
    public sealed record SearchCepRequest(string ZipCode, BureauType BureauType, ProviderType ProviderType);
}
