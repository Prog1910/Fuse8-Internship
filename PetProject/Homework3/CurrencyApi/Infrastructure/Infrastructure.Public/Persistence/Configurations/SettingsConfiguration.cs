using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Public.Persistence.Configurations;

public sealed class SettingsConfiguration : IEntityTypeConfiguration<CachedSettings>
{
	public void Configure(EntityTypeBuilder<CachedSettings> builder)
	{
		builder.HasKey(s => s.Id);

		builder.Property(s => s.DefaultCurrency).IsRequired();
		builder.Property(s => s.CurrencyRoundCount).IsRequired();
	}
}