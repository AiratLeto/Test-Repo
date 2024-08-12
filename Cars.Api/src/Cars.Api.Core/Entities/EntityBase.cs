using System;

namespace Cars.Api.Core.Entities
{
	/// <summary>
	/// Базовая сущность
	/// </summary>
	public abstract class EntityBase
	{
		/// <summary>
		/// Идентификатор сущности
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Дата создания сущности
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Является ли сущность новой
		/// </summary>
		public bool IsNew => Id == default;
	}
}
