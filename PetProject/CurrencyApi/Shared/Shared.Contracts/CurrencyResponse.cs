namespace Shared.Contracts;

/// <summary> Represents a response containing currency code and corresponding value. </summary>
/// <param name="Code"> The currency code. </param>
/// <param name="Value"> The currency value. </param>
public sealed record CurrencyResponse(string Code, decimal Value);
