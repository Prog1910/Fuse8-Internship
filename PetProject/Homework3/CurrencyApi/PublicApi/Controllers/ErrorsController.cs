using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

public sealed class ErrorsController : ControllerBase
{
	[HttpGet]
	[Route("/error")]
	public IActionResult Error()
	{
		return Problem();
	}
}