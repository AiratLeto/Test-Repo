using System;
using System.Collections.Generic;

namespace Cars.Api.Core.Entities
{
	/// <summary>
	/// Тип кузова
	/// </summary>
	public class BodyType : DictionaryBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Наименование</param>
		public BodyType(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Конструктор EF
		/// </summary>
		protected BodyType()
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
		public static BodyType CreateForMigration(Guid id, string name, DateTime createdOn)
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
		public static BodyType CreateForTest(
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
