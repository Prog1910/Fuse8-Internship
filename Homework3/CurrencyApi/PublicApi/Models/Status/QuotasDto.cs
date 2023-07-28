using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

public record QuotasDto(
	[property: JsonPropertyName("month")] MonthDto Month,
	[property: JsonPropertyName("grace")] GraceDto Grace
	);
