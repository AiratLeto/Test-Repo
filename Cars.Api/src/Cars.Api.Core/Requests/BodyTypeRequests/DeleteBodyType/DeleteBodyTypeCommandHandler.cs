using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.BodyTypeRequests.DeleteBodyType
{
	/// <summary>
	/// Обработчик запроса <see cref="DeleteBodyTypeCommand"/>
	/// </summary>
	public class DeleteBodyTypeCommandHandler : IRequestHandler<DeleteBodyTypeCommand>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public DeleteBodyTypeCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>-</returns>
		public async Task<Unit> Handle(
			DeleteBodyTypeCommand request,
			CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var entity = await _dbContext.BodyTypes
				.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<BodyType>(request.Id);

			var isCarExists = await _dbContext.Cars.AnyAsync(x => x.BodyTypeId == entity.Id, cancellationToken);
			if (isCarExists)
				throw new ValidationException("Данный тип кузова используется в автомобиле");

			_dbContext.BodyTypes.Remove(entity);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return default;
		}
	}
}
