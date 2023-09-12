using Mapster;
using Protos;
using Shared.Application.Dtos;

namespace InternalApi.Api.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<SettingsDto, SettingsResponse>()
			.Map(dest => dest.BaseCurrencyCode, src => (CurrencyType)(int)src.BaseCurrencyCode);
	}
}
