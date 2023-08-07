namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;

/// <summary>
/// API status response
/// </summary>
/// <param name="AccountId">Your unique account identifier</param>
/// <param name="Quotas">Contains information about your request quota</param>
public record StatusResponse(
	long AccountId,
	Quotas Quotas);