using Application.Shared.Dtos;
using Mapster;
using Protos;

namespace InternalApi.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<CurrencyDto, CurrencyResponse>()
			.Map(dest => dest.CurrencyCode, src => (CurrencyType)src.Code);
	}
}
