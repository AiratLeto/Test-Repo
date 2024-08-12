using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Core.Requests.CarRequests.PutCar;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest.Requests.CarRequests
{
	/// <summary>
	/// Тест <see cref="PutCarCommandHandler"/>
	/// </summary>
	public class PutCarCommandHandlerTest : UnitTestBase
	{
		private readonly IDbContext _dbContext;

		private readonly Car _entity;
		private readonly Brand _brand;
		private readonly BodyType _bodyType;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Логгер</param>
		public PutCarCommandHandlerTest(ITestOutputHelper testOutputHelper)
			: base(testOutputHelper)
		{
			_brand = Brand.CreateForTest();
			_bodyType = BodyType.CreateForTest();
			_entity = Car.CreateForTest(
				name: "1804085038",
				seatsCount: 127,
				url: "584133992",
				brand: _brand,
				bodyType: _bodyType);

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(
					_entity,
					_brand,
					_bodyType
				));
		}

		/// <summary>
		/// Запрос на изменение сущности должен обновить данные
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task Handle_PutRequest_ShouldChangeEntityAsync()
		{
			var request = new PutCarCommand(_entity.Id)
			{
				Name = "1486778466",
				SeatsCount = 5,
				Url = "1606423650.ru",
				BrandId = _brand.Id,
				BodyTypeId = _bodyType.Id
			};
			var handler = new PutCarCommandHandler(_dbContext);
			var response = await handler.Handle(request, default);

			var entity = await _dbContext.Cars
				.FirstOrDefaultAsync(x => x.Id == _entity.Id);

			Assert.NotNull(entity);

			Assert.Equal(request.Id, entity.Id);
			Assert.Equal(request.Name, entity.Name);
			Assert.Equal(request.SeatsCount, entity.SeatsCount);
			Assert.Equal(request.Url, entity.Url);
			Assert.Equal(request.BrandId, entity.BrandId);
			Assert.Equal(request.BodyTypeId, entity.BodyTypeId);
		}
	}
}
