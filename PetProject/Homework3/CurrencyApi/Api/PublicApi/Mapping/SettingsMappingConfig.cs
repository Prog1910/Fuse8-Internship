using Application.Shared.Dtos;
using Mapster;

namespace PublicApi.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<(string DefaultCurrency, Protos.SettingsResponse settingsProto, int CurrencyRoundCount), FullSettingsDto>()
			.Map(dest => dest.DefaultCurrency, src => src.DefaultCurrency)
			.Map(dest => dest.BaseCurrency, src => src.settingsProto.BaseCurrency.ToString())
			.Map(dest => dest.NewRequestsAvailable, src => src.settingsProto.NewRequestsAvailable)
			.Map(dest => dest.CurrencyRoundCount, src => src.CurrencyRoundCount);

		config.NewConfig<FullSettingsDto, Contracts.SettingsResponse>();
	}
}