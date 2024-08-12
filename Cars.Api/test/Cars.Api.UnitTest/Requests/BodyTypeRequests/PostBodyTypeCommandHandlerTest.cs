using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Requests.BodyTypeRequests.PostBodyType;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BodyTypeRequests
{
	/// <summary>
	/// Тест <see cref="PostBodyTypeCommandHandler"/>
	/// </summary>
	public class PostBodyTypeCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public PostBodyTypeCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper) => _dbContext = CreateInMemoryContext();

		/// <summary>
		/// Запрос на добавление cущности должен добавить сущность в БД
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_PostRequest_ShouldCreateEntityAsync()
		{
			var request = new PostBodyTypeCommand()
			{
				Name = "390872673",
			};
			var handler = new PostBodyTypeCommandHandler(_dbContext);
			var response = await handler.Handle(request, default);

			Assert.NotNull(response);

			var entity = await _dbContext.BodyTypes
				.FirstOrDefaultAsync(x => x.Id == response.BodyTypeId);

			Assert.NotNull(entity);

			Assert.Equal(entity.Name, request.Name);
		}
	}
}
