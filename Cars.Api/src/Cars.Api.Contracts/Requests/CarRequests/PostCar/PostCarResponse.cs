using System;

namespace Cars.Api.Contracts.Requests.CarRequests.PostCar
{
	/// <summary>
	/// Ответ на запрос <see cref="PostCarRequest"/>
	/// </summary>
	public class PostCarResponse
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Идентификатор созданного объекта</param>
		public PostCarResponse(Guid id)
			=> Id = id;

		/// <summary>
		/// Идентификатор сущности "Бибика"
		/// </summary>
		public Guid Id { get; }
	}
}
