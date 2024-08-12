using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Data.MsSql;
using Cars.Api.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace Cars.Api.IntegrationTest
{
	/// <summary>
	/// Фабрика сервисов для интеграционного тестирования
	/// </summary>
	/// <typeparam name="TEntryPoint">Startup</typeparam>
	public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
	{
		/// <summary>
		/// БД
		/// </summary>
		private readonly TestInitializerFixture _testFixture;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Назначение для логов</param>
		/// <param name="testFixture">Инициализация контейнеров</param>
		public CustomWebApplicationFactory(
			ITestOutputHelper testOutputHelper,
			TestInitializerFixture testFixture)
		{
			ClientOptions.AllowAutoRedirect = false;

			DateTimeProvider = new Mock<IDateTimeProvider>();
			DateTimeProvider.Setup(x => x.UtcNow).Returns(new DateTime(2021, 5, 28));

			Log.Logger = new LoggerConfiguration()
				.WriteTo.Debug()
				.WriteTo.TestOutput(testOutputHelper, LogEventLevel.Verbose)
				.CreateLogger();

			_testFixture = testFixture;
		}

		/// <summary>
		/// Провайдер даты и времени
		/// </summary>
		protected Mock<IDateTimeProvider> DateTimeProvider { get; private set; }

		/// <summary>
		/// Получить сущность из БД
		/// </summary>
		/// <typeparam name="TResult">Тип сущности</typeparam>
		/// <param name="condition">Условие</param>
		/// <returns>Сущность</returns>
		protected async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TResult, bool>> condition = default)
			where TResult : EntityBase
		{
			using var scope = Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<EfContext>();
			return await context.Set<TResult>().FirstOrDefaultAsync(condition ?? (x => true));
		}

		/// <summary>
		/// Создать HTTP-клиента тест-сервиса с моками хранилищ в памяти
		/// </summary>
		/// <param name="dbSeeder">Инициализация БД</param>
		/// <returns>HTTP-клиент</returns>
		protected HttpClient CreateClient(Action<EfContext> dbSeeder = null)
		{
			var client = base.CreateClient();
			using var scope = Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<EfContext>();
			context.Database.EnsureCreated();

			dbSeeder?.Invoke(context);
			context.SaveChangesAsync().GetAwaiter().GetResult();

			return client;
		}

		/// <summary>
		/// Создать API-клиента тест-сервиса с моками хранилищ в памяти
		/// </summary>
		/// <param name="dbSeeder">Инициализация БД</param>
		/// <returns>Клиентская библиотека сервиса</returns>
		protected virtual MicroserviceClient CreateApiClient(Action<EfContext> dbSeeder = null)
			=> new(CreateClient(dbSeeder));

		protected override void ConfigureWebHost(IWebHostBuilder builder)
			=> builder
			.ConfigureAppConfiguration((context, conf)
				=> conf
					.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
					.AddEnvironmentVariables()
					.AddInMemoryCollection(new List<KeyValuePair<string, string>>
					{
						// переопределение строки подключения для интеграционных тестов
						new("Application:DbConnectionString", _testFixture.DbConnectionString),
					}))
				.UseEnvironment("Test")
				.ConfigureTestServices(ConfigureServices);

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<EfContext>(options =>
				options.UseSqlServer(_testFixture.DbConnectionString));
			services.AddTransient(x => DateTimeProvider.Object);
		}

		protected override void Dispose(bool disposing)
		{
			_testFixture.ClearDatabaseData();
			base.Dispose(disposing);
		}
	}
}
