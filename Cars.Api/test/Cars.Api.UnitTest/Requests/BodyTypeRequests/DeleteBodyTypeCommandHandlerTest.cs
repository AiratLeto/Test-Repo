using System.Linq;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.BodyTypeRequests.DeleteBodyType;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BodyTypeRequests
{
	/// <summary>
	/// Тест <see cref="DeleteBodyTypeCommandHandler"/>
	/// </summary>
	public class DeleteBodyTypeCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly BodyType _entity;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public DeleteBodyTypeCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_entity = BodyType.CreateForTest(
				name: "479611819");

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_entity));
		}

		/// <summary>
		/// Запрос на изменение сущности должен удалить данные
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_DeleteRequest_ShouldDeleteEntityAsync()
		{
			var request = new DeleteBodyTypeCommand(_entity.Id);
			var handler = new DeleteBodyTypeCommandHandler(_dbContext);
			_ = await handler.Handle(request, default);

			Assert.Null(_dbContext.Brands.FirstOrDefault(x => x.Id == _entity.Id));
		}
	}
}
