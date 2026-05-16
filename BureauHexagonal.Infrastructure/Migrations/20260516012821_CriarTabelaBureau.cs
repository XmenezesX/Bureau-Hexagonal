using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauHexagonal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaBureau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bureau",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_type = table.Column<int>(type: "integer", nullable: false),
                    provider_type_description = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    bureau_type = table.Column<int>(type: "integer", nullable: false),
                    bureau_type_description = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    response_provider = table.Column<string>(type: "json", nullable: false),
                    data = table.Column<string>(type: "json", nullable: false),
                    synchronized = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bureau", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bureau");
        }
    }
}
