using Contracts.Models.Currency;
using System.Text.Json.Serialization;

namespace Application.Services.Common;

/// <summary>
/// Historical currency information
/// </summary>
/// <param name="Date">Date of exchange rate relevance</param>
/// <param name="Code">Currency code</param>
/// <param name="Value">Currency rate</param>
public record CurrencyOnDate(
    [property: JsonPropertyName("date")] string Date,
    string Code,
    decimal Value) : CurrencyDataResponse(Code, Value);