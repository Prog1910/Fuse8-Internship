using Domain.Aggregates.CachedCurrenciesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class CurrenciesOnDateConfiguration : IEntityTypeConfiguration<CachedCurrencies>
{
	public void Configure(EntityTypeBuilder<CachedCurrencies> builder)
	{
		builder.HasKey(cod => new { cod.LastUpdatedAt, cod.BaseCurrency });

		builder.Property(cod => cod.LastUpdatedAt).IsRequired();

		builder.Property(cod => cod.BaseCurrency).IsRequired();

		builder.OwnsMany(cod => cod.Currencies, cb =>
		{
			cb.HasIndex(c => c.Code);
			cb.WithOwner().HasForeignKey(nameof(CachedCurrencies.LastUpdatedAt), nameof(CachedCurrencies.BaseCurrency));

			cb.Property(c => c.Code).IsRequired().HasColumnName("currency");

			cb.Property(c => c.Value).IsRequired().HasColumnName("exchange_rate");
		});
	}
}