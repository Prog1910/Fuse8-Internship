using Application.Common.Services.Rest.Common.Dtos;
using Contracts;
using Domain.Aggregates.SettingsAggregate;
using Domain.Enums;
using Mapster;

namespace InternalApi.Common.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Settings, SettingsDto>()
			.Map(dest => dest.BaseCurrency, src => Enum.Parse<CurrencyType>(src.BaseCurrency));

		config.NewConfig<SettingsDto, SettingsResponse>()
			.Map(dest => dest.BaseCurrency, src => src.BaseCurrency.ToString());
	}
}