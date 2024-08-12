using System;
using Cars.Api.Core.Exceptions;

namespace Cars.Api.Core.Entities
{
	/// <summary>
	/// Базовый справочник
	/// </summary>
	public class DictionaryBase : EntityBase
	{
		private string _name = default!;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Наименование</param>
		public DictionaryBase(string name) => _name = name;

		/// <summary>
		/// Конструктор
		/// </summary>
		protected DictionaryBase()
		{
		}

		/// <summary>
		/// Наименование
		/// </summary>
		public string Name
		{
			get => _name;
			set => _name = string.IsNullOrWhiteSpace(value)
				? throw new RequiredFieldNotSpecifiedException("Наименование")
				: value;
		}

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Идентификатор сущности</param>
		/// <param name="name">Наименование</param>
		/// <returns>Заполненный объект класса</returns>
		[Obsolete("Только для тестов")]
		public static DictionaryBase CreateForTest(
			Guid id = default,
			string name = "someName")
			=> new()
			{
				Id = id,
				_name = name,
			};
	}
}
