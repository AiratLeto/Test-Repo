using System;

namespace Cars.Api.Contracts.Requests.BrandRequests.PostBrand
{
	/// <summary>
	/// Ответ на запрос <see cref="PostBrandRequest"/>
	/// </summary>
	public class PostBrandResponse
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Идентификатор созданного обращения</param>
		public PostBrandResponse(Guid id)
			=> BrandId = id;

		/// <summary>
		/// Идентификатор сущности "Бренд"
		/// </summary>
		public Guid BrandId { get; }
	}
}