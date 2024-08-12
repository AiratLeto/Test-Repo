using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.BrandRequests.GetBrands;
using Cars.Api.Contracts.Requests.BrandRequests.PostBrand;
using Cars.Api.Contracts.Requests.BrandRequests.PutBrand;
using Cars.Api.Core.Requests.BrandRequests.DeleteBrand;
using Cars.Api.Core.Requests.BrandRequests.GetBrands;
using Cars.Api.Core.Requests.BrandRequests.PostBrand;
using Cars.Api.Core.Requests.BrandRequests.PutBrand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cars.Api.Web.Controllers
{
	/// <summary>
	/// Контроллер сущности "Бренд"
	/// </summary>
	public class BrandController : ApiControllerBase
	{
		/// <summary>
		/// Получить список сущностей по фильтру
		/// </summary>
		/// <param name="mediator">Медиатор CQRS</param>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Список сущностей</returns>
		[HttpGet]
		[SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetBrandsResponse))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetBrandsResponse> GetAsync(
			[FromServices] IMediator mediator,
			[FromQuery] GetBrandsRequest request,
			CancellationToken cancellationToken) =>
			await mediator.Send(
				request == null
					? new GetBrandsQuery()
					: new GetBrandsQuery
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
			[FromBody] PutBrandRequest request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			await mediator.Send(
				new PutBrandCommand(id)
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
		[SwaggerResponse(StatusCodes.Status200OK, type: typeof(PostBrandResponse))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<PostBrandResponse> CreateAsync(
			[FromServices] IMediator mediator,
			[FromBody] PostBrandRequest request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			return await mediator.Send(
				new PostBrandCommand()
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
			=> await mediator.Send(new DeleteBrandCommand(id), cancellationToken);
	}
}
