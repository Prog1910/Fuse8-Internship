using Microsoft.EntityFrameworkCore;
using PublicApi.Domain.Aggregates;
using Shared.Domain.Enums;

namespace PublicApi.Domain.Persistence;

public sealed class UserDbContext : DbContext
{
	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
	}

	public DbSet<SettingsCache> Settings { get; set; }
	public DbSet<FavoritesCache> Favorites { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("user");

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		SettingsCache settings = new() { Id = 1, DefaultCurrencyCode = CurrencyType.RUB.ToString(), CurrencyRoundCount = 2 };
		modelBuilder.Entity<SettingsCache>().HasData(settings);

		base.OnModelCreating(modelBuilder);
	}
}
