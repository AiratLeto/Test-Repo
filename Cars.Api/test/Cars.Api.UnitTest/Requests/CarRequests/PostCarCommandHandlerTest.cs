using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.CarRequests.PostCar;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.CarRequests
{
	/// <summary>
	/// Тест <see cref="PostCarCommandHandler"/>
	/// </summary>
	public class PostCarCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly Brand _brand;
		private readonly BodyType _bodyType;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public PostCarCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_brand = Brand.CreateForTest();
			_bodyType = BodyType.CreateForTest();

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(
					_brand,
					_bodyType
				));
		}

		/// <summary>
		/// Запрос на добавление cущности должен добавить сущность в БД
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_PostRequest_ShouldCreateEntityAsync()
		{
			var request = new PostCarCommand()
			{
				Name = "1190543872",
				SeatsCount = 12,
				Url = "2035216125.ru",
				BrandId = _brand.Id,
				BodyTypeId = _bodyType.Id
			};
			var handler = new PostCarCommandHandler(_dbContext);
			var response = await handler.Handle(request, default);

			Assert.NotNull(response);

			var entity = await _dbContext.Cars
				.FirstOrDefaultAsync(x => x.Id == response.Id);

			Assert.NotNull(entity);

			Assert.Equal(entity.Name, request.Name);
			Assert.Equal(entity.SeatsCount, request.SeatsCount);
			Assert.Equal(entity.Url, request.Url);
			Assert.Equal(entity.Brand!.Id, request.BrandId);
			Assert.Equal(entity.BodyType!.Id, request.BodyTypeId);
		}
	}
}
