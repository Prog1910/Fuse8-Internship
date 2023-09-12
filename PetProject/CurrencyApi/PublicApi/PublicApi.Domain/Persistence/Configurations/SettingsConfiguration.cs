using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublicApi.Domain.Aggregates;

namespace PublicApi.Domain.Persistence.Configurations;

public sealed class SettingsConfiguration : IEntityTypeConfiguration<SettingsCache>
{
	public void Configure(EntityTypeBuilder<SettingsCache> builder)
	{
		builder.HasKey(s => s.Id);

		builder.Property(s => s.DefaultCurrencyCode).IsRequired();
		builder.Property(s => s.CurrencyRoundCount).IsRequired();
	}
}
