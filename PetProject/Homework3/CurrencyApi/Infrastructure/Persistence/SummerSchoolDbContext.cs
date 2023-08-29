using Domain.Aggregates.CurrenciesOnDateAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class SummerSchoolDbContext : DbContext
{
	public SummerSchoolDbContext(DbContextOptions<SummerSchoolDbContext> options) : base(options)
	{
	}

	public DbSet<CachedCurrencies> CurrenciesOnDate { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CachedCurrencies>().ToTable(name: "currencies_on_date", schema: "cur");

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
