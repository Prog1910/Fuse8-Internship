using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Public.Persistence;

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

		base.OnModelCreating(modelBuilder);
	}
}