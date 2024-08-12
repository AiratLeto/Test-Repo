using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.CarRequests.PutCar
{
	/// <summary>
	/// Обработчик запроса <see cref="PutCarCommand"/>
	/// </summary>
	public class PutCarCommandHandler : IRequestHandler<PutCarCommand>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public PutCarCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<Unit> Handle(
			PutCarCommand request,
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

			var entity = await _dbContext.Cars
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Car>(request.Id);

			entity.Name = request.Name;
			entity.SeatsCount = request.SeatsCount;
			entity.Url = request.Url;
			entity.Brand = brand;
			entity.BodyType = bodyType;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return default;
		}
	}
}
