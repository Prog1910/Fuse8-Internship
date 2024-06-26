﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PublicApi.Domain.Persistence;

#nullable disable

namespace PublicApi.Infrastructure.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("user")
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PublicApi.Domain.Aggregates.FavoritesCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BaseCurrencyCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("base_currency_code");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("currency_code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_favorites");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_favorites_name");

                    b.HasIndex("CurrencyCode", "BaseCurrencyCode")
                        .IsUnique()
                        .HasDatabaseName("ix_favorites_currency_code_base_currency_code");

                    b.ToTable("favorites", "user", t =>
                        {
                            t.HasCheckConstraint("CK_favorites_base_currency_code_MinLength", "LENGTH(base_currency_code) >= 3");

                            t.HasCheckConstraint("CK_favorites_currency_code_MinLength", "LENGTH(currency_code) >= 3");

                            t.HasCheckConstraint("CK_favorites_name_MinLength", "LENGTH(name) >= 1");
                        });
                });

            modelBuilder.Entity("PublicApi.Domain.Aggregates.SettingsCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrencyRoundCount")
                        .HasColumnType("integer")
                        .HasColumnName("currency_round_count");

                    b.Property<string>("DefaultCurrencyCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("default_currency_code");

                    b.HasKey("Id")
                        .HasName("pk_settings");

                    b.ToTable("settings", "user", t =>
                        {
                            t.HasCheckConstraint("CK_settings_currency_round_count_Range", "currency_round_count >= 0 AND currency_round_count <= 15");

                            t.HasCheckConstraint("CK_settings_default_currency_code_MinLength", "LENGTH(default_currency_code) >= 3");
                        });

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CurrencyRoundCount = 2,
                            DefaultCurrencyCode = "RUB"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
