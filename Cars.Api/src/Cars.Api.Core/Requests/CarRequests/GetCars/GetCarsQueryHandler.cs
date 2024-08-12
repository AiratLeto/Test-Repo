using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.CarRequests.GetCars;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.CarRequests.GetCars
{
	/// <summary>
	/// Обработчик запроса <see cref="GetCarsQuery"/>
	/// </summary>
	public class GetCarsQueryHandler : IRequestHandler<GetCarsQuery, GetCarsResponse>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public GetCarsQueryHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<GetCarsResponse> Handle(GetCarsQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var query = _dbContext.Cars;

			var count = await query.CountAsync(cancellationToken);

			var result = await query
				.Select(x => new GetCarsResponseItem
				{
					Id = x.Id,
					Name = x.Name,
					SeatsCount = x.SeatsCount,
					Url = x.Url,
					BrandId = x.Brand!.Id,
					BrandName = x.Brand.Name,
					BodyTypeId = x.BodyType!.Id,
					BodyTypeName = x.BodyType.Name,
					CreatedOn = x.CreatedOn,
				})
				.OrderBy(request)
				.SkipTake(request)
				.ToListAsync(cancellationToken);

			return new GetCarsResponse(result, count);
		}
	}
}
