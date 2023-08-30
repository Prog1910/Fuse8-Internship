using Domain.Enums;

namespace Application.Common.Services.Rest.Common.Dtos;

public sealed record FavoriteCurrencyDto(string Name, CurrencyType Currency, CurrencyType BaseCurrency);