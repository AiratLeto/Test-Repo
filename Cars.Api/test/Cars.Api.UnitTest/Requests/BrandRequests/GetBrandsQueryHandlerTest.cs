using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.BrandRequests.GetBrands;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BrandRequests
{
	/// <summary>
	/// Тест <see cref="GetBrandsQueryHandler"/>
	/// </summary>
	public class GetBrandsQueryHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly Brand _entity;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public GetBrandsQueryHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_entity = Brand.CreateForTest(
				name: "522571627");

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_entity));
		}

		/// <summary>
		/// Метод получения списка сущностей "Бренд" с фильтром должен вернуть отфильтрованные записи
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_QueryWithFilters_ShouldReturnFilteredEntitiesAsync()
		{
			var request = new GetBrandsQuery();
			var handler = new GetBrandsQueryHandler(_dbContext);
			var response = await handler.Handle(request, default);

			Assert.NotNull(response?.Entities);
			Assert.Equal(1, response.TotalCount);
			var foundEntity = Assert.Single(response.Entities);
			Assert.NotNull(foundEntity);

			Assert.Equal(_entity.Id, foundEntity.Id);
			Assert.Equal(_entity.Name, foundEntity.Name);
			Assert.Equal(_entity.CreatedOn, foundEntity.CreatedOn);
		}
	}
}
