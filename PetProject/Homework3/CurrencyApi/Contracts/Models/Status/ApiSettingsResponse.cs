using System.Text.Json.Serialization;

namespace Contracts.Models.Status;

/// <summary>
/// API status response
/// </summary>
/// <param name="AccountId">Your unique account identifier</param>
/// <param name="Quotas">Contains information about your request quota</param>
public record ApiSettingsResponse(
	[property: JsonPropertyName("account_id")] long AccountId,
	[property: JsonPropertyName("quotas")] QuotasResponse Quotas);

/// <summary>
/// Contains information about your API usage limits
/// </summary>
/// <param name="Month">Usage information for the current month</param>
/// <param name="Grace">Usage information for the grace period</param>
public record QuotasResponse(
	[property: JsonPropertyName("month")] MonthResponse Month,
	[property: JsonPropertyName("grace")] GraceResponse Grace);

/// <summary>
/// Usage information for the current month
/// </summary>
/// <param name="Total">Total number of allowed requests for the current month</param>
/// <param name="Used">Number of requests used in the current month</param>
/// <param name="Remaining">Remaining number of requests for the current month</param>
public record MonthResponse(
	[property: JsonPropertyName("total")] int Total,
	[property: JsonPropertyName("used")] int Used,
	[property: JsonPropertyName("remaining")] int Remaining);

/// <summary>
/// Usage information for the grace period, if applicable
/// </summary>
/// <param name="Total">Total number of allowed requests for the current grace period</param>
/// <param name="Used">Number of requests used in the current grace period</param>
/// <param name="Remaining">Remaining number of requests for the current grace period</param>
public record GraceResponse(
	[property: JsonPropertyName("total")] int Total,
	[property: JsonPropertyName("used")] int Used,
	[property: JsonPropertyName("remaining")] int Remaining);