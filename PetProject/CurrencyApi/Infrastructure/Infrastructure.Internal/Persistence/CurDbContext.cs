using Application.Internal.Interfaces.Persistence;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Internal.Persistence;

public sealed class CurDbContext : DbContext, ICurDbContext
{
	public CurDbContext(DbContextOptions<CurDbContext> options) : base(options)
	{
	}

	public DbSet<CacheTask> CacheTasks { get; set; }
	public DbSet<CurrenciesOnDateCache> CurrenciesOnDates { get; set; }

	public Task<int> SaveChangesAsync()
	{
		return base.SaveChangesAsync();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("cur");

		modelBuilder.HasPostgresExtension("uuid-ossp");

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		base.OnModelCreating(modelBuilder);
	}
}