using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;
using Domain.Aggregates.CachedSettingsAggregate;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class PublicDbContext : DbContext
{
	public PublicDbContext(DbContextOptions<PublicDbContext> options) : base(options)
	{
	}

	public DbSet<CachedSettings> Settings { get; set; }
	public DbSet<CachedFavoriteCurrency> FavoriteCurrencies { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CachedSettings>().ToTable(name: "settings", schema: "user");
		modelBuilder.Entity<CachedFavoriteCurrency>().ToTable(name: "favorite_currencies", schema: "user");

		modelBuilder.ApplyConfiguration(new SettingsConfiguration());
		modelBuilder.ApplyConfiguration(new FavoriteCurrenciesConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}