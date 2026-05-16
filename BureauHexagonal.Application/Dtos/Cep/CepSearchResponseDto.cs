namespace BureauHexagonal.Application.Dtos.Cep
{
    public sealed record CepSearchResponseDto
    {
        public string ZipCode { get; init; }
        public string? Number { get; init; }
        public string Street { get; init; }
        public string? Complement { get; init; }
        public string Neighborhood { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string ResponseProvider { get; init; }
        public string? DDD { get; init; }
        public string? IbgeCode { get; init; }
    }
}
