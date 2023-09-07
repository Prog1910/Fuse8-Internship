using Application.Shared.Dtos;
using Mapster;
using Protos;
using CurrencyType = Domain.Enums.CurrencyType;

namespace PublicApi.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<(CurrencyType DefaultCurrencyCode, SettingsResponse settingsProto, int CurrencyRoundCount), FullSettingsDto>()
			.Map(dest => dest.DefaultCurrencyCode, src => src.DefaultCurrencyCode.ToString())
			.Map(dest => dest.BaseCurrencyCode, src => src.settingsProto.BaseCurrencyCode)
			.Map(dest => dest.NewRequestsAvailable, src => src.settingsProto.NewRequestsAvailable)
			.Map(dest => dest.CurrencyRoundCount, src => src.CurrencyRoundCount);
	}
}
