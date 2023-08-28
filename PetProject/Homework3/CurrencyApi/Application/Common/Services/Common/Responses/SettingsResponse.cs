using System.Text.Json.Serialization;

namespace Application.Common.Services.Common.Responses;

public record SettingsResponse(
	[property: JsonPropertyName("account_id")] long AccountId,
	[property: JsonPropertyName("quotas")] QuotasResponse Quotas);

public record QuotasResponse(
	[property: JsonPropertyName("month")] MonthResponse Month,
	[property: JsonPropertyName("grace")] GraceResponse Grace);

public record MonthResponse(
	[property: JsonPropertyName("total")] int Total,
	[property: JsonPropertyName("used")] int Used,
	[property: JsonPropertyName("remaining")] int Remaining);

public record GraceResponse(
	[property: JsonPropertyName("total")] int Total,
	[property: JsonPropertyName("used")] int Used,
	[property: JsonPropertyName("remaining")] int Remaining);