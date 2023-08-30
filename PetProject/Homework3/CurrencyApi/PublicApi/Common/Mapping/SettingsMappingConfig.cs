using Mapster;
using Protos;

namespace PublicApi.Common.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<(string defaultCurrency, SettingsResponse settingsProtoResponse, int currencyRoundCound), Contracts.SettingsResponse>()
			.Map(dest => dest.DefaultCurrency, src => src.defaultCurrency)
			.Map(dest => dest.BaseCurrency, src => src.settingsProtoResponse.BaseCurrency)
			.Map(dest => dest.NewRequestsAvailable, src => src.settingsProtoResponse.NewRequestsAvailable)
			.Map(dest => dest.CurrencyRoundCount, src => src.currencyRoundCound);
	}
}