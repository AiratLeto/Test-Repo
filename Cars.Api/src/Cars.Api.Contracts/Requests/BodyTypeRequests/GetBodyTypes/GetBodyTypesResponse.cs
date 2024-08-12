using System.Collections.Generic;

namespace Cars.Api.Contracts.Requests.BodyTypeRequests.GetBodyTypes
{
	/// <summary>
	/// Ответ на запрос получения сущности "Тип кузова"
	/// </summary>
	public class GetBodyTypesResponse
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public GetBodyTypesResponse()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="entities">Список сущностей</param>
		/// <param name="totalCount">Общее количество сущностей</param>
		public GetBodyTypesResponse(List<GetBodyTypesResponseItem> entities, int totalCount)
		{
			Entities = entities;
			TotalCount = totalCount;
		}

		/// <summary>
		/// Список сущностей
		/// </summary>
		public List<GetBodyTypesResponseItem> Entities { get; set; } = default!;

		/// <summary>
		/// Общее количество сущностей
		/// </summary>
		public int TotalCount { get; set; }
	}
}