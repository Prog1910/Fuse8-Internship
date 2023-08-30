using Mapster;
using Protos;

namespace PublicApi.Common.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<CurrencyResponse, Contracts.CurrencyResponse>()
			.Map(dest => dest.Code, src => src.CurrencyType)
			.Map(dest => dest.Value, src => src.Value);
	}
}