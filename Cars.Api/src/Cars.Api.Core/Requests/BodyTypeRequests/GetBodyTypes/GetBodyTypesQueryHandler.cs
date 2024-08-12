using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.BodyTypeRequests.GetBodyTypes;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.BodyTypeRequests.GetBodyTypes
{
	/// <summary>
	/// Обработчик запроса <see cref="GetBodyTypesQuery"/>
	/// </summary>
	public class GetBodyTypesQueryHandler : IRequestHandler<GetBodyTypesQuery, GetBodyTypesResponse>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public GetBodyTypesQueryHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<GetBodyTypesResponse> Handle(GetBodyTypesQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var query = _dbContext.BodyTypes;

			var count = await query.CountAsync(cancellationToken);

			var result = await query
				.Select(x => new GetBodyTypesResponseItem
				{
					Id = x.Id,
					Name = x.Name,
					CreatedOn = x.CreatedOn,
				})
				.OrderBy(request)
				.SkipTake(request)
				.ToListAsync(cancellationToken);

			return new GetBodyTypesResponse(result, count);
		}
	}
}
