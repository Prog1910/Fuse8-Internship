﻿using Application.Shared.Dtos;
using Domain.Enums;

namespace Application.Internal.Interfaces.Rest;

public interface ICachedCurrencyApi
{
	Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType defaultCurrency, CancellationToken cancellationToken);

	Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType defaultCurrency, DateOnly date, CancellationToken cancellationToken);

	Task<CurrencyDto> GetCurrentFavoriteCurrencyAsync(CurrencyType defaultCurrency, CurrencyType baseCurrency, CancellationToken cancellationToken);

	Task<CurrencyDto> GetFavoriteCurrencyOnDateAsync(CurrencyType defaultCurrency, CurrencyType baseCurrency, DateOnly date, CancellationToken cancellationToken);

	Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken);
}