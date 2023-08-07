namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;

/// <summary>
/// Usage information for the grace period, if applicable
/// </summary>
/// <param name="Total">Total number of allowed requests for the current grace period</param>
/// <param name="Used">Number of requests used in the current grace period</param>
/// <param name="Remaining">Remaining number of requests for the current grace period</param>
public record Grace(
	int Total,
	int Used,
	int Remaining);