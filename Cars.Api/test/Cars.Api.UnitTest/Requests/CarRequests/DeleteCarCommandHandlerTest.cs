using System.Linq;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.CarRequests.DeleteCar;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.CarRequests
{
	/// <summary>
	/// Тест <see cref="DeleteCarCommandHandler"/>
	/// </summary>
	public class DeleteCarCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly Car _entity;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public DeleteCarCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_entity = Car.CreateForTest(
				name: "520613145",
				seatsCount: 112,
				url: "1959720660");

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
			var request = new DeleteCarCommand(_entity.Id);
			var handler = new DeleteCarCommandHandler(_dbContext);
			_ = await handler.Handle(request, default);

			Assert.Equal(0, _dbContext.Cars.Count());
		}
	}
}
