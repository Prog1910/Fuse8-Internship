using Application.Public.Persistence;
using Domain.Aggregates;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Public.Persistence;

public sealed class UserDbContext : DbContext, IUserDbContext
{
	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
	}

	public DbSet<SettingsCache> Settings { get; set; }
	public DbSet<FavoritesCache> Favorites { get; set; }


	public Task SaveChangesAsync()
	{
		return base.SaveChangesAsync();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("user");

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		SettingsCache settings = new() { Id = 1, DefaultCurrencyCode = CurrencyType.RUB.ToString(), CurrencyRoundCount = 2 };
		modelBuilder.Entity<SettingsCache>().HasData(settings);

		base.OnModelCreating(modelBuilder);
	}
}
