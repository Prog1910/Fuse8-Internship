using Application.Public.Interfaces.Rest;
using Application.Public.Persistence;
using Application.Shared.Dtos;
using Domain.Aggregates;
using Mapster;

namespace Infrastructure.Public.Services.Rest;

public sealed class FavoritesService : IFavoritesService
{
	private readonly IFavoritesRepository _favoritesRepo;

	public FavoritesService(IFavoritesRepository favoritesRepo)
	{
		_favoritesRepo = favoritesRepo;
	}

	public async Task DeleteFavoritesByNameAsync(string name)
	{
		await Task.Run(() =>
		{
			if (_favoritesRepo.TryDeleteFavoritesByName(name) is false)
				throw new Exception("The favorites not found.");
		});
	}

	public async Task<FavoritesDto?> GetFavoritesByNameAsync(string name)
		=> await Task.Run(() => _favoritesRepo.GetFavoritesByName(name)?.Adapt<FavoritesDto>()
		                        ?? throw new Exception("The favorites not found."));

	public async Task<IEnumerable<FavoritesDto>?> GetAllFavoritesAsync()
		=> await Task.Run(() => _favoritesRepo.GetAllFavorites()?.Adapt<IEnumerable<FavoritesDto>>()
		                        ?? throw new Exception("The favorites not found."));

	public async Task AddFavoritesAsync(FavoritesDto favoritesDto)
	{
		await Task.Run(() =>
		{
			var favorites = favoritesDto.Adapt<FavoritesCache>();
			if (_favoritesRepo.TryAddFavorites(favorites) is false)
				throw new Exception("The favorites already exists.");
		});
	}

	public async Task UpdateFavoritesByNameAsync(FavoritesDto favoritesDto, string name)
	{
		await Task.Run(() =>
		{
			var favorites = favoritesDto.Adapt<FavoritesCache>();
			if (_favoritesRepo.TryUpdateFavoritesByName(favorites, name) is false)
				throw new Exception("The favorites not found.");
		});
	}
}