using Domain.Aggregates.CachedCurrenciesAggregate;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class InternalDbContext : DbContext
{
	public InternalDbContext(DbContextOptions<InternalDbContext> options) : base(options)
	{
	}

	public DbSet<CachedCurrencies> CurrenciesOnDate { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CachedCurrencies>().ToTable(name: "currencies_on_date", schema: "cur");

		modelBuilder.ApplyConfiguration(new CurrenciesOnDateConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}