using BureauHexagonal.Core.Enums;

namespace BureauHexagonal.Infrastructure.Options.DataBase
{
    public sealed record DataBaseOptions
    {
        public const string SectionName = "Infraestructure:DataBases:Options";
        public StorageType UseDataBaseType { get; set; }
    }
}
