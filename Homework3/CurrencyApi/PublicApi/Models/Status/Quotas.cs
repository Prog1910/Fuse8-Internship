using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

/// <summary>
/// Contains information about your API usage limits
/// </summary>
/// <param name="Month">Usage information for the current month</param>
/// <param name="Grace">Usage information for the grace period</param>
public record Quotas(
	[property: JsonPropertyName("month")] Month Month,
	[property: JsonPropertyName("grace")] Grace Grace);
