using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Status;

/// <summary>
/// API status response
/// </summary>
/// <param name="AccountId">Your unique account identifier</param>
/// <param name="Quotas">Contains information about your request quota</param>
public record QuotaInfoDto(
	[property: JsonPropertyName("account_id")] long AccountId,
	[property: JsonPropertyName("quotas")] QuotasDto Quotas
	) : IInfo;