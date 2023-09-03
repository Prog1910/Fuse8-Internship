﻿using Domain.Aggregates;

namespace Application.Internal.Interfaces.Rest;

public interface ICacheRecalculationApi
{
	Task RecalculateCacheAsync(Guid cacheTaskId, CancellationToken cancellationToken);

	Task<Currency?> RecalculateCurrencyAsync(string currencyCode, string newBaseCurrencyCode, string oldBaseCurrencyCode, List<Currency> currencies, CancellationToken cancellationToken);
}