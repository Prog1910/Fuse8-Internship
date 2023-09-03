using Application.Public.Interfaces.Rest;
using Application.Shared.Dtos;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using Protos;

namespace Infrastructure.Public.Services.Rest;

public sealed class InternalService : IInternalApi
{
	private readonly IFavoritesService _favoritesService;
	private readonly CurrencyGrpc.CurrencyGrpcClient _grpcClient;
	private readonly ISettingsService _settings;

	public InternalService(IFavoritesService favoritesService, CurrencyGrpc.CurrencyGrpcClient grpcClient, ISettingsService settings)
	{
		_favoritesService = favoritesService;
		_grpcClient = grpcClient;
		_settings = settings;
	}

	public async Task<CurrencyDto> GetCurrentCurrencyAsync()
	{
		var currencyProto = await _grpcClient.GetCurrentCurrencyAsync(new CurrencyRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)_settings.DefaultCurrencyCode
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(DateOnly date)
	{
		var currencyProto = await _grpcClient.GetCurrencyOnDateAsync(new CurrencyOnDateRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)_settings.DefaultCurrencyCode,
			Date = Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrentFavoritesByNameAsync(string name)
	{
		var favoriteDto = await _favoritesService.GetFavoritesByNameAsync(name) ?? throw new Exception("The favorite currencies not found.");

		var currencyProto = await _grpcClient.GetCurrentFavoriteCurrencyAsync(new FavoriteCurrencyRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)favoriteDto.CurrencyCode,
			BaseCurrencyCode = (CurrencyType)(int)favoriteDto.BaseCurrencyCode
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetFavoritesOnDateByNameAsync(string name, DateOnly date)
	{
		var favoriteDto = await _favoritesService.GetFavoritesByNameAsync(name) ?? throw new Exception("The favorite currencies not found.");

		var currencyProto = await _grpcClient.GetFavoriteCurrencyOnDateAsync(new FavoriteCurrencyOnDateRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)favoriteDto.CurrencyCode,
			BaseCurrencyCode = (CurrencyType)(int)favoriteDto.BaseCurrencyCode,
			Date = Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<FullSettingsDto> GetSettingsAsync()
	{
		var settingsProto = await _grpcClient.GetSettingsAsync(new Empty());

		return (_settings.DefaultCurrencyCode, settingsProto, _settings.CurrencyRoundCount).Adapt<FullSettingsDto>();
	}
}