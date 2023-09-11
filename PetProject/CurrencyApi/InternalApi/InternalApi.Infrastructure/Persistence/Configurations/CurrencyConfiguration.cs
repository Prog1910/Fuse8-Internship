using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalApi.Infrastructure.Persistence.Configurations;

public sealed class CurrencyConfiguration : IEntityTypeConfiguration<CurrenciesOnDateCache>
{
	public void Configure(EntityTypeBuilder<CurrenciesOnDateCache> builder)
	{
		builder.HasKey(cod => new
		{
			cod.LastUpdatedAt,
			cod.BaseCurrencyCode
		});

		builder.Property(cod => cod.LastUpdatedAt).IsRequired();

		builder.Property(cod => cod.BaseCurrencyCode).IsRequired();

		builder.OwnsMany(cod => cod.Currencies, cb =>
		{
			cb.HasIndex(c => c.Code);
			cb.WithOwner().HasForeignKey(nameof(CurrenciesOnDateCache.LastUpdatedAt), nameof(CurrenciesOnDateCache.BaseCurrencyCode));

			cb.Property(c => c.Code).IsRequired().HasColumnName("currency_code");

			cb.Property(c => c.Value).IsRequired().HasColumnName("exchange_rate");
		});
	}
}
