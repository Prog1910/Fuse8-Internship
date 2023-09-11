using Domain.Aggregates;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using PublicApi.Application.Persistence;

namespace PublicApi.Infrastructure.Persistence;

public sealed class UserDbContext : DbContext, IUserDbContext
{
	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
	}

	public DbSet<SettingsCache> Settings { get; set; }
	public DbSet<FavoritesCache> Favorites { get; set; }


	public new Task SaveChangesAsync(CancellationToken cancellationToken)
	{
		return base.SaveChangesAsync(cancellationToken);
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
