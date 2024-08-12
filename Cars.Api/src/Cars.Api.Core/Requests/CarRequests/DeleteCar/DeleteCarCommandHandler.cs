using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.CarRequests.DeleteCar
{
	/// <summary>
	/// Обработчик запроса <see cref="DeleteCarCommand"/>
	/// </summary>
	public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public DeleteCarCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>-</returns>
		public async Task<Unit> Handle(
			DeleteCarCommand request,
			CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var entity = await _dbContext.Cars
				.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Car>(request.Id);

			_dbContext.Cars.Remove(entity);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return default;
		}
	}
}
