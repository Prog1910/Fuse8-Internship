using Application.Public.Interfaces.Rest;
using Application.Public.Persistence;
using Application.Shared.Dtos;
using Domain.Aggregates;
using MapsterMapper;

namespace Infrastructure.Public.Services.Rest;

public sealed class FavoriteCurrencyService : IFavoriteCurrencyService
{
	private readonly IFavoriteCurrenciesRepository _favoritesRepo;
	private readonly IMapper _mapper;

	public FavoriteCurrencyService(IFavoriteCurrenciesRepository favoritesRepo, IMapper mapper)
	{
		_favoritesRepo = favoritesRepo;
		_mapper = mapper;
	}

	public async Task DeleteFavoriteCurrencyByNameAsync(string name)
	{
		await Task.CompletedTask;
		if (_favoritesRepo.TryDeleteFavoriteCurrencyByName(name) == false)
			throw new Exception("The favorite currency not found.");
	}

	public async Task<FavoriteCurrencyDto?> GetFavoriteCurrencyByNameAsync(string name)
	{
		await Task.CompletedTask;
		var favorite = _favoritesRepo.GetFavoriteCurrencyByName(name);

		return favorite is not null ? _mapper.Map<FavoriteCurrencyDto>(favorite) : null;
	}

	public async Task<List<FavoriteCurrencyDto>?> GetAllFavoriteCurrenciesAsync()
	{
		await Task.CompletedTask;
		var favorites = _favoritesRepo.GetAllFavoriteCurrencies();

		return favorites is not null ? _mapper.Map<List<FavoriteCurrencyDto>>(favorites) : null;
	}

	public async Task AddFavoriteCurrencyAsync(FavoriteCurrencyDto favoriteCurrencyDto)
	{
		await Task.CompletedTask;

		var favoriteCurrency = _mapper.Map<CachedFavoriteCurrency>(favoriteCurrencyDto);
		if (_favoritesRepo.TryAddFavoriteCurrencies(favoriteCurrency) == false)
			throw new Exception("The favorite currency already exists.");
	}

	public async Task UpdateFavoriteCurrencyByNameAsync(FavoriteCurrencyDto favoriteCurrencyDto)
	{
		await Task.CompletedTask;
		var favoriteCurrency = _mapper.Map<CachedFavoriteCurrency>(favoriteCurrencyDto);
		if (_favoritesRepo.TryUpdateFavoriteCurrencyByName(favoriteCurrency))
			throw new Exception("The favorite currency not found.");
	}
}