using Domain.Aggregates.CachedSettingsAggregate;
using Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class SettingsConfiguration : IEntityTypeConfiguration<CachedSettings>
{
	private readonly PublicApiOptions _options;

	public SettingsConfiguration(PublicApiOptions options)
	{
		_options = options;
	}

	public void Configure(EntityTypeBuilder<CachedSettings> builder)
	{
		builder.HasKey(s => s.Id);

		builder.HasData(new CachedSettings
		{
			Id = 1,
			DefaultCurrency = _options.DefaultCurrency,
			CurrencyRoundCount = _options.CurrencyRoundCount
		});

		builder.Property(s => s.DefaultCurrency).IsRequired().HasMaxLength(8).HasColumnName("default_currency");
		builder.Property(s => s.CurrencyRoundCount).IsRequired();
	}
}
