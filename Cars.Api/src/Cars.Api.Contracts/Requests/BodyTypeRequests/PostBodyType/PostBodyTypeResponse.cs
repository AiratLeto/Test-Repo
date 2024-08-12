using System;

namespace Cars.Api.Contracts.Requests.BodyTypeRequests.PostBodyType
{
	/// <summary>
	/// Ответ на запрос <see cref="PostBodyTypeRequest"/>
	/// </summary>
	public class PostBodyTypeResponse
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Идентификатор созданного обращения</param>
		public PostBodyTypeResponse(Guid id)
			=> BodyTypeId = id;

		/// <summary>
		/// Идентификатор сущности "Тип кузова"
		/// </summary>
		public Guid BodyTypeId { get; }
	}
}