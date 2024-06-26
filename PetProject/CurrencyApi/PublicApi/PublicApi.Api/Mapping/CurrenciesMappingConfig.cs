﻿using Mapster;
using Protos;
using Shared.Application.Dtos;
using CurrencyType = Shared.Domain.Enums.CurrencyType;

namespace PublicApi.Api.Mapping;

public sealed class CurrenciesMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<CurrencyResponse, CurrencyDto>()
			.Map(dest => dest.Code, src => Enum.Parse<CurrencyType>(src.CurrencyCode));

		config.NewConfig<CurrencyDto, Shared.Contracts.CurrencyResponse>()
			.Map(dest => dest.Code, src => src.Code.ToString());
	}
}
