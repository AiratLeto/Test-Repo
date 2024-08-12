using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.CarRequests.GetCars;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.CarRequests
{
	/// <summary>
	/// Тест <see cref="GetCarsQueryHandler"/>
	/// </summary>
	public class GetCarsQueryHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly Car _entity;
		private readonly Brand _brand;
		private readonly BodyType _bodyType;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public GetCarsQueryHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_brand = Brand.CreateForTest();
			_bodyType = BodyType.CreateForTest();
			_entity = Car.CreateForTest(
				name: "1337081861",
				seatsCount: 4,
				url: "1364706895",
				brand: _brand,
				bodyType: _bodyType);

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_entity));
		}

		/// <summary>
		/// Метод получения списка сущностей "Бибика" с фильтром должен вернуть отфильтрованные записи
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_QueryWithFilters_ShouldReturnFilteredEntitiesAsync()
		{
			var request = new GetCarsQuery();
			var handler = new GetCarsQueryHandler(_dbContext);
			var response = await handler.Handle(request, default);

			Assert.NotNull(response?.Entities);
			Assert.Equal(1, response.TotalCount);
			var foundEntity = Assert.Single(response.Entities);
			Assert.NotNull(foundEntity);

			Assert.Equal(_entity.Id, foundEntity.Id);
			Assert.Equal(_entity.Name, foundEntity.Name);
			Assert.Equal(_entity.SeatsCount, foundEntity.SeatsCount);
			Assert.Equal(_entity.Url, foundEntity.Url);
			Assert.Equal(_entity.Brand!.Id, foundEntity.BrandId);
			Assert.Equal(_entity.BodyType!.Id, foundEntity.BodyTypeId);
		}
	}
}
