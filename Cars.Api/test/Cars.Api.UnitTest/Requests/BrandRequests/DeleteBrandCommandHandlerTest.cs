using System.Linq;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.BrandRequests.DeleteBrand;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BrandRequests
{
	/// <summary>
	/// Тест <see cref="DeleteBrandCommandHandler"/>
	/// </summary>
	public class DeleteBrandCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly Brand _entity;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public DeleteBrandCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_entity = Brand.CreateForTest(
				name: "1070373220");

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_entity));
		}

		/// <summary>
		/// Запрос на удаление сущности должен удалить данные
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_DeleteRequest_ShouldDeleteEntityAsync()
		{
			var request = new DeleteBrandCommand(_entity.Id);
			var handler = new DeleteBrandCommandHandler(_dbContext);
			_ = await handler.Handle(request, default);

			Assert.Null(_dbContext.Brands.FirstOrDefault(x => x.Id == _entity.Id));
		}
	}
}
