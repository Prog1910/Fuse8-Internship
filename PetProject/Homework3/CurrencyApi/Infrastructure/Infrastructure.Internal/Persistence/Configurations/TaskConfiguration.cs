using Domain.Aggregates;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Internal.Persistence.Configurations;

public sealed class TaskConfiguration : IEntityTypeConfiguration<CacheTask>
{
	public void Configure(EntityTypeBuilder<CacheTask> builder)
	{
		builder.HasKey(t => t.Id);

		builder.Property(t => t.BaseCurrencyCode).IsRequired();

		builder.Property(t => t.Status).IsRequired().HasConversion<string>().HasDefaultValue(CacheTaskStatus.Created);
	}
}