namespace Cars.Api.Contracts.Requests.BrandRequests.PostBrand
{
	/// <summary>
	/// Запрос на сохранение сущности "Бренд"
	/// </summary>
	public class PostBrandRequest
	{
		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; } = default!;
	}
}
