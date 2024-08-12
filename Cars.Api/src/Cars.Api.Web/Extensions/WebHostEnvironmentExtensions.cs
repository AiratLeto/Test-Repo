using Microsoft.AspNetCore.Hosting;

namespace Cars.Api.Web.Extensions
{
	/// <summary>
	/// Расширения для <see cref="IWebHostEnvironment"/>
	/// </summary>
	public static class WebHostEnvironmentExtensions
	{
		/// <summary>
		/// Проверить среду выполнение на тестирование
		/// </summary>
		/// <param name="env">Среда</param>
		/// <returns>Является ли она тестовой</returns>
		public static bool IsTesting(this IWebHostEnvironment env)
			=> env.EnvironmentName == "Test";
	}
}
