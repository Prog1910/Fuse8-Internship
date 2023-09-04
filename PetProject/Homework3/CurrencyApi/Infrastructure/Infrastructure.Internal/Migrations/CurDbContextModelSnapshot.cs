﻿// <auto-generated />
using System;
using Infrastructure.Internal.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Internal.Migrations
{
    [DbContext(typeof(CurDbContext))]
    partial class CurDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cur")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Aggregates.CacheTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("BaseCurrencyCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("base_currency_code");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Created")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_cache_tasks");

                    b.ToTable("cache_tasks", "cur");
                });

            modelBuilder.Entity("Domain.Aggregates.CurrenciesOnDateCache", b =>
                {
                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_updated_at");

                    b.Property<string>("BaseCurrencyCode")
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("base_currency_code");

                    b.HasKey("LastUpdatedAt", "BaseCurrencyCode")
                        .HasName("pk_currencies_on_date");

                    b.ToTable("currencies_on_date", "cur");
                });

            modelBuilder.Entity("Domain.Aggregates.CurrenciesOnDateCache", b =>
                {
                    b.OwnsMany("Domain.Aggregates.Currency", "Currencies", b1 =>
                        {
                            b1.Property<DateTime>("LastUpdatedAt")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("last_updated_at");

                            b1.Property<string>("BaseCurrencyCode")
                                .HasColumnType("character varying(8)")
                                .HasColumnName("base_currency_code");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasColumnName("id");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasMaxLength(8)
                                .HasColumnType("character varying(8)")
                                .HasColumnName("currency_code");

                            b1.Property<decimal>("Value")
                                .HasColumnType("numeric")
                                .HasColumnName("exchange_rate");

                            b1.HasKey("LastUpdatedAt", "BaseCurrencyCode", "Id")
                                .HasName("pk_currency");

                            b1.HasIndex("Code")
                                .HasDatabaseName("ix_currency_code");

                            b1.ToTable("currency", "cur");

                            b1.WithOwner()
                                .HasForeignKey("LastUpdatedAt", "BaseCurrencyCode")
                                .HasConstraintName("fk_currency_currencies_on_date_currencies_on_date_cache_temp_id");
                        });

                    b.Navigation("Currencies");
                });
#pragma warning restore 612, 618
        }
    }
}
