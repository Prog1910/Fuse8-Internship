using InternalApi.Domain.Aggregates;
using InternalApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalApi.Domain.Persistence.Configurations;

public sealed class CacheTaskConfiguration : IEntityTypeConfiguration<CacheTask>
{
	public void Configure(EntityTypeBuilder<CacheTask> builder)
	{
		builder.HasKey(t => t.Id);

		builder.Property(t => t.Id).HasDefaultValueSql("uuid_generate_v4()");

		builder.Property(t => t.BaseCurrencyCode).IsRequired();

		builder.Property(t => t.Status).IsRequired().HasConversion<string>().HasDefaultValue(CacheTaskStatus.Created);
	}
}
