using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.CarRequests.PostCar;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.CarRequests.PostCar
{
	/// <summary>
	/// Обработчик запроса <see cref="PostCarCommand"/>
	/// </summary>
	public class PostCarCommandHandler : IRequestHandler<PostCarCommand, PostCarResponse>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public PostCarCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<PostCarResponse> Handle(
			PostCarCommand request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var brand = await _dbContext.Brands
				.FirstOrDefaultAsync(x => x.Id == request.BrandId, cancellationToken: cancellationToken)
				?? throw new EntityNotFoundException<Brand>(request.BrandId);

			var bodyType = await _dbContext.BodyTypes
				.FirstOrDefaultAsync(x => x.Id == request.BodyTypeId, cancellationToken: cancellationToken)
				?? throw new EntityNotFoundException<BodyType>(request.BodyTypeId);

			var entity = new Car(
				name: request.Name,
				seatsCount: request.SeatsCount,
				url: request.Url,
				brand: brand,
				bodyType: bodyType);

			await _dbContext.Cars.AddAsync(entity, cancellationToken);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return new PostCarResponse(entity.Id);
		}
	}
}
