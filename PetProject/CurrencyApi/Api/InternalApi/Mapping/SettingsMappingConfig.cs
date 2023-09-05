using Application.Shared.Dtos;
using Mapster;

namespace InternalApi.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<SettingsDto, Protos.SettingsResponse>()
			.Map(dest => dest.BaseCurrencyCode, src => (Protos.CurrencyType)(int)src.BaseCurrencyCode);
	}
}