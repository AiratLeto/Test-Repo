using Cars.Api.Core;
using Cars.Api.Data.MsSql;
using Cars.Api.Web.Cors;
using Cars.Api.Web.Logging;
using Cars.Api.Web.Serialization;
using Cars.Api.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cars.Api.Web
{
	/// <summary>
	/// Стартап веб-сервиса
	/// </summary>
	public class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _webHostEnvironment;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="configuration">Конфигурация</param>
		/// <param name="webHostEnvironment">Окружение и его переменные</param>
		public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
		{
			_configuration = configuration;
			_webHostEnvironment = webHostEnvironment;
		}

		/// <summary>
		/// Конфигурация служб и зависимостей ASP.NET Core
		/// http://blogs.msdn.com/b/webdev/archive/2014/06/17/dependency-injection-in-asp-net-vnext.aspx
		/// </summary>
		/// <param name="services">Службы</param>
		public virtual void ConfigureServices(IServiceCollection services) =>
			services
				.AddCustomCors()
				.AddRouting()
				.AddResponseCaching()
				.AddCustomSwagger()
				.AddHttpContextAccessor()
				.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddControllers()
					.AddCustomJsonOptions(_webHostEnvironment)
				.Services
				.AddMsSql(x => x.ConnectionString = _configuration["Application:DbConnectionString"]!)
				.AddCore();

		/// <summary>
		/// Конфигурация пайплайна обработки запроса ASP.NET Core
		/// </summary>
		/// <param name="application">Билдер приложения</param>
		public virtual void Configure(IApplicationBuilder application) =>
			application
				.UseForwardedHeaders()
				.UseRouting()
				.UseCors(CorsPolicies.AllowAny)
				.UseCustomSerilogRequestLogging()
				.UseExceptionHandling()
				.UseAuthentication()
				.UseAuthorization()
				.UseEndpoints(builder => builder.MapControllers().RequireCors(CorsPolicies.AllowAny))
				.UseCustomSwagger();
	}
}
