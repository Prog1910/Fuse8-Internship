using Domain.Enums;

namespace Application.Shared.Dtos;

public sealed record FavoriteCurrencyDto(string Name, CurrencyType Currency, CurrencyType BaseCurrency);