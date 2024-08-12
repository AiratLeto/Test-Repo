using Cars.Api.Contracts.Requests.BodyTypeRequests.GetBodyTypes;
using Cars.Api.Core.Models;
using MediatR;

namespace Cars.Api.Core.Requests.BodyTypeRequests.GetBodyTypes
{
	/// <summary>
	/// Запрос получения списка "Тип кузова"
	/// </summary>
	public class GetBodyTypesQuery
		: GetBodyTypesRequest, IRequest<GetBodyTypesResponse>, IOrderByQuery, IPaginationQuery
	{
	}
}
