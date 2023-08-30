using Domain.Aggregates.CachedSettingsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Configurations;

public sealed class SettingsConfiguration : IEntityTypeConfiguration<CachedSettings>
{
	public void Configure(EntityTypeBuilder<CachedSettings> builder)
	{
		builder.HasKey(s => s.Id);

		builder.Property(s => s.DefaultCurrency).IsRequired();
		builder.Property(s => s.CurrencyRoundCount).IsRequired();
	}
}