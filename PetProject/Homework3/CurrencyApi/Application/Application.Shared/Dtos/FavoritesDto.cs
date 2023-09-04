using Domain.Enums;

namespace Application.Shared.Dtos;

public sealed record FavoritesDto(string Name, CurrencyType CurrencyCode, CurrencyType BaseCurrencyCode);