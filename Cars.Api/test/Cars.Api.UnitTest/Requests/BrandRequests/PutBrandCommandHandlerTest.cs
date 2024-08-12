using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.BrandRequests.PutBrand;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.BrandRequests
{
	/// <summary>
	/// Тест <see cref="PutBrandCommandHandler"/>
	/// </summary>
	public class PutBrandCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly Brand _entity;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public PutBrandCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_entity = Brand.CreateForTest(
				name: "1482203829");

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
			var request = new PutBrandCommand(_entity.Id)
			{
				Name = "269485282",
			};
			var handler = new PutBrandCommandHandler(_dbContext);
			var response = await handler.Handle(request, default);

			var entity = await _dbContext.Brands
				.FirstOrDefaultAsync(x => x.Id == _entity.Id);

			Assert.NotNull(entity);

			Assert.Equal(request.Id, entity.Id);
			Assert.Equal(request.Name, entity.Name);
		}
	}
}
