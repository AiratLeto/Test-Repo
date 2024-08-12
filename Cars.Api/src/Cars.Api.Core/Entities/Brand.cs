using System;
using System.Collections.Generic;

namespace Cars.Api.Core.Entities
{
	/// <summary>
	/// Бренд
	/// </summary>
	public class Brand : DictionaryBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Наименование</param>
		public Brand(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Конструктор EF
		/// </summary>
		protected Brand()
		{
		}

		#region Navigation properties

		/// <summary>
		/// Автомобили
		/// </summary>
		public List<Car>? Cars { get; set; }

		#endregion

		/// <summary>
		/// Создать сущность для миграции
		/// </summary>
		/// <param name="id">Идентификатор</param>
		/// <param name="name">Наименование</param>
		/// <param name="createdOn">Дата создания</param>
		/// <returns>Заполненная сущность для первичной миграции</returns>
		public static Brand CreateForMigration(Guid id, string name, DateTime createdOn)
			=> new()
			{
				Id = id,
				Name = name,
				CreatedOn = createdOn,
			};

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Идентификатор сущности</param>
		/// <param name="name">Наименование</param>
		/// <param name="cars">Автомобили</param>
		/// <returns>Заполненный объект класса</returns>
		[Obsolete("Только для тестов")]
		public static Brand CreateForTest(
			Guid id = default,
			string name = "someName",
			List<Car>? cars = default)
			=> new()
			{
				Id = id,
				Name = name,
				Cars = cars,
			};
	}
}
