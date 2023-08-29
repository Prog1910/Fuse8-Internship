using Application.Common.Interfaces;
using Application.Persistence;
using Contracts;
using Domain.Enums;
using Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public sealed class InternalService : IInternalApi
{
	private readonly PublicApiOptions _options;
	private readonly Protos.CurrencyGrpc.CurrencyGrpcClient _grpcClient;
	private readonly IMapper _mapper;

	public InternalService(IOptionsSnapshot<PublicApiOptions> options, Protos.CurrencyGrpc.CurrencyGrpcClient grpcClient, IMapper mapper)
	{
		_options = options.Value;
		_grpcClient = grpcClient;
		_mapper = mapper;
	}

	public async Task<CurrencyResponse> GetCurrentCurrencyAsync()
	{
		var currencyProtoResponse = await _grpcClient.GetCurrentCurrencyAsync(new()
		{
			DefaultCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_options.DefaultCurrency),
			BaseCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_options.BaseCurrency),
		});
		currencyProtoResponse.Value = Math.Round(currencyProtoResponse.Value, _options.CurrencyRoundCount);

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<CurrencyResponse> GetCurrencyOnDateAsync(DateOnly date)
	{
		var currencyProtoResponse = await _grpcClient.GetCurrencyOnDateAsync(new()
		{
			DefaultCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_options.DefaultCurrency),
			BaseCurrency = (Protos.CurrencyType)Enum.Parse<CurrencyType>(_options.BaseCurrency),
			Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc)),
		});
		currencyProtoResponse.Value = Math.Round(currencyProtoResponse.Value, _options.CurrencyRoundCount);

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<SettingsResponse> GetSettingsAsync()
	{
		var settingsProtoResponse = await _grpcClient.GetSettingsAsync(new());

		return _mapper.Map<SettingsResponse>((_options.DefaultCurrency, settingsProtoResponse, _options.CurrencyRoundCount));
	}
}
