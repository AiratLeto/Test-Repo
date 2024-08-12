using Cars.Api.Contracts.Requests.BrandRequests.GetBrands;
using Cars.Api.Core.Models;
using MediatR;

namespace Cars.Api.Core.Requests.BrandRequests.GetBrands
{
	/// <summary>
	/// Запрос получения списка "Бренд"
	/// </summary>
	public class GetBrandsQuery
		: GetBrandsRequest, IRequest<GetBrandsResponse>, IOrderByQuery, IPaginationQuery
	{
	}
}
