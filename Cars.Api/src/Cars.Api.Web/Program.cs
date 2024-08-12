using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Cars.Api.Data.MsSql;
using Cars.Api.Web.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Extensions.Hosting;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Cars.Api.Web
{
	public static class Program
	{
		private static readonly ITextFormatter ConsoleTextFormatter = new JsonFormatter(renderMessage: true);

		public static async Task<int> Main(string[] args)
		{
			Log.Logger = CreateBootstrapLogger();
			IHostEnvironment? hostEnvironment = null;

			try
			{
				Log.Information("Initialising.");
				var host = CreateHostBuilder(args).Build();
				hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
				hostEnvironment.ApplicationName = AssemblyInformation.Current.Product;

				// TODO: вынести миграции в шаблон пода как действие перед стартом пода
				Log.Information(
					"Running {Application} migrations in {Environment} mode.",
					hostEnvironment.ApplicationName,
					hostEnvironment.EnvironmentName);

				using (var services = host.Services.CreateScope())
				{
					var migrator = services.ServiceProvider.GetRequiredService<DbMigrator>();
					await migrator.MigrateAsync();
				}
				Log.Information(
					"Started {Application} in {Environment} mode.",
					hostEnvironment.ApplicationName,
					hostEnvironment.EnvironmentName);

				await host.RunAsync().ConfigureAwait(false);

				Log.Information(
					"Stopped {Application} in {Environment} mode.",
					hostEnvironment.ApplicationName,
					hostEnvironment.EnvironmentName);
				return 0;
			}
			catch (Exception exception)
			{
				if (hostEnvironment is null)
					Log.Fatal(exception, "Application terminated unexpectedly while initialising.");
				else
					Log.Fatal(
						exception,
						"{Application} terminated unexpectedly in {Environment} mode.",
						hostEnvironment.ApplicationName,
						hostEnvironment.EnvironmentName);

				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			new HostBuilder()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureHostConfiguration(
					configurationBuilder => configurationBuilder
						.AddEnvironmentVariables(prefix: "DOTNET_")
						.AddIf(
							args is not null,
							x => x.AddCommandLine(args)))
				.ConfigureAppConfiguration((hostingContext, config) =>
					AddConfiguration(config, hostingContext.HostingEnvironment, args))
				.UseSerilog(ConfigureApplicationLogger)
				.UseDefaultServiceProvider(
					(context, options) =>
					{
						var isDevelopment = context.HostingEnvironment.IsDevelopment();
						options.ValidateScopes = isDevelopment;
						options.ValidateOnBuild = isDevelopment;
					})
				.ConfigureWebHost(ConfigureWebHostBuilder)
				.UseConsoleLifetime();

		private static void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder) =>
			webHostBuilder
				.UseKestrel(
					(builderContext, options) => options.AddServerHeader = false)
				.UseIIS()
				.UseStartup<Startup>();

		private static IConfigurationBuilder AddConfiguration(
			IConfigurationBuilder configurationBuilder,
			IHostEnvironment hostEnvironment,
			string[] args) =>
			configurationBuilder
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
				.AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
				.AddKeyPerFile(Path.Combine(Directory.GetCurrentDirectory(), "configuration"), optional: true, reloadOnChange: false)
				.AddIf(
					hostEnvironment.IsDevelopment() && !string.IsNullOrEmpty(hostEnvironment.ApplicationName),
					x => x.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: false))
				.AddEnvironmentVariables()
				.AddIf(
					args is not null,
					x => x.AddCommandLine(args));

		/// <summary>
		/// Создать логгер для инициализации приложения
		/// <see href="https://nblumhardt.com/2020/10/bootstrap-logger/"/>
		/// </summary>
		/// <returns>Логгер для инициализации приложения</returns>
		private static ReloadableLogger CreateBootstrapLogger()
		{
			var isDevelopment = string.Equals(
				Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
				"development",
				StringComparison.OrdinalIgnoreCase);

			var configuration = new LoggerConfiguration()
				.WriteTo.Debug()
				.Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
					.WithDefaultDestructurers()
					.WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }));

			configuration = isDevelopment
				? configuration.WriteTo.Console()
				: configuration.WriteTo.Console(ConsoleTextFormatter);

			return configuration.CreateBootstrapLogger();
		}

		/// <summary>
		/// Создать логгер для работы приложения
		/// </summary>
		/// <param name="context">Билдер хоста</param>
		/// <param name="services">Службы</param>
		/// <param name="configuration">Конфиг логгера</param>
		private static void ConfigureApplicationLogger(
			HostBuilderContext context,
			IServiceProvider services,
			LoggerConfiguration configuration)
		{
			configuration
				.ReadFrom.Configuration(context.Configuration)
				.ReadFrom.Services(services)
				.Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
				.Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
				.Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
					.WithDefaultDestructurers()
					.WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }));

			if (context.HostingEnvironment.IsDevelopment())
				configuration.WriteTo.Console();
			else
				configuration.WriteTo.Console(ConsoleTextFormatter);
		}
	}
}
