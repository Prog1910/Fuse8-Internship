using Domain.Enums;

namespace Application.Shared.Dtos;

public sealed record CurrencyDto(CurrencyType CurrencyType, decimal Value);