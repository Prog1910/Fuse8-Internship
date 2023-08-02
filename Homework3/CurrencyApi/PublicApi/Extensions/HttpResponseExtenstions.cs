using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using System.Net;
using System.Text.Json;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Extensions;

public static class HttpResponseMessageExtenstions
{
	public async static Task<TDto> EnsureValidAndDeserialize<TDto>(this HttpResponseMessage responseMessage) where TDto : IResponse
	{
		EnsureValid(responseMessage);
		var response = await responseMessage.Deserialize<TDto>();
		return response ?? throw new CurrencyNotFoundException();
	}

	public static void EnsureValid(this HttpResponseMessage responseMessage)
	{
		if (responseMessage.IsSuccessStatusCode == false)
			throw responseMessage.StatusCode switch
			{
				HttpStatusCode.TooManyRequests => new ApiRequestLimitException(),
				HttpStatusCode.UnprocessableEntity => new CurrencyNotFoundException(),
				_ => new HttpRequestException(message: "Failed to get default currency data", inner: null, statusCode: HttpStatusCode.InternalServerError),
			};
	}

	public static async Task<TDto?> Deserialize<TDto>(this HttpResponseMessage responseMessage)
		=> JsonSerializer.Deserialize<TDto>(await responseMessage.Content.ReadAsStringAsync());
}
