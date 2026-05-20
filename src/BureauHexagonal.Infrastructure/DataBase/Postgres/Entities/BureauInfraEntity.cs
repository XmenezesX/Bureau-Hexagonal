using BureauHexagonal.Infrastructure.DataBase.Postgres.Entities.BaseInfraPostgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BureauHexagonal.Infrastructure.DataBase.Postgres.Entities
{
    [Table("bureau")]
    public sealed record BureauInfraEntity : BaseInfraEntityPostgres
    {
        [Column("provider_type")]
        public int ProviderType { get; init; }

        [MaxLength(30)]
        [Column("provider_type_description")]
        public string ProviderTypeDescription { get; init; }

        [Column("bureau_type")]
        public int BureauType { get; init; }

        [MaxLength(30)]
        [Column("bureau_type_description")]
        public string BureauTypeDescription { get; init; }

        [MaxLength(255)]
        [Column("code")]
        public string Code { get; init; }

        [Column("response_provider", TypeName = PostgresDbTypes.Json)]
        public string ResponseProviderJson { get; init; }

        [Column("data", TypeName = PostgresDbTypes.Json)]
        public string DataBureau { get; init; }

        [Column("synchronized")]
        public bool Synchronized { get; init; }

        public static void CreateIndexs(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BureauInfraEntity>()
                .HasIndex(b => b.Code);

            modelBuilder.Entity<BureauInfraEntity>()
                .HasIndex(b => b.ProviderType);

            modelBuilder.Entity<BureauInfraEntity>()
                .HasIndex(b => b.BureauType);
        }
    }
}
