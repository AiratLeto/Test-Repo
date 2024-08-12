namespace Cars.Api.Data.MsSql
{
	/// <summary>
	/// Конфиг проекта
	/// </summary>
	public class MsSqlDbOptions
	{
		/// <summary>
		/// Строка подключения к БД
		/// </summary>
		public string ConnectionString { get; set; } = default!;
	}
}
