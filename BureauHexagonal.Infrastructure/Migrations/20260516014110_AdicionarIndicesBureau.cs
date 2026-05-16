using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauHexagonal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarIndicesBureau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_bureau_bureau_type",
                table: "bureau",
                column: "bureau_type");

            migrationBuilder.CreateIndex(
                name: "IX_bureau_code",
                table: "bureau",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_bureau_provider_type",
                table: "bureau",
                column: "provider_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bureau_bureau_type",
                table: "bureau");

            migrationBuilder.DropIndex(
                name: "IX_bureau_code",
                table: "bureau");

            migrationBuilder.DropIndex(
                name: "IX_bureau_provider_type",
                table: "bureau");
        }
    }
}
