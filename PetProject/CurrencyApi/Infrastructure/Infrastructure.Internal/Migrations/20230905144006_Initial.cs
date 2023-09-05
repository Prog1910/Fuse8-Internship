using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Internal.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cur");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "cache_tasks",
                schema: "cur",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    base_currency_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Created")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cache_tasks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currencies_on_date",
                schema: "cur",
                columns: table => new
                {
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    base_currency_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies_on_date", x => new { x.last_updated_at, x.base_currency_code });
                });

            migrationBuilder.CreateTable(
                name: "currency",
                schema: "cur",
                columns: table => new
                {
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    base_currency_code = table.Column<string>(type: "character varying(5)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    currency_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    exchange_rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency", x => new { x.last_updated_at, x.base_currency_code, x.id });
                    table.ForeignKey(
                        name: "fk_currency_currencies_on_date_currencies_on_date_cache_temp_id",
                        columns: x => new { x.last_updated_at, x.base_currency_code },
                        principalSchema: "cur",
                        principalTable: "currencies_on_date",
                        principalColumns: new[] { "last_updated_at", "base_currency_code" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_currency_code",
                schema: "cur",
                table: "currency",
                column: "currency_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cache_tasks",
                schema: "cur");

            migrationBuilder.DropTable(
                name: "currency",
                schema: "cur");

            migrationBuilder.DropTable(
                name: "currencies_on_date",
                schema: "cur");
        }
    }
}
