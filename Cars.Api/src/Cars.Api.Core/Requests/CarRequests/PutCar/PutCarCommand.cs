using System;
using Cars.Api.Contracts.Requests.CarRequests.PutCar;
using MediatR;

namespace Cars.Api.Core.Requests.CarRequests.PutCar
{
	/// <summary>
	/// Запрос на изменение сущности автомобиля
	/// </summary>
	public class PutCarCommand : PutCarRequest, IRequest
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Id сущности</param>
		public PutCarCommand(Guid id)
			=> Id = id;

		/// <summary>
		/// Id записи
		/// </summary>
		public Guid Id { get; }
	}
}
