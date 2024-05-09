using Mapster;
using Microsoft.EntityFrameworkCore;
using PublicApi.Application.Interfaces.Rest;
using PublicApi.Domain.Aggregates;
using PublicApi.Domain.Persistence;
using Shared.Application.Dtos;

namespace PublicApi.Infrastructure.Services.Rest;

public sealed class FavoritesService : IFavoritesService
{
	private readonly UserDbContext _userDbContext;

	public FavoritesService(UserDbContext userDbContext)
	{
		_userDbContext = userDbContext;
	}

	public async Task DeleteFavoritesByNameAsync(string name, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = await _userDbContext.Favorites.SingleOrDefaultAsync(f => f.Name.Equals(name), cancellationToken)
								   ?? throw new Exception("An error occured while deleting favorites.");
		_userDbContext.Favorites.Remove(favorites);
		await _userDbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task<FavoritesDto?> GetFavoritesByNameAsync(string name, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = await _userDbContext.Favorites.SingleOrDefaultAsync(f => f.Name.Equals(name), cancellationToken)
								   ?? throw new Exception("An error occured while getting favorites.");

		return favorites.Adapt<FavoritesDto>();
	}

	public async Task<List<FavoritesDto>> GetAllFavoritesAsync(CancellationToken cancellationToken)
	{
		List<FavoritesCache> allFavorites = await _userDbContext.Favorites.ToListAsync(cancellationToken)
											?? throw new Exception("An error occurred while getting favorites.");
		return allFavorites.Adapt<List<FavoritesDto>>();
	}

	public async Task AddFavoritesAsync(FavoritesDto favoritesDto, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = favoritesDto.Adapt<FavoritesCache>();
		if (_userDbContext.Favorites.Any(
				f => f.Name.Equals(favorites.Name)
					 || (f.CurrencyCode.Equals(favorites.CurrencyCode) && f.BaseCurrencyCode.Equals(favorites.BaseCurrencyCode))))
		{
			throw new Exception("An error occured while adding favorites.");
		}

		_userDbContext.Favorites.Add(favorites);
		await _userDbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateFavoritesByNameAsync(FavoritesDto favoritesDto, string name, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = favoritesDto.Adapt<FavoritesCache>();
		FavoritesCache favoritesToUpdate = await _userDbContext.Favorites.SingleOrDefaultAsync(f => f.Name.Equals(name), cancellationToken)
										   ?? throw new Exception("An error occured while updating favorites.");

		// if (favoritesToUpdate.Equals(favorites)) return;

		if (_userDbContext.Favorites.Where(f => f.Name.Equals(name) == false)
			.Any(f => f.Name.Equals(favorites.Name)
					  || (f.CurrencyCode.Equals(favorites.CurrencyCode) && f.BaseCurrencyCode.Equals(favorites.BaseCurrencyCode))))
		{
			throw new Exception("An error occured while updating favorites.");
		}

		favoritesToUpdate.Name = favorites.Name;
		favoritesToUpdate.CurrencyCode = favorites.CurrencyCode;
		favoritesToUpdate.BaseCurrencyCode = favorites.BaseCurrencyCode;
		await _userDbContext.SaveChangesAsync(cancellationToken);
	}
}
