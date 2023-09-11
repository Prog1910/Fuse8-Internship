using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PublicApi.Infrastructure.Persistence.Configurations;

public sealed class SettingsConfiguration : IEntityTypeConfiguration<SettingsCache>
{
	public void Configure(EntityTypeBuilder<SettingsCache> builder)
	{
		builder.HasKey(s => s.Id);

		builder.Property(s => s.DefaultCurrencyCode).IsRequired();
		builder.Property(s => s.CurrencyRoundCount).IsRequired();
	}
}
