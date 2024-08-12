using System.Collections.Generic;

namespace Cars.Api.Contracts.Requests.CarRequests.GetCars
{
	/// <summary>
	/// Ответ на запрос получения сущности автомобилей
	/// </summary>
	public class GetCarsResponse
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public GetCarsResponse()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="entities">Список сущностей</param>
		/// <param name="totalCount">Общее количество сущностей</param>
		public GetCarsResponse(List<GetCarsResponseItem> entities, int totalCount)
		{
			Entities = entities;
			TotalCount = totalCount;
		}

		/// <summary>
		/// Список сущностей
		/// </summary>
		public List<GetCarsResponseItem> Entities { get; set; } = default!;

		/// <summary>
		/// Общее количество сущностей
		/// </summary>
		public int TotalCount { get; set; }
	}
}
