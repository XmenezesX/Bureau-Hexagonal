using BureauHexagonal.Infrastructure.DataBase.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace BureauHexagonal.Infrastructure.DataBase.Postgres
{
    public sealed class BureauPostgresDbContext : DbContext
    {
        public BureauPostgresDbContext(DbContextOptions<BureauPostgresDbContext> options) : base(options)
        {
        }
        public DbSet<BureauInfraEntity> Bureau { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BureauInfraEntity.CreateIndexs(modelBuilder);
        }
    }
}
