using Domain.Aggregates.CurrenciesOnDateAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class CurrencyConfiguration : IEntityTypeConfiguration<CachedCurrencies>
{
	public void Configure(EntityTypeBuilder<CachedCurrencies> builder)
	{
		builder.HasKey(ccod => new { ccod.LastUpdatedAt, ccod.BaseCurrency });

		builder.Property(ccod => ccod.LastUpdatedAt).IsRequired();
		builder.Property(ccod => ccod.BaseCurrency).IsRequired().HasMaxLength(8).HasColumnName("from_currency");

		builder.OwnsMany(ccod => ccod.Currencies, cb =>
		{
			cb.HasIndex(c => c.Code);
			cb.WithOwner().HasForeignKey(nameof(CachedCurrencies.LastUpdatedAt), nameof(CachedCurrencies.BaseCurrency));

			cb.Property(c => c.Code).IsRequired().HasMaxLength(8).HasColumnName("to_currency");
			cb.Property(c => c.Value).IsRequired().HasColumnName("exchange_rate");
		});
	}
}