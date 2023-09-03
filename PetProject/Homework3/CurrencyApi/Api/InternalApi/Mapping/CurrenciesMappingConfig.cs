using Application.Shared.Dtos;
using Mapster;

namespace InternalApi.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<CurrencyDto, Protos.CurrencyResponse>()
			.Map(dest => dest.CurrencyCode, src => (Protos.CurrencyType)src.Code);
	}
}