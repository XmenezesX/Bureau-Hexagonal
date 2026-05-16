namespace BureauHexagonal.Infrastructure.Options.DataBase
{
    public sealed record PostgresDbOptions
    {
        public const string SectionName = "Infraestructure:DataBases:Postgres";
        public string User {  get; init; }
        public string Password { get; init; }
        public string DataBase { get; init; }
        public string Host { get; init; }
        public int Port { get; init; }

        public string GetConnectionString()
        {
            return $"Host={Host};Port={Port};Database={DataBase};Username={User};Password={Password}";
        }
    }
}
