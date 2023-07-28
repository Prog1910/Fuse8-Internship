using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

public record GraceDto(
	[property: JsonPropertyName("total")] int Total,
	[property: JsonPropertyName("used")] int Used,
	[property: JsonPropertyName("remaining")] int Remaining
	);
