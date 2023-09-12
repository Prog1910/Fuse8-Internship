using InternalApi.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Domain.Persistence;

public sealed class CurDbContext : DbContext
{
	public CurDbContext(DbContextOptions<CurDbContext> options) : base(options)
	{
	}

	public DbSet<CacheTask> CacheTasks { get; set; }
	public DbSet<CurrenciesOnDateCache> CurrenciesOnDates { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("cur");

		modelBuilder.HasPostgresExtension("uuid-ossp");

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
