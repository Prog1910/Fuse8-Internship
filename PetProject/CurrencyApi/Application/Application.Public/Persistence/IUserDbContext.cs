using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Application.Public.Persistence;

public interface IUserDbContext
{
	DbSet<SettingsCache> Settings { get; set; }
	DbSet<FavoritesCache> Favorites { get; set; }

	Task<int> SaveChangesAsync();
}
