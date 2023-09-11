using Domain.Enums;

namespace Shared.Application.Dtos;

public sealed record CurrencyDto(CurrencyType Code, decimal Value);
