using System;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Services;
using Cars.Api.Data.MsSql;
using MediatR;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Json;

namespace Cars.Api.Data.Migrator
{
	public class Program : IDesignTimeDbContextFactory<EfContext>
	{
		private const string AppName = "Cars.Api.Data.Migrator";

		public static async Task Main()
		{
			try
			{
				Log.Logger = CreateSystemLogger();
				Log.Information("Initialising.");
				AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
				TaskScheduler.UnobservedTaskException += OnTaskSchedulerOnUnobservedTaskException;

				var serviceProvider = CreateServices();
				using var services = serviceProvider.CreateScope();

				var migrator = services.ServiceProvider.GetRequiredService<DbMigrator>();
				await migrator.MigrateAsync();
			}
			catch (Exception exception)
			{
				Log.Fatal(exception, "Critical error in Main");
				throw;
			}
		}

		/// <inheritdoc/>
		public EfContext CreateDbContext(string[] args)
			=> CreateServices().GetRequiredService<EfContext>();

		private static IServiceProvider CreateServices()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.AddEnvironmentVariables()
				.Build();

			var services = new ServiceCollection();
			return services
				 .AddLogging(builder =>
				 {
					 builder.SetMinimumLevel(LogLevel.Information);
					 builder.AddSerilog(CreateSystemLogger(configuration), dispose: true);
				 })
				.AddMsSql(x => x.ConnectionString = configuration["Application:DbConnectionString"]!)
				.AddMediatR(typeof(Program).Assembly)
				.AddSingleton<IDateTimeProvider, DateTimeProvider>()
				.BuildServiceProvider(false);
		}

		private static void OnTaskSchedulerOnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs eventArgs)
		{
			eventArgs.SetObserved();
			eventArgs.Exception.Flatten().Handle(ex =>
			{
				Log.Error(ex, $"{AppName}: Unhandled exception in Task Scheduler handler");
				return false;
			});
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = e.ExceptionObject as Exception;
			if (e.IsTerminating)
			{
				Log.Fatal(ex, $"{AppName}: Service terminating with fatal exception");
				return;
			}
			Log.Error(ex, $"{AppName}: Unhandled exception in global handler");
		}

		/// <summary>
		/// Создать логгер для инициализации приложения
		/// </summary>
		/// <param name="config">Конфигурация</param>
		/// <returns>Логгер для инициализации приложения</returns>
		private static Logger CreateSystemLogger(IConfiguration? config = null)
		{
			var configuration = new LoggerConfiguration();

			if (config != null)
				configuration = configuration.ReadFrom.Configuration(config);

			return configuration
				.WriteTo.Console(new JsonFormatter(renderMessage: true))
				.WriteTo.Debug()
				.CreateLogger();
		}
	}
}
