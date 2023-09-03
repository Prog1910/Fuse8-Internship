using Application.Internal.Interfaces.Rest;
using Domain.Errors;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
using Protos;
using System.Net;
using CurrencyType = Domain.Enums.CurrencyType;

namespace Infrastructure.Internal.Services.Grpc;

public sealed class CurrencyGrpcService : CurrencyGrpc.CurrencyGrpcBase
{
	private readonly ICacheCurrencyApi _cacheCurrencyService;

	public CurrencyGrpcService(ICacheCurrencyApi cacheCurrencyService)
	{
		_cacheCurrencyService = cacheCurrencyService;
	}

	public override async Task<CurrencyResponse> GetCurrentCurrency(CurrencyRequest request, ServerCallContext context)
	{
		try
		{
			var defaultCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			var currencyDto = await _cacheCurrencyService.GetCurrentCurrencyAsync(defaultCurrencyCode, context.CancellationToken);

			return currencyDto.Adapt<CurrencyResponse>();
		}
		catch (Exception exception)
		{
			throw GenerateRpcException(exception);
		}
	}

	public override async Task<CurrencyResponse> GetCurrencyOnDate(CurrencyOnDateRequest request, ServerCallContext context)
	{
		try
		{
			var defaultCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			var date = DateOnly.FromDateTime(request.Date.ToDateTime().ToUniversalTime());
			var currencyDto = await _cacheCurrencyService.GetCurrencyOnDateAsync(defaultCurrencyCode, date, context.CancellationToken);

			return currencyDto.Adapt<CurrencyResponse>();
		}
		catch (Exception exception)
		{
			throw GenerateRpcException(exception);
		}
	}

	public override async Task<CurrencyResponse> GetCurrentFavoriteCurrency(FavoriteCurrencyRequest request, ServerCallContext context)
	{
		try
		{
			var favoriteCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			var favoriteBaseCurrencyCode = (CurrencyType)request.BaseCurrencyCode;
			var currencyDto = await _cacheCurrencyService.GetCurrencyByFavoriteCurrenciesCodesAsync(favoriteCurrencyCode, favoriteBaseCurrencyCode, default, context.CancellationToken);

			return currencyDto.Adapt<CurrencyResponse>();
		}
		catch (Exception exception)
		{
			throw GenerateRpcException(exception);
		}
	}

	public override async Task<CurrencyResponse> GetFavoriteCurrencyOnDate(FavoriteCurrencyOnDateRequest request, ServerCallContext context)
	{
		try
		{
			var favoriteCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			var favoriteBaseCurrencyCode = (CurrencyType)request.BaseCurrencyCode;
			var date = DateOnly.FromDateTime(request.Date.ToDateTime().ToUniversalTime());
			var currencyDto = await _cacheCurrencyService.GetCurrencyByFavoriteCurrenciesCodesAsync(favoriteCurrencyCode, favoriteBaseCurrencyCode, date, context.CancellationToken);

			return currencyDto.Adapt<CurrencyResponse>();
		}
		catch (Exception exception)
		{
			throw GenerateRpcException(exception);
		}
	}

	public override async Task<SettingsResponse> GetSettings(Empty request, ServerCallContext context)
	{
		try
		{
			var settingsDto = await _cacheCurrencyService.GetSettingsAsync(context.CancellationToken);

			return settingsDto.Adapt<SettingsResponse>();
		}
		catch (Exception exception)
		{
			throw GenerateRpcException(exception);
		}
	}

	private static RpcException GenerateRpcException(Exception exception)
	{
		return exception switch
		{
			CurrencyNotFoundException => new RpcException(CreateStatus(HttpStatusCode.NotFound)),
			ApiRequestLimitException => new RpcException(CreateStatus(HttpStatusCode.TooManyRequests)),
			_ => new RpcException(CreateStatus(HttpStatusCode.InternalServerError))
		};

		Status CreateStatus(HttpStatusCode httpStatusCode) => new((StatusCode)httpStatusCode, exception.Message, exception);
	}
}