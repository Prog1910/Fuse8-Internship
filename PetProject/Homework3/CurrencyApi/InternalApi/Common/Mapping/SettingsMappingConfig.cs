using Application.Common.Services.Common.Dtos;
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
				.Map(dest => dest.DefaultCurrency, src => Enum.Parse<CurrencyType>(src.DefaultCurrency))
				.Map(dest => dest.BaseCurrency, src => Enum.Parse<CurrencyType>(src.BaseCurrency));

		config.NewConfig<SettingsDto, SettingsResponse>()
			.Map(dest => dest.DefaultCurrency, src => src.DefaultCurrency.ToString())
			.Map(dest => dest.BaseCurrency, src => src.BaseCurrency.ToString());
	}
}