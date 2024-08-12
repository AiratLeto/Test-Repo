using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.BrandRequests.PostBrand;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using MediatR;

namespace Cars.Api.Core.Requests.BrandRequests.PostBrand
{
	/// <summary>
	/// Обработчик запроса <see cref="PostBrandCommand"/>
	/// </summary>
	public class PostBrandCommandHandler : IRequestHandler<PostBrandCommand, PostBrandResponse>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public PostBrandCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<PostBrandResponse> Handle(
			PostBrandCommand request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var entity = new Brand(name: request.Name);

			await _dbContext.Brands.AddAsync(entity, cancellationToken);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return new PostBrandResponse(entity.Id);
		}
	}
}
