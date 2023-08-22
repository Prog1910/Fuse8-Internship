using CurrencyApi.Domain.Enums;

namespace CurrencyApi.Application.Common.Services.Common.Dtos;

public sealed record CurrencyDto(CurrencyType CurrencyType, decimal Value);