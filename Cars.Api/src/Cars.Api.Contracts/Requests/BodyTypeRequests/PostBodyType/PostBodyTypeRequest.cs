namespace Cars.Api.Contracts.Requests.BodyTypeRequests.PostBodyType
{
	/// <summary>
	/// Запрос на сохранение сущности "Тип кузова"
	/// </summary>
	public class PostBodyTypeRequest
	{
		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; } = default!;
	}
}
