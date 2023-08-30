using Domain.Enums;

namespace Application.Common.Services.Rest.Common.Dtos;

public sealed record CurrencyDto(CurrencyType CurrencyType, decimal Value);