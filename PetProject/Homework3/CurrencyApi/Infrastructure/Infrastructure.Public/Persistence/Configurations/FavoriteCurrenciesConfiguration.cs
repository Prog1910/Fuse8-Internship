using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Public.Persistence.Configurations;

public sealed class FavoriteCurrenciesConfiguration : IEntityTypeConfiguration<CachedFavoriteCurrency>
{
	public void Configure(EntityTypeBuilder<CachedFavoriteCurrency> builder)
	{
		builder.HasKey(fc => fc.Name);
		builder.HasIndex(fc => fc.Name).IsUnique();
		builder.HasIndex(fc => new
		{
			fc.Currency,
			fc.BaseCurrency
		}).IsUnique();

		builder.Property(fc => fc.Name).IsRequired();
		builder.Property(fc => fc.Currency).IsRequired();
		builder.Property(fc => fc.BaseCurrency).IsRequired();
	}
}