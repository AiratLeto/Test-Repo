using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cars.Api.Contracts.Requests.CarRequests.GetCars;
using Cars.Api.Contracts.Requests.CarRequests.PostCar;
using Cars.Api.Contracts.Requests.CarRequests.PutCar;
using Cars.Api.Core.Clients;

namespace Cars.Api.IntegrationTest
{
	/// <summary>
	/// Клиент микросервиса
	/// </summary>
	public class MicroserviceClient : HttpClientBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="httpClient">HTTP-клиент</param>
		public MicroserviceClient(HttpClient httpClient)
			: base(httpClient)
		{
		}

		#region CarController

		/// <summary>
		/// Получить список машин
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns></returns>
		public async Task<GetCarsResponse> GetCarsAsync(GetCarsRequest request)
			=> await GetAsync<GetCarsResponse>($"/api/Car", request);

		/// <summary>
		/// Обновить машины
		/// </summary>
		/// <param name="id">Идентификатор записи</param>
		/// <param name="request">Запрос на обновление</param>
		/// <returns>-</returns>
		public async Task PutCarAsync(Guid id, PutCarRequest request)
			=> await PutAsync<object>($"/api/Car/{id}", request, false);

		/// <summary>
		/// Создать машины
		/// </summary>
		/// <param name="request">Запрос на создание</param>
		/// <returns>Идентификатор созданной записи</returns>
		public async Task<PostCarResponse> PostCarAsync(PostCarRequest request)
			=> await PostAsync<PostCarResponse>($"/api/Car", request);

		/// <summary>
		/// Удалить машину
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task DeleteCarAsync(Guid id)
			=> await DeleteAsync<object>($"/api/Car/{id}", default, false);

		#endregion
	}
}
