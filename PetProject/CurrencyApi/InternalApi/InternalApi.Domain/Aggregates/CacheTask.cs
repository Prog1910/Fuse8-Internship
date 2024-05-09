using System.ComponentModel.DataAnnotations;
using InternalApi.Domain.Enums;

namespace InternalApi.Domain.Aggregates;

public sealed record CacheTask
{
	private CacheTask(Guid id, string baseCurrencyCode, CacheTaskStatus status)
	{
		Id = id;
		BaseCurrencyCode = baseCurrencyCode;
		Status = status;
	}

	public Guid Id { get; init; }
	[StringLength(5, MinimumLength = 3)] public string BaseCurrencyCode { get; set; }
	public CacheTaskStatus Status { get; set; }

	public static CacheTask Create(string baseCurrencyCode)
	{
		return new CacheTask(Guid.NewGuid(), baseCurrencyCode, CacheTaskStatus.Created);
	}
}
