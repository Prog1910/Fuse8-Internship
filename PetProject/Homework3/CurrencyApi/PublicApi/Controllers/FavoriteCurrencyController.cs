using Application.Common.Interfaces.Rest;
using Application.Common.Services.Rest.Common.Dtos;
using Contracts;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

/// <summary>
///     Controller for managinf favorite currencies.
/// </summary>
[ApiController]
[Route("currency-api/favorite-currency")]
public sealed class FavoriteCurrencyController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IFavoriteCurrencyService _publicService;

	public FavoriteCurrencyController(IFavoriteCurrencyService publicService, IMapper mapper)
	{
		_publicService = publicService;
		_mapper = mapper;
	}

	/// <summary>
	///     Retrieves the list of all favorite currencies
	/// </summary>
	/// <response code="200">The list of all favorite currencies were successfully obtained.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet]
	[ProducesDefaultResponseType(typeof(List<FavoriteCurrencyResponse>))]
	public async Task<IActionResult> GetAllFavoriteCurrencies()
	{
		var favoriteCurrencyDtos = await _publicService.GetAllFavoriteCurrenciesAsync();
		var favoriteCurrenciesResponse = favoriteCurrencyDtos is not null
			? _mapper.Map<List<FavoriteCurrencyResponse>>(favoriteCurrencyDtos) : null;

		return Ok(favoriteCurrenciesResponse);
	}

	/// <summary>
	///     Retrieves the favorite currency
	/// </summary>
	/// <param name="name">The name of favorite currency you want to get.</param>
	/// <response code="200">The favorite currency was successfully obtained.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("by-name/{name}")]
	[ProducesDefaultResponseType(typeof(FavoriteCurrencyResponse))]
	public async Task<IActionResult> GetFavoriteCurrencyByName(string name)
	{
		var favoriteCurrencyDto = await _publicService.GetFavoriteCurrencyByNameAsync(name);
		var favoriteCurrencyResponse = favoriteCurrencyDto is not null
			? _mapper.Map<FavoriteCurrencyResponse>(favoriteCurrencyDto) : null;

		return Ok(favoriteCurrencyResponse);
	}

	/// <summary>
	///     Adds the favorite currency
	/// </summary>
	/// <param name="favoriteCurrencyRequest">The favorite currency you want to add.</param>
	/// <response code="200">The favorite currency was successfully added.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPost]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> AddFavoriteCurrencies([FromQuery] FavoriteCurrencyRequest favoriteCurrencyRequest)
	{
		var favoriteCurrencyDto = _mapper.Map<FavoriteCurrencyDto>(favoriteCurrencyRequest);
		await _publicService.AddFavoriteCurrencyAsync(favoriteCurrencyDto);

		return Accepted();
	}

	/// <summary>
	///     Updates the favorite currency by name
	/// </summary>
	/// <param name="favoriteCurrencyRequest">The favorite currency you want to update.</param>
	/// <response code="200">The favorite currency was successfully updated.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("by-name/{name}")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateFavoriteCurrenciesByName([FromQuery] FavoriteCurrencyRequest favoriteCurrencyRequest)
	{
		var favoriteCurrencyDto = _mapper.Map<FavoriteCurrencyDto>(favoriteCurrencyRequest);
		await _publicService.UpdateFavoriteCurrencyByNameAsync(favoriteCurrencyDto);

		return Accepted();
	}

	/// <summary>
	///     Deletes the favorite currency by name
	/// </summary>
	/// <param name="name">The name of favorite currency you want to delete.</param>
	/// <response code="200">The favorite currency was successfully deleted.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpDelete("by-name/{name}")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> DeleteFavoriteCurrenciesByName(string name)
	{
		await _publicService.DeleteFavoriteCurrencyByNameAsync(name);

		return Ok();
	}
}