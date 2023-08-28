using Contracts;
using Mapster;

namespace PublicApi.Common.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Protos.CurrencyResponse, CurrencyResponse>()
			.Map(dest => dest.Code, src => src.CurrencyType)
			.Map(dest => dest.Value, src => (double)src.Value);
	}
}