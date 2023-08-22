using CurrencyApi.Application.Common.Services.Common.Dtos;
using CurrencyApi.Contracts;
using CurrencyApi.Domain.Aggregates.CurrencyAggregate;
using CurrencyApi.Domain.Enums;
using Mapster;

namespace CurrencyApi.InternalApi.Common.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, CurrencyDto>()
			.Map(dest => dest.CurrencyType, src => Enum.Parse<CurrencyType>(src.Code));

		config.NewConfig<CurrencyDto, CurrencyResponse>()
			.Map(dest => dest.Code, src => src.CurrencyType.ToString());
	}
}