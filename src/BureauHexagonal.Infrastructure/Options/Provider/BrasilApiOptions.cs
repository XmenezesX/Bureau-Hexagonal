namespace BureauHexagonal.Infrastructure.Options.Provider
{
    public sealed class BrasilApiOptions
    {
        public const string SectionName = "Infraestructure:Providers:BrasilApi";
        public string BaseUrl { get; init; }
        public int TimeoutInSeconds { get; init; }
    }
}
