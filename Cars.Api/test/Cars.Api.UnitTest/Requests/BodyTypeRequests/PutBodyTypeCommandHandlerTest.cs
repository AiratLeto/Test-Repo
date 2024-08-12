using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.BodyTypeRequests.PutBodyType;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BodyTypeRequests
{
	/// <summary>
	/// Тест <see cref="PutBodyTypeCommandHandler"/>
	/// </summary>
	public class PutBodyTypeCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly BodyType _entity;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public PutBodyTypeCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_entity = BodyType.CreateForTest(
				name: "734975170");

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_entity));
		}

		/// <summary>
		/// Запрос на изменение сущности должен обновить данные
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_PutRequest_ShouldChangeEntityAsync()
		{
			var request = new PutBodyTypeCommand(_entity.Id)
			{
				Name = "208935758",
			};
			var handler = new PutBodyTypeCommandHandler(_dbContext);
			var response = await handler.Handle(request, default);

			var entity = await _dbContext.BodyTypes
				.FirstOrDefaultAsync(x => x.Id == _entity.Id);

			Assert.NotNull(entity);

			Assert.Equal(request.Id, entity.Id);
			Assert.Equal(request.Name, entity.Name);
		}
	}
}
