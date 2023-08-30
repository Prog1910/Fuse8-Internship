using Application.Common.Interfaces.Rest;
using Contracts;
using Domain.Enums;
using Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Rest;

public sealed class InternalService : IInternalApi
{
	private readonly IFavoriteCurrencyService _favoriteCurrencyService;
	private readonly Protos.CurrencyGrpc.CurrencyGrpcClient _grpcClient;
	private readonly ISettingsReaderService _settings;
	private readonly IMapper _mapper;

	public InternalService(
		IFavoriteCurrencyService favoriteCurrencyService,
		Protos.CurrencyGrpc.CurrencyGrpcClient grpcClient,
		ISettingsReaderService settings,
		IMapper mapper)
	{
		_favoriteCurrencyService = favoriteCurrencyService;
		_grpcClient = grpcClient;
		_settings = settings;
		_mapper = mapper;
	}

	public async Task<CurrencyResponse> GetCurrentCurrencyAsync()
	{
		var currencyProtoResponse = await _grpcClient.GetCurrentCurrencyAsync(new Protos.CurrencyRequest
		{
			DefaultCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_settings.DefaultCurrency),
		});
		currencyProtoResponse.Value = Math.Round(currencyProtoResponse.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<CurrencyResponse> GetCurrencyOnDateAsync(DateOnly date)
	{
		var currencyProtoResponse = await _grpcClient.GetCurrencyOnDateAsync(new Protos.CurrencyOnDateRequest
		{
			DefaultCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_settings.DefaultCurrency),
			Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		});
		currencyProtoResponse.Value = Math.Round(currencyProtoResponse.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<CurrencyResponse> GetCurrentFavoriteCurrencyByNameAsync(string name)
	{
		var favoriteDto = await _favoriteCurrencyService.GetFavoriteCurrencyByNameAsync(name) ?? throw new Exception("The favorite currency not found.");

		var currencyProtoResponse = await _grpcClient.GetCurrentFavoriteCurrencyAsync(new Protos.FavoriteCurrencyRequest
		{
			DefaultCurrency = (Protos.CurrencyType)favoriteDto.Currency,
			BaseCurrency = (Protos.CurrencyType)favoriteDto.BaseCurrency,
		});
		currencyProtoResponse.Value = Math.Round(currencyProtoResponse.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<CurrencyResponse> GetFavoriteCurrencyOnDateByNameAsync(string name, DateOnly date)
	{
		var favoriteDto = await _favoriteCurrencyService.GetFavoriteCurrencyByNameAsync(name) ?? throw new Exception("The favorite currency not found.");

		var currencyProtoResponse = await _grpcClient.GetFavoriteCurrencyOnDateAsync(new Protos.FavoriteCurrencyOnDateRequest
		{
			DefaultCurrency = (Protos.CurrencyType)favoriteDto.Currency,
			BaseCurrency = (Protos.CurrencyType)favoriteDto.BaseCurrency,
			Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		});
		currencyProtoResponse.Value = Math.Round(currencyProtoResponse.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<SettingsResponse> GetSettingsAsync()
	{
		var settingsProtoResponse = await _grpcClient.GetSettingsAsync(new Google.Protobuf.WellKnownTypes.Empty());

		return _mapper.Map<SettingsResponse>((_settings.DefaultCurrency, settingsProtoResponse, _settings.CurrencyRoundCount));
	}
}