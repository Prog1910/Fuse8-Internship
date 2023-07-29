namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;

public class ApiRequestLimitException : Exception
{
	public ApiRequestLimitException() : base("You have reached your request limit") { }

	public ApiRequestLimitException(string? message) : base(message) { }
}
