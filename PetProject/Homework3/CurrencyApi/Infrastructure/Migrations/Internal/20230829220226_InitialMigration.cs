using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Internal
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cur");

            migrationBuilder.CreateTable(
                name: "currencies_on_date",
                schema: "cur",
                columns: table => new
                {
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    from_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies_on_date", x => new { x.last_updated_at, x.from_currency });
                });

            migrationBuilder.CreateTable(
                name: "currency",
                schema: "cur",
                columns: table => new
                {
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    base_currency = table.Column<string>(type: "character varying(8)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    to_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    exchange_rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency", x => new { x.last_updated_at, x.base_currency, x.id });
                    table.ForeignKey(
                        name: "fk_currency_currencies_on_date_cached_currencies_temp_id",
                        columns: x => new { x.last_updated_at, x.base_currency },
                        principalSchema: "cur",
                        principalTable: "currencies_on_date",
                        principalColumns: new[] { "last_updated_at", "from_currency" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_currency_code",
                schema: "cur",
                table: "currency",
                column: "to_currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currency",
                schema: "cur");

            migrationBuilder.DropTable(
                name: "currencies_on_date",
                schema: "cur");
        }
    }
}
