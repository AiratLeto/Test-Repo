using System;
using MediatR;

namespace Cars.Api.Core.Requests.BodyTypeRequests.DeleteBodyType
{
	/// <summary>
	/// Запрос удаления сущности "Тип кузова"
	/// </summary>
	public class DeleteBodyTypeCommand : IRequest
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Id сущности</param>
		public DeleteBodyTypeCommand(Guid id)
			=> Id = id;

		/// <summary>
		/// Id записи
		/// </summary>
		public Guid Id { get; }
	}
}
