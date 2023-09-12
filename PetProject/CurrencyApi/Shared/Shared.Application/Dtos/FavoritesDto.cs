using Shared.Domain.Enums;

namespace Shared.Application.Dtos;

public sealed record FavoritesDto(string Name, CurrencyType CurrencyCode, CurrencyType BaseCurrencyCode);