using Application.Common.Services.Rest.Common.Dtos;
using Contracts;
using Domain.Aggregates.CachedFavoriteCurrenciesAggregate;
using Mapster;

namespace PublicApi.Common.Mapping;

public sealed class FavoriteCurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<FavoriteCurrencyDto, CachedFavoriteCurrency>()
			.Map(dest => dest.Currency, src => src.Currency.ToString())
			.Map(dest => dest.BaseCurrency, src => src.BaseCurrency.ToString());

		config.NewConfig<FavoriteCurrencyRequest, FavoriteCurrencyDto>();

		config.NewConfig<CachedFavoriteCurrency, FavoriteCurrencyResponse>()
			.Map(dest => dest.Currency, src => src.Currency.ToString())
			.Map(dest => dest.BaseCurrency, src => src.BaseCurrency.ToString());
	}
}