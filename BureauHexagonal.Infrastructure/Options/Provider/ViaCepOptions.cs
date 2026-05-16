namespace BureauHexagonal.Infrastructure.Options.Provider
{
    public sealed class ViaCepOptions
    {
        public const string SectionName = "Infraestructure:Providers:ViaCep";
        public string BaseUrl { get; init; }
        public int TimeoutInSeconds { get; init; }
    }
}
