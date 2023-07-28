using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

public record ApiStatusDto(
	[property: JsonPropertyName("account_id")] long AccountId,
	[property: JsonPropertyName("quotas")] QuotasDto Quotas
	);