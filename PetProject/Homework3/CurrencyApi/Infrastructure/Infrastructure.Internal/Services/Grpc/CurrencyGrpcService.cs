using Application.Internal.Interfaces.Rest;
using Domain.Errors;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using Protos;
using System.Net;

namespace Infrastructure.Internal.Services.Grpc;

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
		try
		{
			var defaultCurrency = (Domain.Enums.CurrencyType)request.DefaultCurrency;
			var currencyDto = await _cachedCurrenyService.GetCurrentCurrencyAsync(defaultCurrency, context.CancellationToken);

			return _mapper.Map<CurrencyResponse>(currencyDto);
		}
		catch (CurrencyNotFoundException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.NotFound, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(CurrencyNotFoundException)
					}
				});
		}
		catch (ApiRequestLimitException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.TooManyRequests, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(ApiRequestLimitException)
					}
				});
		}
		catch (Exception ex)
		{
			throw new RpcException(
				Status.DefaultCancelled,
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? string.Empty
					}
				});
		}
	}

	public override async Task<CurrencyResponse> GetCurrencyOnDate(CurrencyOnDateRequest request, ServerCallContext context)
	{
		try
		{
			var defaultCurrency = (Domain.Enums.CurrencyType)request.DefaultCurrency;
			var date = DateOnly.FromDateTime(request.Date.ToDateTime().ToUniversalTime());
			var currencyDto = await _cachedCurrenyService.GetCurrencyOnDateAsync(defaultCurrency, date, context.CancellationToken);

			return _mapper.Map<CurrencyResponse>(currencyDto);
		}
		catch (CurrencyNotFoundException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.NotFound, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(CurrencyNotFoundException)
					}
				});
		}
		catch (ApiRequestLimitException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.TooManyRequests, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(ApiRequestLimitException)
					}
				});
		}
		catch (Exception ex)
		{
			throw new RpcException(
				Status.DefaultCancelled,
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? string.Empty
					}
				});
		}
	}

	public override async Task<CurrencyResponse> GetCurrentFavoriteCurrency(FavoriteCurrencyRequest request, ServerCallContext context)
	{
		try
		{
			var defaultCurrency = (Domain.Enums.CurrencyType)request.DefaultCurrency;
			var baseCurrency = (Domain.Enums.CurrencyType)request.BaseCurrency;
			var currencyDto = await _cachedCurrenyService.GetCurrentFavoriteCurrencyAsync(defaultCurrency, baseCurrency, context.CancellationToken);

			return _mapper.Map<CurrencyResponse>(currencyDto);
		}
		catch (CurrencyNotFoundException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.NotFound, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(CurrencyNotFoundException)
					}
				});
		}
		catch (ApiRequestLimitException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.TooManyRequests, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(ApiRequestLimitException)
					}
				});
		}
		catch (Exception ex)
		{
			throw new RpcException(
				Status.DefaultCancelled,
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? string.Empty
					}
				});
		}
	}

	public override async Task<CurrencyResponse> GetFavoriteCurrencyOnDate(FavoriteCurrencyOnDateRequest request, ServerCallContext context)
	{
		try
		{
			var defaultCurrency = (Domain.Enums.CurrencyType)request.DefaultCurrency;
			var baseCurrency = (Domain.Enums.CurrencyType)request.BaseCurrency;
			var date = DateOnly.FromDateTime(request.Date.ToDateTime().ToUniversalTime());
			var currencyDto = await _cachedCurrenyService.GetFavoriteCurrencyOnDateAsync(defaultCurrency, baseCurrency, date, context.CancellationToken);

			return _mapper.Map<CurrencyResponse>(currencyDto);
		}
		catch (CurrencyNotFoundException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.NotFound, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(CurrencyNotFoundException)
					}
				});
		}
		catch (ApiRequestLimitException ex)
		{
			throw new RpcException(
				new Status((StatusCode)HttpStatusCode.TooManyRequests, ex.Message),
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? nameof(ApiRequestLimitException)
					}
				});
		}
		catch (Exception ex)
		{
			throw new RpcException(
				Status.DefaultCancelled,
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? string.Empty
					}
				});
		}
	}

	public override async Task<SettingsResponse> GetSettings(Empty request, ServerCallContext context)
	{
		try
		{
			var settingsDto = await _cachedCurrenyService.GetSettingsAsync(context.CancellationToken);

			return _mapper.Map<SettingsResponse>(settingsDto);
		}
		catch (Exception ex)
		{
			throw new RpcException(
				Status.DefaultCancelled,
				new Metadata
				{
					{
						"ExceptionType", ex?.GetType().Name ?? string.Empty
					}
				});
		}
	}
}