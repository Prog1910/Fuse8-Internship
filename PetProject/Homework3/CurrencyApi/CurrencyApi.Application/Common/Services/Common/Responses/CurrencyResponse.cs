using System.Text.Json.Serialization;

namespace CurrencyApi.Application.Common.Services.Common.Responses;

public record CurrencyResponse(
	[property: JsonPropertyName("meta")] CurrencyMetaResponse Meta,
	[property: JsonPropertyName("data")] Dictionary<string, CurrencyDataResponse> Data);

public record CurrencyMetaResponse(
	[property: JsonPropertyName("last_updated_at")] string LastUpdatedAt);

public record CurrencyDataResponse(
	[property: JsonPropertyName("code")] string Code,
	[property: JsonPropertyName("value")] decimal Value);