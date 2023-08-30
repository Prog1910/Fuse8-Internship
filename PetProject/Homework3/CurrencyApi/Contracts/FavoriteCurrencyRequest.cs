using Contracts.Enums;

namespace Contracts;

/// <summary>
///     Represents a request for managing favorite currencies.
/// </summary>
/// <param name="Name">The of favorite currency.</param>
/// <param name="Currency"> The default currency for the favorite.</param>
/// <param name="BaseCurrency">The base currency for the favorite.</param>
public sealed record FavoriteCurrencyRequest(string Name, CurrencyTypeDto Currency, CurrencyTypeDto BaseCurrency);