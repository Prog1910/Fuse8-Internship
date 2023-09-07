﻿using Application.Shared.Dtos;
using Mapster;
using Protos;

namespace InternalApi.Mapping;

public sealed class SettingsMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<SettingsDto, SettingsResponse>()
			.Map(dest => dest.BaseCurrencyCode, src => (CurrencyType)(int)src.BaseCurrencyCode);
	}
}
