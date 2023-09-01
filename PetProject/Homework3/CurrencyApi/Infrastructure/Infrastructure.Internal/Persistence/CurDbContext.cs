using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Internal.Persistence;

public sealed class CurDbContext : DbContext
{
	public CurDbContext(DbContextOptions<CurDbContext> options) : base(options)
	{
	}

	public DbSet<CachedCurrencies> CurrenciesOnDate { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("cur");

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
