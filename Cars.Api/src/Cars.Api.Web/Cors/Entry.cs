using Microsoft.Extensions.DependencyInjection;

namespace Cars.Api.Web.Cors
{
	/// <summary>
	/// Точка входа сервисов CORS для приложения
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить CORS в службы приложения
		/// https://docs.asp.net/en/latest/security/cors.html
		/// </summary>
		/// <param name="services">Службы приложения</param>
		/// <returns>Службы приложения с CORS</returns>
		public static IServiceCollection AddCustomCors(this IServiceCollection services) =>
			services.AddCors(
				options =>
					options.AddPolicy(
						CorsPolicies.AllowAny,
						x => x
							.AllowAnyOrigin()
							.AllowAnyMethod()
							.AllowAnyHeader()));
	}
}
