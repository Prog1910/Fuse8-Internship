using Domain.Enums;

namespace Application.Shared.Dtos;

public sealed record CurrencyDto(CurrencyType Code, decimal Value);