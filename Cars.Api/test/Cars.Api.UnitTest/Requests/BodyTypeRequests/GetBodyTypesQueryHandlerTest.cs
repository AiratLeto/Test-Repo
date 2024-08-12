using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.BodyTypeRequests.GetBodyTypes;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BodyTypeRequests
{
	/// <summary>
	/// Тест <see cref="GetBodyTypesQueryHandler"/>
	/// </summary>
	public class GetBodyTypesQueryHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly BodyType _entity;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public GetBodyTypesQueryHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_entity = BodyType.CreateForTest(
				name: "2049622011");

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_entity));
		}

		/// <summary>
		/// Метод получения списка сущностей "Тип кузова" с фильтром должен вернуть отфильтрованные записи
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_QueryWithFilters_ShouldReturnFilteredEntitiesAsync()
		{
			var request = new GetBodyTypesQuery();
			var handler = new GetBodyTypesQueryHandler(_dbContext);
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
