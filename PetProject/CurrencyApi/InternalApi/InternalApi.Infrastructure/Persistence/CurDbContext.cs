using Domain.Aggregates;
using InternalApi.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Infrastructure.Persistence;

public sealed class CurDbContext : DbContext, ICurDbContext
{
	public CurDbContext(DbContextOptions<CurDbContext> options) : base(options)
	{
	}

	public DbSet<CacheTask> CacheTasks { get; set; }
	public DbSet<CurrenciesOnDateCache> CurrenciesOnDates { get; set; }

	public new Task SaveChangesAsync(CancellationToken cancellationToken)
	{
		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("cur");

		modelBuilder.HasPostgresExtension("uuid-ossp");

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
