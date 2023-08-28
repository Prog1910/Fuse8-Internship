using Domain.Enums;

namespace Application.Common.Services.Common.Dtos;

public sealed record CurrencyDto(CurrencyType CurrencyType, decimal Value);