using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Application.Internal.Interfaces.Persistence;

public interface ICurDbContext
{
	DbSet<CacheTask> CacheTasks { get; set; }
	DbSet<CurrenciesOnDateCache> CurrenciesOnDates { get; set; }

	Task SaveChangesAsync();
}
