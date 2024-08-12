using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Cars.Api.Contracts.Services;
using Cars.Api.Core.Extensions;

namespace Cars.Api.Core.Clients
{
	/// <summary>
	/// Базовый клиент HTTP
	/// </summary>
	public class HttpClientBase
	{
		private readonly HttpClient _httpClient;

		protected HttpClientBase(HttpClient httpClient) => _httpClient = httpClient;

		/// <summary>
		/// Сформировать query string по полям объекта
		/// </summary>
		/// <param name="data">Объект</param>
		/// <returns>query string</returns>
		protected static string GetQueryString(object? data)
		{
			if (data == null)
				return string.Empty;

			var properties = from p in data.GetType().GetProperties()
							 where p.GetValue(data, null) != null
							 select ObjectToQueryStringValue(p.Name, p.GetValue(data, null)!);

			return string.Join("&", properties.ToArray());
		}

		/// <summary>
		/// Сформировать query string по ассоциативному массиву
		/// </summary>
		/// <param name="data">Массив</param>
		/// <returns>query string</returns>
		protected static string GetQueryString(Dictionary<string, string> data)
		{
			if (data is null || !data.Any())
				return string.Empty;

			var properties = from key in data.Keys
							 where data[key] != null
							 select $"{key}={HttpUtility.UrlEncode(data[key].ToString())}";

			return string.Join("&", properties.ToArray());
		}

		/// <summary>
		/// GET
		/// </summary>
		/// <typeparam name="TResponse">Тип ответа</typeparam>
		/// <param name="url">url</param>
		/// <param name="data">query string</param>
		/// <returns>Ответ</returns>
		protected virtual async Task<TResponse> GetAsync<TResponse>(string url, object? data = null)
			where TResponse : new()
		{
			var parameters = GetQueryString(data);
			parameters = !string.IsNullOrEmpty(parameters) ? $"?{parameters}" : parameters;
			var responseMessage = await _httpClient.GetAsync($"{url}{parameters}").ConfigureAwait(false);

			if (!responseMessage.IsSuccessStatusCode)
				await HandleUnsuccessfulResponseAsync<TResponse>(responseMessage).ConfigureAwait(false);

			return await ExtractJsonDataAsync<TResponse>(responseMessage).ConfigureAwait(false);
		}

		/// <summary>
		/// POST
		/// </summary>
		/// <typeparam name="TResponse">Тип ответа</typeparam>
		/// <param name="url">url</param>
		/// <param name="data">Тело</param>
		/// <returns>Ответ</returns>
		protected virtual async Task<TResponse> PostAsync<TResponse>(string url, object data)
		{
			var responseMessage = await _httpClient.PostAsync(url, JsonContent.GetJsonContent(data)).ConfigureAwait(false);

			if (!responseMessage.IsSuccessStatusCode)
				await HandleUnsuccessfulResponseAsync<TResponse>(responseMessage).ConfigureAwait(false);

			return await ExtractJsonDataAsync<TResponse>(responseMessage).ConfigureAwait(false);
		}

		/// <summary>
		/// POST
		/// </summary>
		/// <typeparam name="TResponse">Тип ответа</typeparam>
		/// <param name="url">url</param>
		/// <param name="data">Тело</param>
		/// <returns>Ответ</returns>
		protected virtual async Task<TResponse?> PostAsync<TResponse>(string url, HttpContent data)
			where TResponse : new()
		{
			var responseMessage = await _httpClient.PostAsync(url, data).ConfigureAwait(false);

			if (!responseMessage.IsSuccessStatusCode)
				await HandleUnsuccessfulResponseAsync<TResponse>(responseMessage).ConfigureAwait(false);

			return await ExtractJsonDataAsync<TResponse>(responseMessage).ConfigureAwait(false);
		}

		/// <summary>
		/// PUT
		/// </summary>
		/// <typeparam name="TResponse">Тип ответа</typeparam>
		/// <param name="url">url</param>
		/// <param name="data">Тело</param>
		/// <param name="hasReturnValue">Есть ли возвращаемое значение</param>
		/// <returns>Ответ</returns>
		protected virtual async Task<TResponse?> PutAsync<TResponse>(string url, object data, bool hasReturnValue = true)
		{
			var responseMessage = await _httpClient.PutAsync(url, JsonContent.GetJsonContent(data)).ConfigureAwait(false);

			if (!responseMessage.IsSuccessStatusCode)
				await HandleUnsuccessfulResponseAsync<TResponse>(responseMessage).ConfigureAwait(false);

			return hasReturnValue
				? await ExtractJsonDataAsync<TResponse>(responseMessage).ConfigureAwait(false)
				: default;
		}

		/// <summary>
		/// DELETE
		/// </summary>
		/// <typeparam name="TResponse">Тип ответа</typeparam>
		/// <param name="url">url</param>
		/// <param name="data">query string</param>
		/// <param name="hasReturnValue">Есть ли возвращаемое значение</param>
		/// <returns>Ответ</returns>
		protected virtual async Task<TResponse?> DeleteAsync<TResponse>(string url, object data, bool hasReturnValue = true)
		{
			var responseMessage = await DeleteAsJsonAsync(_httpClient, url, data).ConfigureAwait(false);

			if (!responseMessage.IsSuccessStatusCode)
				await HandleUnsuccessfulResponseAsync<TResponse>(responseMessage).ConfigureAwait(false);

			return hasReturnValue
				? await ExtractJsonDataAsync<TResponse>(responseMessage).ConfigureAwait(false)
				: default;
		}

		/// <summary>
		/// Удаление с доп данными
		/// </summary>
		/// <typeparam name="T">Тип данных</typeparam>
		/// <param name="httpClient">HttpClient</param>
		/// <param name="url">url</param>
		/// <param name="data">данные</param>
		/// <returns>-</returns>
		private static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(
			HttpClient httpClient,
			string url,
			T data)
		=> httpClient.SendAsync(
			new HttpRequestMessage(HttpMethod.Delete, url)
			{
				Content = JsonContent.GetJsonContent(data!),
			});

		private static string ObjectToQueryStringValue(string key, object value)
		{
			if (value is null)
				return $"{key}=";

			if (value is IEnumerable and not string)
			{
				var options = new List<string>();
				foreach (var item in (value as IEnumerable)!)
					options.Add($"{key}={HttpUtility.UrlEncode(item?.ToString() ?? string.Empty)}");
				return string.Join("&", options);
			}

			return $"{key}={HttpUtility.UrlEncode(value?.ToString() ?? string.Empty)}";
		}

		private static async Task<TResponse> ExtractJsonDataAsync<TResponse>(HttpResponseMessage responseMessage)
		{
			if (responseMessage?.Content is null)
				return default!;

			var responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
			if (responseStream is null)
				return default!;

			return (await JsonSerializer
				.DeserializeAsync<TResponse>(responseStream, JsonContent.CreateOptions())
				.ConfigureAwait(false))!;
		}

		/// <summary>
		/// Обработка исключения для ответа с ошибочным статусом
		/// </summary>
		/// <typeparam name="TResponse">Тип ответа</typeparam>
		/// <param name="responseMessage">Ответ сервера</param>
		/// <returns>Ответ на запрос</returns>
		private async Task<TResponse> HandleUnsuccessfulResponseAsync<TResponse>(HttpResponseMessage responseMessage)
		{
			try
			{
				var details = await ExtractJsonDataAsync<ProblemDetailsResponse>(responseMessage).ConfigureAwait(false);
				var message = details?.Title ?? details?.Detail ?? "Ошибка при обработке запроса";
				throw new ClientException(message, responseMessage);
			}
			catch (JsonException)
			{
				var responseText = await responseMessage.Content.ReadAsStringAsync();
				throw new ClientException($"Произошло неожиданное исключение: {responseText}", responseMessage);
			}
		}
	}
}
