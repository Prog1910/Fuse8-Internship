using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Currency;

public record CurrencyMetaDto(
    [property: JsonPropertyName("last_updated_at")] DateTime LastUpdatedAt
   );
