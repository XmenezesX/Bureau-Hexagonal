using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BureauHexagonal.Infrastructure.DataBase.Postgres.Entities.BaseInfraPostgres
{
    public abstract record BaseInfraEntityPostgres
    {
        [Key]
        [Column("id")]
        public Guid Id { get; init; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; init; }
        
        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; init; }
        
        [Column("deleted_at")]
        public DateTimeOffset? DeletedAt { get; init; }
    }
}
