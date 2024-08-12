using System;
using System.Collections.Generic;
using Cars.Api.Worker.Workers;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Cars.Api.Worker
{
	/// <summary>
	/// Класс - входная точка проекта, регистрирующий реализованные зависимости текущим проектом
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить службы проекта с тасками по расписанию
		/// </summary>
		/// <param name="services">Коллекция служб</param>
		/// <returns>Обновленная коллекция служб</returns>
		public static IServiceCollection AddHangfireWorker(this IServiceCollection services)
			=> services
				.AddHangfire(x => x.UseMemoryStorage())
				.AddHangfireServer();

		/// <summary>
		/// Использование Hangfire в pipeline
		/// </summary>
		/// <param name="app">app</param>
		/// <returns>IApplicationBuilder</returns>
		public static IApplicationBuilder UseHangfireWorker(this IApplicationBuilder app, bool useDashboard)
		{
			if (useDashboard)
				app.UseHangfireDashboard("/worker", new DashboardOptions
				{
					Authorization = new[] { new DashboardAuthorizationFilter() },
				});

			AddJob<CarTestWorker>("*/1 * * * *");

			return app;
		}

		/// <summary>
		/// Добавить healthcheck на Hangfire
		/// </summary>
		/// <param name="healthChecksBuilder">Строитель healthcheck'ов</param>
		/// <param name="name">Назвнаие</param>
		/// <param name="failureStatus">Статус ошибки</param>
		/// <param name="tags">Тэги</param>
		/// <returns>Строитель healthcheck-ов</returns>
		public static IHealthChecksBuilder AddHangfireWorker(
			this IHealthChecksBuilder healthChecksBuilder,
			string? name = null,
			HealthStatus? failureStatus = null,
			IEnumerable<string>? tags = null)
				=> healthChecksBuilder.AddHangfire(
					x =>
					{
						x.MaximumJobsFailed = null;
						x.MinimumAvailableServers = 1;
					},
					name,
					failureStatus,
					tags);

		/// <summary>
		/// Добавить работу по расписанию
		/// </summary>
		/// <typeparam name="T">Тип работы</typeparam>
		/// <param name="cron">CRON-расписание</param>
		private static void AddJob<T>(string cron)
			where T : IWorker
			=> RecurringJob.AddOrUpdate<T>(
				typeof(T).FullName,
				(x) => x.RunAsync(),
				cron,
				TimeZoneInfo.Local);
	}
}
