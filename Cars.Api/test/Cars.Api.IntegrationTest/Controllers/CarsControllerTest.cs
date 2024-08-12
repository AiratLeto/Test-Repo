using System;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.CarRequests.GetCars;
using Cars.Api.Contracts.Requests.CarRequests.PostCar;
using Cars.Api.Contracts.Requests.CarRequests.PutCar;
using Cars.Api.Core.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Cars.Api.IntegrationTest.Controllers
{
	/// <summary>
	/// Тестовые методы для контроллера машинок
	/// </summary>
	[Collection(nameof(TestCollectionFixture))]
	public class CarsControllerTest : CustomWebApplicationFactory
	{
		private readonly Car _car;
		private readonly Car _anotherCar;
		private readonly Brand _brand;
		private readonly Brand _anotherBrand;
		private readonly BodyType _bodyType;
		private readonly BodyType _anotherBodyType;
		private readonly MicroserviceClient _client;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Лог для тестов</param>
		/// <param name="testInitializer">Инициализация тестовых контейнеров</param>
		public CarsControllerTest(
			ITestOutputHelper testOutputHelper,
			TestInitializerFixture testInitializer)
			: base(testOutputHelper, testInitializer)
		{
			_brand = Brand.CreateForTest(name: "someBrand");
			_anotherBrand = Brand.CreateForTest(name: "anotherBrand");
			_bodyType = BodyType.CreateForTest(name: "someType");
			_anotherBodyType = BodyType.CreateForTest(name: "anotherType");
			_car = Car.CreateForTest(
				name: "someName2",
				seatsCount: 1,
				brand: _brand,
				bodyType: _bodyType,
				url: "url9.ru");
			_anotherCar = Car.CreateForTest(
				name: "someName1",
				seatsCount: 2,
				brand: _brand,
				bodyType: _bodyType,
				url: "url.ru");

			_client = CreateApiClient(
				dbSeeder: x => x.AddRange(
					_brand,
					_anotherBrand,
					_bodyType,
					_anotherBodyType,
					_anotherCar,
					_car));
		}

		/// <summary>
		/// Метод получения списка машинок должен возвращать машинки
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task GetCarsAsync_DefaultRequest_ShouldReturnCarsAsync()
		{
			var request = new GetCarsRequest()
			{
				IsAscending = true,
				OrderBy = nameof(GetCarsResponseItem.Name),
				PageNumber = 1,
				PageSize = 1,
			};
			var response = await _client.GetCarsAsync(request);

			Assert.NotNull(response.Entities);
			var entity = Assert.Single(response.Entities);
			Assert.NotNull(entity);
			Assert.Equal(2, response.TotalCount);
		}

		/// <summary>
		/// Метод создания машины должен создать сущность
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task PostCarAsync_DefaultRequest_ShouldCreateCarAsync()
		{
			var request = new PostCarRequest()
			{
				BrandId = _brand.Id,
				BodyTypeId = _bodyType.Id,
				Name = "длинное наименование",
				Url = "NewUrl.ru",
				SeatsCount = 10,
			};
			var response = await _client.PostCarAsync(request);

			Assert.NotNull(response);
			Assert.NotEqual(Guid.Empty, response.Id);

			var entity = await GetEntityAsync<Car>(x => x.Name == request.Name);

			Assert.NotNull(entity);
			Assert.Equal(request.BrandId, entity.BrandId);
			Assert.Equal(request.BodyTypeId, entity.BodyTypeId);
			Assert.Equal(request.Name, entity.Name);
			Assert.Equal(request.Url, entity.Url);
			Assert.Equal(request.SeatsCount, entity.SeatsCount);
		}

		/// <summary>
		/// Метод обновления машины должен обновить сущность
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task PutCarAsync_DefaultRequest_ShouldUpdateCarAsync()
		{
			var carToUpdate = await GetEntityAsync<Car>();

			var request = new PutCarRequest()
			{
				Name = "PutRequest",
				BrandId = _anotherBrand.Id,
				BodyTypeId = _anotherBodyType.Id,
				Url = "UpdatedUrl.ru",
				SeatsCount = 1,
			};
			await _client.PutCarAsync(carToUpdate.Id, request);

			var entity = await GetEntityAsync<Car>(x => x.Id == carToUpdate.Id);

			Assert.NotNull(entity);
			Assert.Equal(request.BrandId, entity.BrandId);
			Assert.Equal(request.BodyTypeId, entity.BodyTypeId);
			Assert.Equal(request.Name, entity.Name);
			Assert.Equal(request.Url, entity.Url);
			Assert.Equal(request.SeatsCount, entity.SeatsCount);
		}

		/// <summary>
		/// Метод обновления машины должен обновить сущность
		/// </summary>
		/// <returns>-</returns>
		[Fact]
		public async Task DeleteCarAsync_DefaultRequest_ShouldDeleteCarAsync()
		{
			var carToDelete = await GetEntityAsync<Car>();

			await _client.DeleteCarAsync(carToDelete.Id);

			var deletedCar = await GetEntityAsync<Car>(x => x.Id == carToDelete.Id);
			Assert.Null(deletedCar);
		}
	}
}
