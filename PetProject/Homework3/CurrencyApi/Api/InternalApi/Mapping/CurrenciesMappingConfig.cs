using Application.Shared.Dtos;
using Contracts;
using Domain.Aggregates;
using Domain.Enums;
using Mapster;

namespace InternalApi.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, CurrencyDto>()
			.Map(dest => dest.CurrencyType, src => Enum.Parse<CurrencyType>(src.Code));

		config.NewConfig<CurrencyDto, Protos.CurrencyResponse>()
			.Map(dest => dest.CurrencyType, src => (Protos.CurrencyType)(src.CurrencyType));
	}
}