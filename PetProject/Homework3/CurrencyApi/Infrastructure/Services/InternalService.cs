using Application.Common.Interfaces;
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

	public async Task<CurrencyResponse> GetCurrentCurrencyAsync(CurrencyType currencyType)
	{
		var currencyProtoResponse = await _grpcClient.GetCurrentCurrencyAsync(new()
		{
			CurrencyType = (Protos.CurrencyType)currencyType
		});

		currencyProtoResponse.Value = Math.Round(currencyProtoResponse.Value, _options.CurrencyRoundCount);

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<CurrencyResponse> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date)
	{
		var currencyProtoResponse = await _grpcClient.GetCurrencyOnDateAsync(new()
		{
			CurrencyType = (Protos.CurrencyType)currencyType,
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

	public Task<SettingsResponse> UpdateSettingsAsync(CurrencyType defaultCurrency, int currencyRoundCount)
	{
		throw new NotImplementedException();
	}
}
