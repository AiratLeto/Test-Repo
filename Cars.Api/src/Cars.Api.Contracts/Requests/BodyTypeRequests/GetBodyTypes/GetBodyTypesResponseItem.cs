using System;

namespace Cars.Api.Contracts.Requests.BodyTypeRequests.GetBodyTypes
{
	/// <summary>
	/// Тип кузова для <see cref="GetBodyTypesResponse"/>
	/// </summary>
	public class GetBodyTypesResponseItem
	{
		/// <summary>
		/// Идентификатор сущности
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; } = default!;

		/// <summary>
		/// Дата создания записи
		/// </summary>
		public DateTime CreatedOn { get; set; }
	}
}
