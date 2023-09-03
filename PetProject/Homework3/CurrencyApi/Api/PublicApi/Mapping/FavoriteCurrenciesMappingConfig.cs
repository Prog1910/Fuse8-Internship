using Application.Shared.Dtos;
using Contracts;
using Domain.Aggregates;
using Mapster;

namespace PublicApi.Mapping;

public sealed class FavoriteCurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<FavoritesDto, FavoritesCache>();

		config.NewConfig<FavoritesRequest, FavoritesDto>();

		config.NewConfig<FavoritesCache, FavoritesResponse>();

		config.NewConfig<FavoritesDto, FavoritesResponse>()
			.Map(dest => dest.CurrencyCode, src => src.CurrencyCode.ToString())
			.Map(dest => dest.BaseCurrencyCode, src => src.BaseCurrencyCode.ToString());
	}
}