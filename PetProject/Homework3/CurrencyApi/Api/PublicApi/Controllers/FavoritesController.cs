using Application.Public.Interfaces.Rest;
using Application.Shared.Dtos;
using Contracts;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

/// <summary>
///     Controller for managing favorite currencies.
/// </summary>
[ApiController]
[Route("currency-api/favorites")]
public sealed class FavoritesController : ControllerBase
{
	private readonly IFavoritesService _favoritesService;

	public FavoritesController(IFavoritesService favoritesService)
	{
		_favoritesService = favoritesService;
	}

	/// <summary>
	///     Retrieves a list of all favorite currencies
	/// </summary>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <response code="200">The list of all favorites were successfully obtained.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet]
	[ProducesDefaultResponseType(typeof(List<FavoritesResponse>))]
	public async Task<IActionResult> GetAllFavoriteCurrencies(CancellationToken cancellationToken)
	{
		var favoritesDtos = await _favoritesService.GetAllFavoritesAsync(cancellationToken);

		return Ok(favoritesDtos?.Adapt<List<FavoritesResponse>>());
	}

	/// <summary>
	///     Retrieves the favorite currencies from cache
	/// </summary>
	/// <param name="name">The name of favorites you want to get.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <response code="200">The favorites was successfully obtained.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpGet("{name}")]
	[ProducesDefaultResponseType(typeof(FavoritesResponse))]
	public async Task<IActionResult> GetFavoritesByName([FromRoute] string name, CancellationToken cancellationToken)
	{
		var favoriteCurrencyDto = await _favoritesService.GetFavoritesByNameAsync(name, cancellationToken);

		return Ok(favoriteCurrencyDto?.Adapt<FavoritesResponse>());
	}

	/// <summary>
	///     Adds the favorite currencies to cache
	/// </summary>
	/// <param name="favoritesRequest">The favorites you want to add.</param>
	///  <param name="cancellationToken">Cancellation token.</param>
	/// <response code="200">The favorites was successfully added.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPost]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> AddFavorites([FromQuery] FavoritesRequest favoritesRequest, CancellationToken cancellationToken)
	{
		await _favoritesService.AddFavoritesAsync(favoritesRequest.Adapt<FavoritesDto>(), cancellationToken);

		return Accepted();
	}

	/// <summary>
	///     Updates the favorite currencies in cache by name
	/// </summary>
	/// <param name="favoritesRequest">The new favorites you want to update.</param>
	/// <param name="name">The name of favorites you want to update.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <response code="200">The favorites was successfully updated.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpPut("{name}")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> UpdateFavoritesByName([FromQuery] FavoritesRequest favoritesRequest, [FromRoute] string name, CancellationToken cancellationToken)
	{
		await _favoritesService.UpdateFavoritesByNameAsync(favoritesRequest.Adapt<FavoritesDto>(), name, cancellationToken);

		return NoContent();
	}

	/// <summary>
	///     Deletes the favorite currencies from cache by name
	/// </summary>
	/// <param name="name">The name of favorites you want to delete.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <response code="200">The favorites was successfully deleted.</response>
	/// <response code="404">The requested endpoint could not be found.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	[HttpDelete("{name}")]
	[ProducesDefaultResponseType(typeof(void))]
	public async Task<IActionResult> DeleteFavoritesByName([FromRoute] string name, CancellationToken cancellationToken)
	{
		await _favoritesService.DeleteFavoritesByNameAsync(name, cancellationToken);

		return NoContent();
	}
}