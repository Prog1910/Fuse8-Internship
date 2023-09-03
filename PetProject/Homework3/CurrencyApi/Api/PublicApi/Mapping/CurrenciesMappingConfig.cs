using Application.Shared.Dtos;
using Mapster;
using Protos;
using CurrencyType = Domain.Enums.CurrencyType;

namespace PublicApi.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<CurrencyResponse, CurrencyDto>()
			.Map(dest => dest.Code, src => Enum.Parse<CurrencyType>(src.CurrencyCode));

		config.NewConfig<CurrencyDto, Contracts.CurrencyResponse>()
			.Map(dest => dest.Code, src => src.Code.ToString());
	}
}