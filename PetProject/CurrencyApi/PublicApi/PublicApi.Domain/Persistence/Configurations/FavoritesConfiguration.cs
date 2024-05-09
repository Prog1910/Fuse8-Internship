using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublicApi.Domain.Aggregates;

namespace PublicApi.Domain.Persistence.Configurations;

public sealed class FavoritesConfiguration : IEntityTypeConfiguration<FavoritesCache>
{
	public void Configure(EntityTypeBuilder<FavoritesCache> builder)
	{
		builder.HasKey(f => f.Id);

		builder.HasIndex(f => f.Name).IsUnique();
		builder.HasIndex(f => new { f.CurrencyCode, f.BaseCurrencyCode }).IsUnique();

		builder.Property(f => f.Name).IsRequired();
		builder.Property(f => f.CurrencyCode).IsRequired();
		builder.Property(f => f.BaseCurrencyCode).IsRequired();
	}
}
