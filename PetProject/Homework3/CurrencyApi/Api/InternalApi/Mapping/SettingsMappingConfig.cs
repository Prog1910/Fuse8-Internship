using Application.Shared.Dtos;
using Contracts;
using Domain.Aggregates;
using Domain.Enums;
using Mapster;

namespace InternalApi.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Settings, SettingsDto>()
			.Map(dest => dest.BaseCurrency, src => Enum.Parse<CurrencyType>(src.BaseCurrency));

		config.NewConfig<SettingsDto, Protos.SettingsResponse>()
			.Map(dest => dest.BaseCurrency, src => (Protos.CurrencyType)src.BaseCurrency);
	}
}