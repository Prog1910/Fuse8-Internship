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

	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CancellationToken cancellationToken)
	{
		CurrencyResponse? currencyProto = await _grpcClient.GetCurrentCurrencyAsync(new CurrencyRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)_settings.DefaultCurrencyCode
		}, cancellationToken: cancellationToken);
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(DateOnly date, CancellationToken cancellationToken)
	{
		CurrencyResponse? currencyProto = await _grpcClient.GetCurrencyOnDateAsync(new CurrencyOnDateRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)_settings.DefaultCurrencyCode,
			Date = Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		}, cancellationToken: cancellationToken);
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetCurrentFavoritesByNameAsync(string name, CancellationToken cancellationToken)
	{
		FavoritesDto favoriteDto = await _favoritesService.GetFavoritesByNameAsync(name, cancellationToken)
								   ?? throw new Exception("The favorite currencies not found.");

		CurrencyResponse? currencyProto = await _grpcClient.GetCurrentFavoritesAsync(new FavoriteCurrencyRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)favoriteDto.CurrencyCode,
			BaseCurrencyCode = (CurrencyType)(int)favoriteDto.BaseCurrencyCode
		}, cancellationToken: cancellationToken);
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetFavoritesOnDateByNameAsync(string name, DateOnly date, CancellationToken cancellationToken)
	{
		FavoritesDto favoriteDto = await _favoritesService.GetFavoritesByNameAsync(name, cancellationToken)
								   ?? throw new Exception("The favorite currencies not found.");

		CurrencyResponse? currencyProto = await _grpcClient.GetFavoritesOnDateAsync(new FavoriteCurrencyOnDateRequest
		{
			DefaultCurrencyCode = (CurrencyType)(int)favoriteDto.CurrencyCode,
			BaseCurrencyCode = (CurrencyType)(int)favoriteDto.BaseCurrencyCode,
			Date = Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		}, cancellationToken: cancellationToken);
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return currencyProto.Adapt<CurrencyDto>();
	}

	public async Task<FullSettingsDto> GetSettingsAsync(CancellationToken cancellationToken)
	{
		SettingsResponse? settingsProto = await _grpcClient.GetSettingsAsync(new Empty(), cancellationToken: cancellationToken);

		return (_settings.DefaultCurrencyCode, settingsProto, _settings.CurrencyRoundCount).Adapt<FullSettingsDto>();
	}
}
