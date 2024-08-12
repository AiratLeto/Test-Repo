using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cars.Api.Web.Serialization
{
	/// <summary>
	/// Точка входа сервисов логирования для приложения
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Сконфигурировать службы сериализации ASP.NET Core
		/// </summary>
		/// <param name="builder">Строитель контекста MVC</param>
		/// <param name="webHostEnvironment">Окружение сервиса</param>
		/// <returns>Строитель контекста MVC с параметрами json</returns>
		public static IMvcBuilder AddCustomJsonOptions(
			this IMvcBuilder builder,
			IWebHostEnvironment webHostEnvironment) =>
			builder.AddJsonOptions(
				options =>
				{
					var jsonSerializerOptions = options.JsonSerializerOptions;
					if (webHostEnvironment.IsDevelopment())
					{
						// Pretty print the JSON in development for easier debugging.
						jsonSerializerOptions.WriteIndented = true;
					}

					jsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin);
					jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
					jsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
					jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				});
	}
}
