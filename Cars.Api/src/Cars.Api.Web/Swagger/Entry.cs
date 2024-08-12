using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cars.Api.Web.Swagger
{
	/// <summary>
	/// Точка входа сервисов swagger для приложения
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить в службы приложения swagger
		/// </summary>
		/// <param name="services">Службы приложения</param>
		/// <returns>Службы приложения со сваггером</returns>
		public static IServiceCollection AddCustomSwagger(this IServiceCollection services) =>
			services
				.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
				.AddSwaggerGen();

		/// <summary>
		/// Добавить кастомный UI сваггера
		/// </summary>
		/// <param name="application">Пайплайн приложения</param>
		/// <returns>Пайплайн приложения со сваггером</returns>
		public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder application) =>
			application
				.UseSwagger()
				.UseSwaggerUI(
					options =>
					{
						options.DocumentTitle = AssemblyInformation.Current.Product;
						options.RoutePrefix = "swagger";
						options.DisplayOperationId();
						options.DisplayRequestDuration();

						options.SwaggerEndpoint(
							$"/swagger/v1/swagger.json",
							$"Version 1");
					});
	}
}
