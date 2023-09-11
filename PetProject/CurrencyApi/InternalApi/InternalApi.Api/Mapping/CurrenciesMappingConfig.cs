using Mapster;
using Protos;
using Shared.Application.Dtos;

namespace InternalApi.Api.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<CurrencyDto, CurrencyResponse>()
			.Map(dest => dest.CurrencyCode, src => (CurrencyType)src.Code);
	}
}
