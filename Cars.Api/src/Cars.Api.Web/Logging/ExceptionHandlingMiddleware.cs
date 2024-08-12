using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Cars.Api.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cars.Api.Web.Logging
{
	/// <summary>
	/// Обработчик ошибок API
	/// </summary>
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILoggerFactory _loggerFactory;
		private readonly IHostApplicationLifetime _applicationLifetime;

		/// <summary>
		/// Обработчик ошибок API
		/// </summary>
		/// <param name="next">Делегат следующего обработчика в пайплайне ASP.NET Core</param>
		/// <param name="loggerFactory">Фабрика логгеров</param>
		/// <param name="applicationLifetime">Цикл приложения</param>
		public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILoggerFactory loggerFactory,
			IHostApplicationLifetime applicationLifetime)
		{
			_next = next;
			_loggerFactory = loggerFactory;
			_applicationLifetime = applicationLifetime;
		}

		/// <summary>
		/// Действия по обработке запроса ASP.NET
		/// </summary>
		/// <param name="context">Контекст запроса ASP.NET</param>
		/// <returns>Задача на обработку запроса ASP.NET</returns>
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
		public async Task Invoke(HttpContext context)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
		{
			var originalBodyStream = context.Response.Body;
			try
			{
				var responseBodyText = string.Empty;

				await using var memoryStream = new MemoryStream();
				try
				{
					context.Response.Body = memoryStream;

					await _next(context);
					memoryStream.Seek(0, SeekOrigin.Begin);
					responseBodyText = await new StreamReader(memoryStream).ReadToEndAsync();

					memoryStream.Seek(0, SeekOrigin.Begin);
					context.Response.Body = originalBodyStream;
				}
				finally
				{
					if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
					{
						await HandleBadRequestAsync(context, responseBodyText);
					}
					else
					{
						await memoryStream.CopyToAsync(originalBodyStream);
						context.Response.Body = originalBodyStream;
					}
				}
			}
			catch (NotFoundException ex)
			{
				await HandleNotFoundExceptionAsync(context, ex);
			}
			catch (ApplicationExceptionBase ex)
			{
				await HandleApplicationExceptionBaseAsync(context, ex);
			}
			catch (OutOfMemoryException ex)
			{
				await HandleOutOfMemoryExceptionAsync(context, ex);
			}
			catch (UnauthorizedAccessException ex)
			{
				await HandleUnauthorizedAccessExceptionAsync(context, ex);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		/// <summary>
		/// Создать логгер для контроллера, исполнявшего запрос
		/// </summary>
		/// <param name="context">Контекст запроса</param>
		/// <returns>Логгер</returns>
		private ILogger GetLogger(HttpContext context)
		{
			var endpoint = context.GetEndpoint();
			var controllerActionDescriptor = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>();
			var controllerType = controllerActionDescriptor?.ControllerTypeInfo?.AsType();
			return controllerType != null
				? _loggerFactory.CreateLogger(controllerType)
				: _loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();
		}

		private async Task LogAndReturnAsync(
			HttpContext context,
			Exception exception,
			string errorText,
			HttpStatusCode responseCode,
			LogLevel logLevel,
			Dictionary<string, object>? details = null)
		{
			var errorId = Guid.NewGuid().ToString();
			details ??= new Dictionary<string, object>();
			details.Add("errorId", errorId);

			GetLogger(context).Log(logLevel, exception, "Error #{errorId}", errorId);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)responseCode;

			var response = new ProblemDetails
			{
				Title = errorText,
				Instance = null,
				Status = (int)responseCode,
				Type = null,
				Detail = errorText,
			};
			response.Extensions.Add("ErrorId", errorId);

			foreach (var detail in details)
				response.Extensions.Add(detail.Key, detail.Value);

			var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();
			await context.Response.WriteAsync(
				JsonSerializer.Serialize(response, jsonOptions.Value.JsonSerializerOptions));
		}

		private async Task LogAndReturnAsync(
			HttpContext context,
			string errorTitle,
			string errorText,
			HttpStatusCode responseCode,
			LogLevel logLevel,
			Dictionary<string, object>? details = null)
		{
			var errorId = Guid.NewGuid().ToString();
			details ??= new Dictionary<string, object>();
			details.Add("errorId", errorId);

			GetLogger(context).Log(logLevel, "Error #{errorId}: {errorText}", errorId, errorText);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)responseCode;

			var response = new ProblemDetails
			{
				Title = errorTitle,
				Instance = null,
				Status = (int)responseCode,
				Type = null,
			};
			response.Extensions.Add("ErrorId", errorId);

			foreach (var detail in details)
				response.Extensions.Add(detail.Key, detail.Value);

			var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();
			await context.Response.WriteAsync(
				JsonSerializer.Serialize(response, jsonOptions.Value.JsonSerializerOptions));
		}

		/// <summary>
		/// Обработка исключения <see cref="NotFoundException"/>
		/// </summary>
		/// <param name="context">Контекст запроса ASP.NET</param>
		/// <param name="exception">Исключение</param>
		/// <returns>Задача на обработку запроса ASP.NET</returns>
		private async Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException exception)
		{
			var errorText = exception.Message;
			var logLevel = LogLevel.Warning;
			var responseCode = HttpStatusCode.NotFound;
			await LogAndReturnAsync(context, exception, errorText, responseCode, logLevel);
		}

		/// <summary>
		/// Обработка исключения <see cref="ApplicationExceptionBase"/>
		/// </summary>
		/// <param name="context">Контекст запроса ASP.NET</param>
		/// <param name="exception">Исключение</param>
		/// <returns>Задача на обработку запроса ASP.NET</returns>
		private async Task HandleApplicationExceptionBaseAsync(HttpContext context, ApplicationExceptionBase exception)
		{
			var errorText = exception.Message;
			var logLevel = LogLevel.Warning;
			var responseCode = HttpStatusCode.BadRequest;
			await LogAndReturnAsync(context, exception, errorText, responseCode, logLevel);
		}

		/// <summary>
		/// Обработка исключения <see cref="OutOfMemoryException"/>
		/// </summary>
		/// <param name="context">Контекст запроса ASP.NET</param>
		/// <param name="exception">Исключение</param>
		/// <returns>Задача на обработку запроса ASP.NET</returns>
		private async Task HandleOutOfMemoryExceptionAsync(HttpContext context, OutOfMemoryException exception)
		{
			var errorText = $"Server has troubles with its infrastructure. Please inform system administrator about this issue.";
			var logLevel = LogLevel.Error;
			var responseCode = HttpStatusCode.InternalServerError;
			await LogAndReturnAsync(context, exception, errorText, responseCode, logLevel);

			_applicationLifetime.StopApplication();
		}

		/// <summary>
		/// Обработка исключения <see cref="UnauthorizedAccessException"/>
		/// </summary>
		/// <param name="context">Контекст запроса ASP.NET</param>
		/// <param name="exception">Исключение</param>
		/// <returns>Задача на обработку запроса ASP.NET</returns>
		private async Task HandleUnauthorizedAccessExceptionAsync(HttpContext context, UnauthorizedAccessException exception)
		{
			var errorText = "Доступ к запрашиваемому ресурсу ограничен";
			var logLevel = LogLevel.Warning;
			var responseCode = HttpStatusCode.Forbidden;
			await LogAndReturnAsync(context, exception, errorText, responseCode, logLevel);
		}

		/// <summary>
		/// Обработка исключения <see cref="Exception"/>
		/// </summary>
		/// <param name="context">Контекст запроса ASP.NET</param>
		/// <param name="exception">Исключение</param>
		/// <returns>Задача на обработку запроса ASP.NET</returns>
		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var errorText = exception.Message;
			var logLevel = LogLevel.Error;
			var responseCode = HttpStatusCode.InternalServerError;
			await LogAndReturnAsync(context, exception, errorText, responseCode, logLevel);
		}

		/// <summary>
		/// Обработка исключения Json Validation
		/// </summary>
		/// <param name="context">Контекст запроса ASP.NET</param>
		/// <param name="responseBodyText">Текст тела ответа</param>
		/// <returns>Задача на обработку запроса ASP.NET</returns>
		private async Task HandleBadRequestAsync(HttpContext context, string responseBodyText)
		{
			const string errorTitle = "Некорректный запрос";

			var errors = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBodyText)
				?.FirstOrDefault(x => x.Key == "errors").Value?.ToString() ?? string.Empty;

			var details = JsonSerializer.Deserialize<Dictionary<string, object>>(errors)
				?.Where(x => x.Key != "request")
				.ToDictionary(x => x.Key, x => x.Value);

			await LogAndReturnAsync(
				context: context,
				errorTitle: errorTitle,
				errorText: errors,
				responseCode: HttpStatusCode.BadRequest,
				logLevel: LogLevel.Error,
				details: details);
		}
	}
}
