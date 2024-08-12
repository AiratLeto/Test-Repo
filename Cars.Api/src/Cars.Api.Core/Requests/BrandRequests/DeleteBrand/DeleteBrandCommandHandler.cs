using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.BrandRequests.DeleteBrand
{
	/// <summary>
	/// Обработчик запроса <see cref="DeleteBrandCommand"/>
	/// </summary>
	public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public DeleteBrandCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>-</returns>
		public async Task<Unit> Handle(
			DeleteBrandCommand request,
			CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var entity = await _dbContext.Brands
				.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Brand>(request.Id);

			var isCarExists = await _dbContext.Cars.AnyAsync(x => x.BrandId == entity.Id, cancellationToken);
			if (isCarExists)
				throw new ValidationException("Данный бренд используется в автомобиле");

			_dbContext.Brands.Remove(entity);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return default;
		}
	}
}
