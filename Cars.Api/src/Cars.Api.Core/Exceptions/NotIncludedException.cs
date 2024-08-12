using System;

namespace Cars.Api.Core.Exceptions
{
	/// <summary>
	/// Исключение для забытого инклюда
	/// </summary>
	public class NotIncludedException : ArgumentNullException
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="message">Наименование поля</param>
		public NotIncludedException(string message)
			: base($"Не загружено поле {message}")
		{
		}
	}
}
