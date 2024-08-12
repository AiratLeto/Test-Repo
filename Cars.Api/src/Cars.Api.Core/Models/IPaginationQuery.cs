namespace Cars.Api.Core.Models
{
	/// <summary>
	/// Запрос данных с пагинацией
	/// </summary>
	public interface IPaginationQuery
	{
		/// <summary>
		/// Номер страницы, начиная с 1
		/// </summary>
		int PageNumber { get; set; }

		/// <summary>
		/// Размер страницы
		/// </summary>
		int PageSize { get; set; }
	}
}
