using Domain.Enums;

namespace Application.Common.Services.Rest.Common.Dtos;

public sealed record SettingsDto(
    CurrencyType BaseCurrency,
    bool NewRequestsAvailable);