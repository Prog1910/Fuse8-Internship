using Application.Common.Interfaces;
using Grpc.Core;
using MapsterMapper;
using Protos;

namespace Infrastructure.Services;

public sealed class CurrencyGrpcService : CurrencyGrpc.CurrencyGrpcBase
{
	private readonly ICachedCurrencyApi _cachedCurrenyService;
	private readonly IMapper _mapper;

	public CurrencyGrpcService(ICachedCurrencyApi cachedCurrenyService, IMapper mapper)
	{
		_cachedCurrenyService = cachedCurrenyService;
		_mapper = mapper;
	}

	public override async Task<CurrencyResponse> GetCurrentCurrency(CurrencyRequest request, ServerCallContext context)
	{
		var currencyType = (Domain.Enums.CurrencyType)request.CurrencyType;
		var currencyDto = await _cachedCurrenyService.GetCurrentCurrencyAsync(currencyType, context.CancellationToken);

		return _mapper.Map<CurrencyResponse>(currencyDto);
	}

	public override async Task<CurrencyResponse> GetCurrencyOnDate(CurrencyOnDateRequest request, ServerCallContext context)
	{
		var currencyType = (Domain.Enums.CurrencyType)request.CurrencyType;
		var date = DateOnly.FromDateTime(request.Date.ToDateTime().ToUniversalTime());
		var currencyDto = await _cachedCurrenyService.GetCurrencyOnDateAsync(currencyType, date, context.CancellationToken);

		return _mapper.Map<CurrencyResponse>(currencyDto);
	}

	public override async Task<SettingsResponse> GetSettings(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
	{
		var settingsDto = await _cachedCurrenyService.GetSettingsAsync(context.CancellationToken);

		return _mapper.Map<SettingsResponse>(settingsDto);
	}
}