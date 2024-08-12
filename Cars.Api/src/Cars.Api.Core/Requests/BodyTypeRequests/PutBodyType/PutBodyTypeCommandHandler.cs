using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Requests.BodyTypeRequests.PutBodyType
{
	/// <summary>
	/// Обработчик запроса <see cref="PutBodyTypeCommand"/>
	/// </summary>
	public class PutBodyTypeCommandHandler : IRequestHandler<PutBodyTypeCommand>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public PutBodyTypeCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<Unit> Handle(
			PutBodyTypeCommand request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var entity = await _dbContext.BodyTypes
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<BodyType>(request.Id);

			entity.Name = request.Name;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return default;
		}
	}
}
