using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

/// <summary>
/// Usage information for the grace period, if applicable
/// </summary>
/// <param name="Total">Total number of allowed requests for the current grace period</param>
/// <param name="Used">Number of requests used in the current grace period</param>
/// <param name="Remaining">Remaining number of requests for the current grace period</param>
public record GraceDto(
	[property: JsonPropertyName("total")] int Total,
	[property: JsonPropertyName("used")] int Used,
	[property: JsonPropertyName("remaining")] int Remaining
	);
