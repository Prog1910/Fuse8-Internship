using Domain.Enums;

namespace Application.Internal.Interfaces.Rest;

public interface IRestApi
{
	Task<Guid> RecalculateCurrencyCacheAsync(CurrencyType baseCurrency);
}