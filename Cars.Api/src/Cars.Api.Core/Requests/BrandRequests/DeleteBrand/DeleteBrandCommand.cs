using System;
using MediatR;

namespace Cars.Api.Core.Requests.BrandRequests.DeleteBrand
{
	/// <summary>
	/// Запрос удаления сущности "Бренд"
	/// </summary>
	public class DeleteBrandCommand : IRequest
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Id сущности</param>
		public DeleteBrandCommand(Guid id)
			=> Id = id;

		/// <summary>
		/// Id записи
		/// </summary>
		public Guid Id { get; }
	}
}
