using Cars.Api.Contracts.Requests.BrandRequests.PostBrand;
using MediatR;

namespace Cars.Api.Core.Requests.BrandRequests.PostBrand
{
	/// <summary>
	/// Команда запроса <see cref="PostBrandRequest"/>
	/// </summary>
	public class PostBrandCommand : PostBrandRequest, IRequest<PostBrandResponse>
	{
	}
}
