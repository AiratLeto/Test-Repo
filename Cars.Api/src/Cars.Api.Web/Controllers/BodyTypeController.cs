using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.BodyTypeRequests.GetBodyTypes;
using Cars.Api.Contracts.Requests.BodyTypeRequests.PostBodyType;
using Cars.Api.Contracts.Requests.BodyTypeRequests.PutBodyType;
using Cars.Api.Core.Requests.BodyTypeRequests.DeleteBodyType;
using Cars.Api.Core.Requests.BodyTypeRequests.GetBodyTypes;
using Cars.Api.Core.Requests.BodyTypeRequests.PostBodyType;
using Cars.Api.Core.Requests.BodyTypeRequests.PutBodyType;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cars.Api.Web.Controllers
{
	/// <summary>
	/// Контроллер сущности "Тип кузова"
	/// </summary>
	public class BodyTypeController : ApiControllerBase
	{
		/// <summary>
		/// Получить список сущностей по фильтру
		/// </summary>
		/// <param name="mediator">Медиатор CQRS</param>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Список сущностей</returns>
		[HttpGet]
		[SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetBodyTypesResponse))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetBodyTypesResponse> GetAsync(
			[FromServices] IMediator mediator,
			[FromQuery] GetBodyTypesRequest request,
			CancellationToken cancellationToken) =>
			await mediator.Send(
				request == null
					? new GetBodyTypesQuery()
					: new GetBodyTypesQuery
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
			[FromBody] PutBodyTypeRequest request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			await mediator.Send(
				new PutBodyTypeCommand(id)
				{
					Name = request.Name,
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
		[SwaggerResponse(StatusCodes.Status200OK, type: typeof(PostBodyTypeResponse))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<PostBodyTypeResponse> CreateAsync(
			[FromServices] IMediator mediator,
			[FromBody] PostBodyTypeRequest request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			return await mediator.Send(
				new PostBodyTypeCommand()
				{
					Name = request.Name,
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
			=> await mediator.Send(new DeleteBodyTypeCommand(id), cancellationToken);
	}
}
