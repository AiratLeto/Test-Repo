using Cars.Api.Contracts.Requests.BodyTypeRequests.PostBodyType;
using MediatR;

namespace Cars.Api.Core.Requests.BodyTypeRequests.PostBodyType
{
	/// <summary>
	/// Команда запроса <see cref="PostBodyTypeRequest"/>
	/// </summary>
	public class PostBodyTypeCommand : PostBodyTypeRequest, IRequest<PostBodyTypeResponse>
	{
	}
}
