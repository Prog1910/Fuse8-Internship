using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

/// <summary>
/// Useful information
/// </summary>
/// <param name="LastUpdatedAt">Datetime to let you know then this dataset was last updated</param>
public record CurrencyMetaDto(
	[property: JsonPropertyName("last_updated_at")] string LastUpdatedAt
   );
