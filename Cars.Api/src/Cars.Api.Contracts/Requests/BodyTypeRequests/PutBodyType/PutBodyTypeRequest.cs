namespace Cars.Api.Contracts.Requests.BodyTypeRequests.PutBodyType
{
	/// <summary>
	/// Команда на изменение списка "Тип кузова"
	/// </summary>
	public class PutBodyTypeRequest
	{
		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; } = default!;
	}
}
