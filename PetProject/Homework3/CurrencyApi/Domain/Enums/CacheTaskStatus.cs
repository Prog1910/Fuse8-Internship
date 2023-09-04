using System.Text.Json.Serialization;

namespace Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CacheTaskStatus
{
	Created,
	InProgress,
	CompletedSuccessfully,
	CompletedWithError,
	Cancelled
}