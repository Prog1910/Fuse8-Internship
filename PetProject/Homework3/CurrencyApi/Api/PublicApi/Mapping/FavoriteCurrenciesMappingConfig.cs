using Application.Shared.Dtos;
using Domain.Aggregates;
using Mapster;

namespace PublicApi.Mapping;

public sealed class FavoriteCurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<FavoriteCurrencyDto, CachedFavoriteCurrency>()
			.Map(dest => dest.Currency, src => src.Currency.ToString())
			.Map(dest => dest.BaseCurrency, src => src.BaseCurrency.ToString());

		config.NewConfig<Contracts.FavoriteCurrencyRequest, FavoriteCurrencyDto>();

		config.NewConfig<CachedFavoriteCurrency, Contracts.FavoriteCurrencyResponse>()
			.Map(dest => dest.Currency, src => src.Currency.ToString())
			.Map(dest => dest.BaseCurrency, src => src.BaseCurrency.ToString());
	}
}