using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.BrandRequests.GetBrands;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.BrandRequests.GetBrands
{
	/// <summary>
	/// Обработчик запроса <see cref="GetBrandsQuery"/>
	/// </summary>
	public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, GetBrandsResponse>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public GetBrandsQueryHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<GetBrandsResponse> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var query = _dbContext.Brands;

			var count = await query.CountAsync(cancellationToken);

			var result = await query
				.Select(x => new GetBrandsResponseItem
				{
					Id = x.Id,
					Name = x.Name,
					CreatedOn = x.CreatedOn,
				})
				.OrderBy(request)
				.SkipTake(request)
				.ToListAsync(cancellationToken);

			return new GetBrandsResponse(result, count);
		}
	}
}
