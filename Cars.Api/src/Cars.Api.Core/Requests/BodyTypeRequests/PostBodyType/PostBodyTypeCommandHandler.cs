using System;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.BodyTypeRequests.PostBodyType;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using MediatR;

namespace Cars.Api.Core.Requests.BodyTypeRequests.PostBodyType
{
	/// <summary>
	/// Обработчик запроса <see cref="PostBodyTypeCommand"/>
	/// </summary>
	public class PostBodyTypeCommandHandler : IRequestHandler<PostBodyTypeCommand, PostBodyTypeResponse>
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dbContext">Контекст БД</param>
		public PostBodyTypeCommandHandler(IDbContext dbContext)
			=> _dbContext = dbContext;

		/// <summary>
		/// Обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены операции</param>
		/// <returns>Ответ на запрос</returns>
		public async Task<PostBodyTypeResponse> Handle(
			PostBodyTypeCommand request,
			CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var entity = new BodyType(name: request.Name);

			await _dbContext.BodyTypes.AddAsync(entity, cancellationToken);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return new PostBodyTypeResponse(entity.Id);
		}
	}
}
