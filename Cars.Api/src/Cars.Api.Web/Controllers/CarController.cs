using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.CarRequests.GetCars;
using Cars.Api.Contracts.Requests.CarRequests.PostCar;
using Cars.Api.Contracts.Requests.CarRequests.PutCar;
using Cars.Api.Core.Requests.CarRequests.DeleteCar;
using Cars.Api.Core.Requests.CarRequests.GetCars;
using Cars.Api.Core.Requests.CarRequests.PostCar;
using Cars.Api.Core.Requests.CarRequests.PutCar;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cars.Api.Web.Controllers
{
	/// <summary>
	/// Контроллер сущности "Автомобиль"
	/// </summary>
	public class CarController : ApiControllerBase
	{
		/// <summary>
		/// Получить список сущностей по фильтру
		/// </summary>
		/// <param name="mediator">Медиатор CQRS</param>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Список сущностей</returns>
		[HttpGet]
		[SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetCarsResponse))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetCarsResponse> GetAsync(
			[FromServices] IMediator mediator,
			[FromQuery] GetCarsRequest request,
			CancellationToken cancellationToken) =>
			await mediator.Send(
				request == null
					? new GetCarsQuery()
					: new GetCarsQuery
					{
						PageNumber = request.PageNumber,
						PageSize = request.PageSize,
						OrderBy = request.OrderBy,
						IsAscending = request.IsAscending,
					},
				cancellationToken);

		/// <summary>
		/// Обновление записи
		/// </summary>
		/// <param name="mediator">Медиатор CQRS</param>
		/// <param name="id">Идентификатор</param>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		[HttpPut("{id}")]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task PutAsync(
			[FromServices] IMediator mediator,
			[FromRoute] Guid id,
			[FromBody] PutCarRequest request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			await mediator.Send(
				new PutCarCommand(id)
				{
					Name = request.Name,
					SeatsCount = request.SeatsCount,
					Url = request.Url,
					BrandId = request.BrandId,
					BodyTypeId = request.BodyTypeId,
				},
				cancellationToken);
		}

		/// <summary>
		/// Создание новой записи
		/// </summary>
		/// <param name="mediator">Медиатор CQRS</param>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Идентификатор созданной записи</returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK, type: typeof(PostCarResponse))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<PostCarResponse> CreateAsync(
			[FromServices] IMediator mediator,
			[FromBody] PostCarRequest request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			return await mediator.Send(
				new PostCarCommand()
				{
					Name = request.Name,
					SeatsCount = request.SeatsCount,
					Url = request.Url,
					BrandId = request.BrandId,
					BodyTypeId = request.BodyTypeId,
				},
				cancellationToken);
		}

		/// <summary>
		/// Удалить сущность по идентификатору
		/// </summary>
		/// <param name="mediator">Медиатор CQRS</param>
		/// <param name="id">Идентификатор</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Сущность</returns>
		[HttpDelete("{id}")]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task DeleteAsync(
			[FromServices] IMediator mediator,
			[FromRoute] Guid id,
			CancellationToken cancellationToken)
			=> await mediator.Send(new DeleteCarCommand(id), cancellationToken);
	}
}
