using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;

/// <summary>
/// Usage information for the current month
/// </summary>
/// <param name="Total">Total number of allowed requests for the current month</param>
/// <param name="Used">Number of requests used in the current month</param>
/// <param name="Remaining">Remaining number of requests for the current month</param>
public record Month(
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("used")] int Used,
    [property: JsonPropertyName("remaining")] int Remaining);
