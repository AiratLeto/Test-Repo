using Cars.Api.Contracts.Requests.CarRequests.GetCars;
using Cars.Api.Core.Models;
using MediatR;

namespace Cars.Api.Core.Requests.CarRequests.GetCars
{
	/// <summary>
	/// Запрос получения списка автомобилей
	/// </summary>
	public class GetCarsQuery
		: GetCarsRequest, IRequest<GetCarsResponse>, IOrderByQuery, IPaginationQuery
	{
	}
}
