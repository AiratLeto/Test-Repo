using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Cars.Api.Core.Extensions
{
	/// <summary>
	/// JsonContent
	/// </summary>
	public static class JsonContent
	{
		/// <summary>
		/// Сереализовать данные
		/// </summary>
		/// <param name="data">данные</param>
		/// <returns>-</returns>
		public static StringContent GetJsonContent(object data)
			=> new(JsonSerializer.Serialize(data, CreateOptions()), Encoding.UTF8, "application/json");

		/// <summary>
		/// Создать JsonSerializerOptions
		/// </summary>
		/// <returns>-</returns>
		public static JsonSerializerOptions CreateOptions()
		{
			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin),
			};
			options.Converters.Add(new JsonStringEnumConverter());

			options.PropertyNameCaseInsensitive = true;
			return options;
		}
	}
}
