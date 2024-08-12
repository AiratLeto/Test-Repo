using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Requests.BrandRequests.PostBrand;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BrandRequests
{
	/// <summary>
	/// Тест <see cref="PostBrandCommandHandler"/>
	/// </summary>
	public class PostBrandCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public PostBrandCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper) => _dbContext = CreateInMemoryContext();

		/// <summary>
		/// Запрос на добавление cущности должен добавить сущность в БД
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_PostRequest_ShouldCreateEntityAsync()
		{
			var request = new PostBrandCommand()
			{
				Name = "959156652",
			};
			var handler = new PostBrandCommandHandler(_dbContext);
			var response = await handler.Handle(request, default);

			Assert.NotNull(response);

			var entity = await _dbContext.Brands
				.FirstOrDefaultAsync(x => x.Id == response.BrandId);

			Assert.NotNull(entity);

			Assert.Equal(entity.Name, request.Name);
		}
	}
}
