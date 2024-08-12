using System;

namespace Cars.Api.Contracts.Requests.BrandRequests.GetBrands
{
	/// <summary>
	/// Бренд для <see cref="GetBrandsResponse"/>
	/// </summary>
	public class GetBrandsResponseItem
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
