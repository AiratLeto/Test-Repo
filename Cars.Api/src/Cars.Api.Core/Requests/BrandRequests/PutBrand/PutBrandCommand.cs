using System;
using Cars.Api.Contracts.Requests.BrandRequests.PutBrand;
using MediatR;

namespace Cars.Api.Core.Requests.BrandRequests.PutBrand
{
	/// <summary>
	/// Запрос на изменение сущности "Бренд"
	/// </summary>
	public class PutBrandCommand : PutBrandRequest, IRequest
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="id">Id сущности</param>
		public PutBrandCommand(Guid id)
			=> Id = id;

		/// <summary>
		/// Id записи
		/// </summary>
		public Guid Id { get; }
	}
}
