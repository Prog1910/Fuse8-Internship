namespace Contracts;

/// <summary>
///     Represents a response containing favorite currency information.
/// </summary>
/// <param name="Name">The of favorite currency.</param>
/// <param name="Currency"> The default currency for the favorite.</param>
/// <param name="BaseCurrency">The base currency for the favorite.</param>
public sealed record FavoriteCurrencyResponse(string Name, string Currency, string BaseCurrency);