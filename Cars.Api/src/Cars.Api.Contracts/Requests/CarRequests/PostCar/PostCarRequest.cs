using System;

namespace Cars.Api.Contracts.Requests.CarRequests.PostCar
{
	/// <summary>
	/// Запрос на сохранение сущности автомобиля
	/// </summary>
	public class PostCarRequest
	{
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
		/// Бренд
		/// </summary>
		public Guid BrandId { get; set; }

		/// <summary>
		/// Тип кузова
		/// </summary>
		public Guid BodyTypeId { get; set; }
	}
}
