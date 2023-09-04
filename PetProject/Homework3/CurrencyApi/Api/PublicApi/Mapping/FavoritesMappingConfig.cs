using Application.Shared.Dtos;
using Contracts;
using Domain.Aggregates;
using Domain.Enums;
using Mapster;

namespace PublicApi.Mapping;

public sealed class FavoritesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<FavoritesDto, FavoritesCache>()
			.Map(dest => dest.CurrencyCode, src => src.CurrencyCode.ToString())
			.Map(dest => dest.BaseCurrencyCode, src => src.BaseCurrencyCode.ToString());

		config.NewConfig<FavoritesRequest, FavoritesDto>();

		config.NewConfig<FavoritesCache, FavoritesDto>()
			.Map(dest => dest.CurrencyCode, src => Enum.Parse<CurrencyType>(src.CurrencyCode))
			.Map(dest => dest.BaseCurrencyCode, src => Enum.Parse<CurrencyType>(src.BaseCurrencyCode));

		config.NewConfig<FavoritesDto, FavoritesResponse>()
			.Map(dest => dest.CurrencyCode, src => src.CurrencyCode.ToString())
			.Map(dest => dest.BaseCurrencyCode, src => src.BaseCurrencyCode.ToString());
	}
}