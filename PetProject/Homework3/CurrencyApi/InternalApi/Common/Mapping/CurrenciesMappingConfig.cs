using Application.Common.Services.Rest.Common.Dtos;
using Contracts;
using Domain.Aggregates.CurrencyAggregate;
using Domain.Enums;
using Mapster;

namespace InternalApi.Common.Mapping;

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