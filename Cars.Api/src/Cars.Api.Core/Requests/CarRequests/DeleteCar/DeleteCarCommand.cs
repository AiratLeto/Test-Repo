using System;
using MediatR;

namespace Cars.Api.Core.Requests.CarRequests.DeleteCar
{
	/// <summary>
	/// Запрос удаления сущности "Бибика"
	/// </summary>
	public class DeleteCarCommand : IRequest
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Id сущности</param>
		public DeleteCarCommand(Guid id)
			=> Id = id;

		/// <summary>
		/// Id записи
		/// </summary>
		public Guid Id { get; }
	}
}
