namespace Cars.Api.Contracts.Requests.BrandRequests.PutBrand
{
	/// <summary>
	/// Команда на изменение списка "Бренд"
	/// </summary>
	public class PutBrandRequest
	{
		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; } = default!;
	}
}
