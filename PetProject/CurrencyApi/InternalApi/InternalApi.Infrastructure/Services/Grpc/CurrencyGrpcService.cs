using System.Net;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalApi.Application.Interfaces.Rest;
using Mapster;
using Protos;
using Shared.Application.Dtos;
using Shared.Domain.Errors;
using CurrencyType = Shared.Domain.Enums.CurrencyType;

namespace InternalApi.Infrastructure.Services.Grpc;

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
			CurrencyType defaultCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			CurrencyDto currencyDto = await _cacheCurrencyService.GetCurrentCurrencyAsync(defaultCurrencyCode, context.CancellationToken);

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
			CurrencyType defaultCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			DateOnly date = DateOnly.FromDateTime(request.Date.ToDateTime().ToUniversalTime());
			CurrencyDto currencyDto = await _cacheCurrencyService.GetCurrencyOnDateAsync(defaultCurrencyCode, date, context.CancellationToken);

			return currencyDto.Adapt<CurrencyResponse>();
		}
		catch (Exception exception)
		{
			throw GenerateRpcException(exception);
		}
	}

	public override async Task<CurrencyResponse> GetCurrentFavorites(FavoriteCurrencyRequest request, ServerCallContext context)
	{
		try
		{
			CurrencyType favoriteCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			CurrencyType favoriteBaseCurrencyCode = (CurrencyType)request.BaseCurrencyCode;
			CurrencyDto currencyDto =
				await _cacheCurrencyService.GetCurrencyByFavoritesAsync(favoriteCurrencyCode, favoriteBaseCurrencyCode, default, context.CancellationToken);

			return currencyDto.Adapt<CurrencyResponse>();
		}
		catch (Exception exception)
		{
			throw GenerateRpcException(exception);
		}
	}

	public override async Task<CurrencyResponse> GetFavoritesOnDate(FavoriteCurrencyOnDateRequest request, ServerCallContext context)
	{
		try
		{
			CurrencyType favoriteCurrencyCode = (CurrencyType)request.DefaultCurrencyCode;
			CurrencyType favoriteBaseCurrencyCode = (CurrencyType)request.BaseCurrencyCode;
			DateOnly date = DateOnly.FromDateTime(request.Date.ToDateTime().ToUniversalTime());
			CurrencyDto currencyDto =
				await _cacheCurrencyService.GetCurrencyByFavoritesAsync(favoriteCurrencyCode, favoriteBaseCurrencyCode, date, context.CancellationToken);

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
			SettingsDto settingsDto = await _cacheCurrencyService.GetSettingsAsync(context.CancellationToken);

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

		Status CreateStatus(HttpStatusCode httpStatusCode)
		{
			return new Status((StatusCode)httpStatusCode, exception.Message, exception);
		}
	}
}