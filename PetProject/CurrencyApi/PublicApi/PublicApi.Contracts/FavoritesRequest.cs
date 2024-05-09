using Shared.Contracts.Enums;

namespace PublicApi.Contracts;

/// <summary> Represents a request for managing favorite currencies. </summary>
/// <param name="Name"> The name of favorite currencies. </param>
/// <param name="CurrencyCode"> The default currency for the favorites. </param>
/// <param name="BaseCurrencyCode"> The base currency for the favorites. </param>
public sealed record FavoritesRequest(string Name, CurrencyType CurrencyCode, CurrencyType BaseCurrencyCode);
