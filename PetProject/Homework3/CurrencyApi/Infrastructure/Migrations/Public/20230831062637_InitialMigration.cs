using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Public
{
	/// <inheritdoc />
	public partial class InitialMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "user");

			migrationBuilder.CreateTable(
				name: "favorite_currencies",
				schema: "user",
				columns: table => new
				{
					name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
					currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
					base_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_favorite_currencies", x => x.name);
					table.CheckConstraint("CK_favorite_currencies_base_currency_MinLength", "LENGTH(base_currency) >= 3");
					table.CheckConstraint("CK_favorite_currencies_currency_MinLength", "LENGTH(currency) >= 3");
					table.CheckConstraint("CK_favorite_currencies_name_MinLength", "LENGTH(name) >= 1");
				});

			migrationBuilder.CreateTable(
				name: "settings",
				schema: "user",
				columns: table => new
				{
					id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					default_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
					currency_round_count = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_settings", x => x.id);
					table.CheckConstraint("CK_settings_currency_round_count_Range", "currency_round_count >= 0 AND currency_round_count <= 2147483647");
					table.CheckConstraint("CK_settings_default_currency_MinLength", "LENGTH(default_currency) >= 3");
				});

			migrationBuilder.InsertData(
				schema: "user",
				table: "settings",
				columns: new[] { "id", "default_currency", "currency_round_count" },
				values: new object[] { 1, "RUB", 2 });

			migrationBuilder.CreateIndex(
				name: "ix_favorite_currencies_currency_base_currency",
				schema: "user",
				table: "favorite_currencies",
				columns: new[] { "currency", "base_currency" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "ix_favorite_currencies_name",
				schema: "user",
				table: "favorite_currencies",
				column: "name",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "favorite_currencies",
				schema: "user");

			migrationBuilder.DropTable(
				name: "settings",
				schema: "user");
		}
	}
}