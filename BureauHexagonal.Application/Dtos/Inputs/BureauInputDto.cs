using BureauHexagonal.Core.Enums;

namespace BureauHexagonal.Application.Dtos.Inputs
{
    public sealed record BureauInputDto(string Code, ProviderType ProviderType, BureauType BureauType);
}
