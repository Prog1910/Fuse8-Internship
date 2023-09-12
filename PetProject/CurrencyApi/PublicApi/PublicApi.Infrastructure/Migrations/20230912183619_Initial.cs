using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PublicApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.CreateTable(
                name: "favorites",
                schema: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    currency_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    base_currency_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_favorites", x => x.id);
                    table.CheckConstraint("CK_favorites_base_currency_code_MinLength", "LENGTH(base_currency_code) >= 3");
                    table.CheckConstraint("CK_favorites_currency_code_MinLength", "LENGTH(currency_code) >= 3");
                    table.CheckConstraint("CK_favorites_name_MinLength", "LENGTH(name) >= 1");
                });

            migrationBuilder.CreateTable(
                name: "settings",
                schema: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    default_currency_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    currency_round_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_settings", x => x.id);
                    table.CheckConstraint("CK_settings_currency_round_count_Range", "currency_round_count >= 0 AND currency_round_count <= 15");
                    table.CheckConstraint("CK_settings_default_currency_code_MinLength", "LENGTH(default_currency_code) >= 3");
                });

            migrationBuilder.InsertData(
                schema: "user",
                table: "settings",
                columns: new[] { "id", "currency_round_count", "default_currency_code" },
                values: new object[] { 1, 2, "RUB" });

            migrationBuilder.CreateIndex(
                name: "ix_favorites_currency_code_base_currency_code",
                schema: "user",
                table: "favorites",
                columns: new[] { "currency_code", "base_currency_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_favorites_name",
                schema: "user",
                table: "favorites",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorites",
                schema: "user");

            migrationBuilder.DropTable(
                name: "settings",
                schema: "user");
        }
    }
}
