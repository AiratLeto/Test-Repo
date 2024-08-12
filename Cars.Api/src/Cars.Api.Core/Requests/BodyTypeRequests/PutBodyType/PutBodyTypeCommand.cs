using System;
using Cars.Api.Contracts.Requests.BodyTypeRequests.PutBodyType;
using MediatR;

namespace Cars.Api.Core.Requests.BodyTypeRequests.PutBodyType
{
	/// <summary>
	/// Запрос на изменение сущности "Тип кузова"
	/// </summary>
	public class PutBodyTypeCommand : PutBodyTypeRequest, IRequest
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Id сущности</param>
		public PutBodyTypeCommand(Guid id)
			=> Id = id;

		/// <summary>
		/// Id записи
		/// </summary>
		public Guid Id { get; }
	}
}
