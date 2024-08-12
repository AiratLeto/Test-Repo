using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Cars.Api.Web.Logging
{
	/// <summary>
	/// Точка входа сервисов логирования для приложения
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить пайп структурного логирования запросов Serilog в пайплайн обработки запросов.
		/// https://github.com/serilog/serilog-aspnetcore
		/// </summary>
		/// <param name="application">Пайплайн обработки запросов</param>
		/// <returns>Пайплайн обработки запросов c логированием</returns>
		public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder application) =>
			application.UseSerilogRequestLogging(
				options =>
				{
					options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
					{
						var request = httpContext.Request;
						var response = httpContext.Response;
						var endpoint = httpContext.GetEndpoint();

						diagnosticContext.Set("Host", request.Host);
						diagnosticContext.Set("Protocol", request.Protocol);
						diagnosticContext.Set("Scheme", request.Scheme);

						if (request.QueryString.HasValue)
							diagnosticContext.Set("QueryString", request.QueryString.Value);

						if (endpoint is not null)
							diagnosticContext.Set("EndpointName", endpoint.DisplayName);

						diagnosticContext.Set("ContentType", response.ContentType);
					};
					options.GetLevel = GetLevel;

					static LogEventLevel GetLevel(HttpContext httpContext, double elapsedMilliseconds, Exception exception)
					{
						if (exception == null && httpContext.Response.StatusCode <= 499)
						{
							if (IsHealthCheckEndpoint(httpContext))
								return LogEventLevel.Verbose;

							return LogEventLevel.Information;
						}

						return LogEventLevel.Error;
					}

					static bool IsHealthCheckEndpoint(HttpContext httpContext)
					{
						var endpoint = httpContext.GetEndpoint();
						if (endpoint is not null)
							return endpoint.DisplayName == "Health checks";

						return false;
					}
				});

		/// <summary>
		/// Использовать обработчик исключений.
		/// Можно заменить на страницу с подробным описанием для разработки:
		/// .UseIf(
		/// _webHostEnvironment.IsDevelopment(),
		/// x => x.UseDeveloperExceptionPage());
		/// </summary>
		/// <param name="builder">Билдер пайплайна ASP.NET Core</param>
		/// <returns>Билдер пайплайна ASP.NET Core с обработкой исключений</returns>
		public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
			=> builder.UseMiddleware<ExceptionHandlingMiddleware>();
	}
}
