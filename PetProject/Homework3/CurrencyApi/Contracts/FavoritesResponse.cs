namespace Contracts;

/// <summary>
///     Represents a response containing favorite currency information.
/// </summary>
/// <param name="Name">The of favorite currency.</param>
/// <param name="CurrencyCode"> The default currency for the favorite.</param>
/// <param name="BaseCurrencyCode">The base currency for the favorite.</param>
public sealed record FavoritesResponse(string Name, string CurrencyCode, string BaseCurrencyCode);