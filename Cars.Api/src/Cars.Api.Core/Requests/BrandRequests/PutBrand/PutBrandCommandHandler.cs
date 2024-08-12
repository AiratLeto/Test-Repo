using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.BrandRequests.PutBrand
{
	/// <summary>
	/// Обработчик запроса <see cref="PutBrandCommand"/>
	/// </summary>
	public class PutBrandCommandHandler : IRequestHandler<PutBrandCommand>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public PutBrandCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<Unit> Handle(
			PutBrandCommand request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var entity = await _dbContext.Brands
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Brand>(request.Id);

			entity.Name = request.Name;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return default;
		}
	}
}
