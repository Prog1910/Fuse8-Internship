using Application.Public.Interfaces.Rest;
using Application.Shared.Dtos;
using Domain.Enums;
using MapsterMapper;

namespace Infrastructure.Public.Services.Rest;

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

	public async Task<CurrencyDto> GetCurrentCurrencyAsync()
	{
		var currencyProto = await _grpcClient.GetCurrentCurrencyAsync(new Protos.CurrencyRequest
		{
			DefaultCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_settings.DefaultCurrency)
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyDto>(currencyProto);
	}

	public async Task<CurrencyDto> GetCurrencyOnDateAsync(DateOnly date)
	{
		var currencyProto = await _grpcClient.GetCurrencyOnDateAsync(new Protos.CurrencyOnDateRequest
		{
			DefaultCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_settings.DefaultCurrency),
			Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyDto>(currencyProto);
	}

	public async Task<CurrencyDto> GetCurrentFavoriteCurrencyByNameAsync(string name)
	{
		var favoriteDto = await _favoriteCurrencyService.GetFavoriteCurrencyByNameAsync(name) ?? throw new Exception("The favorite currency not found.");

		var currencyProto = await _grpcClient.GetCurrentFavoriteCurrencyAsync(new Protos.FavoriteCurrencyRequest
		{
			DefaultCurrency = (Protos.CurrencyType)favoriteDto.Currency,
			BaseCurrency = (Protos.CurrencyType)favoriteDto.BaseCurrency
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyDto>(currencyProto);
	}

	public async Task<CurrencyDto> GetFavoriteCurrencyOnDateByNameAsync(string name, DateOnly date)
	{
		var favoriteDto = await _favoriteCurrencyService.GetFavoriteCurrencyByNameAsync(name) ?? throw new Exception("The favorite currency not found.");

		var currencyProto = await _grpcClient.GetFavoriteCurrencyOnDateAsync(new Protos.FavoriteCurrencyOnDateRequest
		{
			DefaultCurrency = (Protos.CurrencyType)favoriteDto.Currency,
			BaseCurrency = (Protos.CurrencyType)favoriteDto.BaseCurrency,
			Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc))
		});
		currencyProto.Value = Math.Round(currencyProto.Value, _settings.CurrencyRoundCount);

		return _mapper.Map<CurrencyDto>(currencyProto);
	}

	public async Task<FullSettingsDto> GetSettingsAsync()
	{
		var settingsProto = await _grpcClient.GetSettingsAsync(new Google.Protobuf.WellKnownTypes.Empty());

		return _mapper.Map<FullSettingsDto>((_settings.DefaultCurrency, settingsProto, _settings.CurrencyRoundCount));
	}
}