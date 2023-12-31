﻿namespace Fuse8_ByteMinds.SummerSchool.Domain.Models.Status;

/// <summary>
/// Usage information for the current month
/// </summary>
/// <param name="Total">Total number of allowed requests for the current month</param>
/// <param name="Used">Number of requests used in the current month</param>
/// <param name="Remaining">Remaining number of requests for the current month</param>
public record Month(
	int Total,
	int Used,
	int Remaining);