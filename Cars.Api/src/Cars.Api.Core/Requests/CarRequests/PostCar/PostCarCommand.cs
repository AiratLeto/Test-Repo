using Cars.Api.Contracts.Requests.CarRequests.PostCar;
using MediatR;

namespace Cars.Api.Core.Requests.CarRequests.PostCar
{
	/// <summary>
	/// Команда запроса <see cref="PostCarRequest"/>
	/// </summary>
	public class PostCarCommand : PostCarRequest, IRequest<PostCarResponse>
	{
	}
}
