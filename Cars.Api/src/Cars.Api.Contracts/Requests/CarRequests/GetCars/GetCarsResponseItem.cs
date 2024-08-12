using System;

namespace Cars.Api.Contracts.Requests.CarRequests.GetCars
{
	/// <summary>
	/// Бибика для <see cref="GetCarsResponse"/>
	/// </summary>
	public class GetCarsResponseItem
	{
		/// <summary>
		/// Идентификатор сущности
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название модели
		/// </summary>
		public string Name { get; set; } = default!;

		/// <summary>
		/// Число сидений в салоне
		/// </summary>
		public int SeatsCount { get; set; }

		/// <summary>
		/// URL сайта оф. дилера
		/// </summary>
		public string? Url { get; set; }

		/// <summary>
		/// Идентификатор бренда
		/// </summary>
		public Guid BrandId { get; set; }

		/// <summary>
		/// Наименование бренда
		/// </summary>
		public string BrandName { get; set; } = default!;

		/// <summary>
		/// Идентификатор типа кузова
		/// </summary>
		public Guid BodyTypeId { get; set; }

		/// <summary>
		/// Наименование типа кузова
		/// </summary>
		public string BodyTypeName { get; set; } = default!;

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime CreatedOn { get; set; }
	}
}
