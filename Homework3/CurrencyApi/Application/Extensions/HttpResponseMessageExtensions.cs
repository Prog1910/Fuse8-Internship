using Fuse8_ByteMinds.SummerSchool.Domain.Exceptions;
using System.Net;
using System.Net.Http.Json;

namespace Fuse8_ByteMinds.SummerSchool.Application.Extensions;

public static class HttpResponseMessageExtenstions
{
    public async static Task<T> EnsureValidAndDeserialize<T>(this HttpResponseMessage responseMessage)
    {
        EnsureValid(responseMessage);
        var response = await responseMessage.Deserialize<T>();
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

    public static async Task<T?> Deserialize<T>(this HttpResponseMessage responseMessage)
        => await responseMessage.Content.ReadFromJsonAsync<T>();
}
