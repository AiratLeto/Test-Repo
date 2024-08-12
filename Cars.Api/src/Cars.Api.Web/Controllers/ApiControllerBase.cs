using Cars.Api.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cars.Api.Web.Controllers
{
	/// <summary>
	/// Базовый API-контроллер
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ProblemDetailsResponse))]
	public class ApiControllerBase : ControllerBase
	{
	}
}
