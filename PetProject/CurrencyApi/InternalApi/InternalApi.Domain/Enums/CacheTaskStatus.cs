using System.Text.Json.Serialization;

namespace InternalApi.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CacheTaskStatus
{
	Created,
	InProgress,
	CompletedSuccessfully,
	CompletedWithError,
	Cancelled
}
