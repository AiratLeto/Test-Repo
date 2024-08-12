using System.Collections.Generic;

namespace Cars.Api.Contracts.Requests.BrandRequests.GetBrands
{
	/// <summary>
	/// Ответ на запрос получения сущности "Бренд"
	/// </summary>
	public class GetBrandsResponse
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public GetBrandsResponse()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="entities">Список сущностей</param>
		/// <param name="totalCount">Общее количество сущностей</param>
		public GetBrandsResponse(List<GetBrandsResponseItem> entities, int totalCount)
		{
			Entities = entities;
			TotalCount = totalCount;
		}

		/// <summary>
		/// Список сущностей
		/// </summary>
		public List<GetBrandsResponseItem> Entities { get; set; } = default!;

		/// <summary>
		/// Общее количество сущностей
		/// </summary>
		public int TotalCount { get; set; }
	}
}