using CurrencyApi.Application.Common.Services.Interfaces;
using CurrencyApi.Contracts;
using CurrencyApi.Domain.Enums;
using CurrencyApi.Domain.Options;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace CurrencyApi.Infrastructure.Services;

public sealed class InternalService : IInternalApi
{
	private readonly CurrencyApiOptions _options;
	private readonly Protos.CurrencyGrpc.CurrencyGrpcClient _grpcClient;
	private readonly IMapper _mapper;

	public InternalService(IOptionsSnapshot<CurrencyApiOptions> options, Protos.CurrencyGrpc.CurrencyGrpcClient grpcClient, IMapper mapper)
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

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<CurrencyResponse> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date)
	{
		var currencyProtoResponse = await _grpcClient.GetCurrencyOnDateAsync(new()
		{
			CurrencyType = (Protos.CurrencyType)currencyType,
			Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(date.ToDateTime(TimeOnly.MinValue))
		});

		return _mapper.Map<CurrencyResponse>(currencyProtoResponse);
	}

	public async Task<SettingsResponse> GetSettingsAsync()
	{
		var settingsProtoResponse = await _grpcClient.GetSettingsAsync(new());

		return _mapper.Map<SettingsResponse>((_options.DefaultCurrency, settingsProtoResponse, _options.CurrencyRoundCount));
	}
}