using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;
using Domain.Aggregates.CachedSettingsAggregate;
using Domain.Options;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence;

public sealed class PublicDbContext : DbContext
{
	private readonly PublicApiOptions _options;

	public PublicDbContext(DbContextOptions<PublicDbContext> options, IOptionsSnapshot<PublicApiOptions> apiOptions) : base(options)
	{
		_options = apiOptions.Value;
	}

	public DbSet<CachedSettings> Settings { get; set; }
	public DbSet<CachedFavoriteCurrencies> FavoriteCurrencies { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CachedSettings>().ToTable("settings", "user");
		modelBuilder.Entity<CachedFavoriteCurrencies>().ToTable("favorite_currencies", "user");

		modelBuilder.ApplyConfiguration(new SettingsConfiguration(_options));
		modelBuilder.ApplyConfiguration(new FavoriteCurrenciesConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}
