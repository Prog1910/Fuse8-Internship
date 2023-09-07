using Application.Public.Interfaces.Rest;
using Application.Public.Persistence;
using Application.Shared.Dtos;
using Domain.Aggregates;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Public.Services.Rest;

public sealed class FavoritesService : IFavoritesService
{
	private readonly IUserDbContext _userDbContext;

	public FavoritesService(IUserDbContext userDbContext)
	{
		_userDbContext = userDbContext;
	}

	public async Task DeleteFavoritesByNameAsync(string name, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = await _userDbContext.Favorites.SingleOrDefaultAsync(f => f.Name.Equals(name), cancellationToken)
								   ?? throw new Exception("An error occured while deleting favorites.");
		_userDbContext.Favorites.Remove(favorites);
		await _userDbContext.SaveChangesAsync();
	}

	public async Task<FavoritesDto?> GetFavoritesByNameAsync(string name, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = await _userDbContext.Favorites.SingleOrDefaultAsync(f => f.Name.Equals(name), cancellationToken)
								   ?? throw new Exception("An error occured while getting favorites.");

		return favorites.Adapt<FavoritesDto>();
	}

	public async Task<List<FavoritesDto>> GetAllFavoritesAsync()
	{
		List<FavoritesCache> allFavorites = await _userDbContext.Favorites.ToListAsync() ?? throw new Exception("An error occurred while getting favorites.");
		return allFavorites.Adapt<List<FavoritesDto>>();
	}

	public async Task AddFavoritesAsync(FavoritesDto favoritesDto, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = favoritesDto.Adapt<FavoritesCache>();
		if (_userDbContext.Favorites.Any(
				f => f.Name.Equals(favorites.Name)
					 || f.CurrencyCode.Equals(favorites.CurrencyCode) && f.BaseCurrencyCode.Equals(favorites.BaseCurrencyCode)))
			throw new Exception("An error occured while adding favorites.");

		_userDbContext.Favorites.Add(favorites);
		await _userDbContext.SaveChangesAsync();
	}

	public async Task UpdateFavoritesByNameAsync(FavoritesDto favoritesDto, string name, CancellationToken cancellationToken)
	{
		FavoritesCache favorites = favoritesDto.Adapt<FavoritesCache>();
		FavoritesCache favoritesToUpdate = await _userDbContext.Favorites.SingleOrDefaultAsync(f => f.Name.Equals(name), cancellationToken) ?? throw new Exception("An error occured while updating favorites.");

		// if (favoritesToUpdate.Equals(favorites)) return;

		if (_userDbContext.Favorites.Where(f => f.Name.Equals(name) == false)
			.Any(f => f.Name.Equals(favorites.Name)
					  || f.CurrencyCode.Equals(favorites.CurrencyCode) && f.BaseCurrencyCode.Equals(favorites.BaseCurrencyCode)))
			throw new Exception("An error occured while updating favorites.");

		favoritesToUpdate.Name = favorites.Name;
		favoritesToUpdate.CurrencyCode = favorites.CurrencyCode;
		favoritesToUpdate.BaseCurrencyCode = favorites.BaseCurrencyCode;
		await _userDbContext.SaveChangesAsync();
	}
}