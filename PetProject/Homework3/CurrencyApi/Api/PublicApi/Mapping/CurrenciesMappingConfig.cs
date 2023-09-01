using Application.Shared.Dtos;
using Domain.Enums;
using Mapster;

namespace PublicApi.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Protos.CurrencyResponse, CurrencyDto>()
			.Map(dest => dest.CurrencyType, src => Enum.Parse<CurrencyType>(src.CurrencyType));

		config.NewConfig<CurrencyDto, Contracts.CurrencyResponse>()
			.Map(dest => dest.Code, src => src.CurrencyType.ToString());
	}
}