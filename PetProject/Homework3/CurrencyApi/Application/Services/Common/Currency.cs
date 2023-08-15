using System.Text.Json.Serialization;

namespace Application.Services.Common;

/// <summary>
/// Actual currency information
/// </summary>
/// <param name="Code">Currency code</param>
/// <param name="Value">Currency rate</param>
public record Currency(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("value")] decimal Value);