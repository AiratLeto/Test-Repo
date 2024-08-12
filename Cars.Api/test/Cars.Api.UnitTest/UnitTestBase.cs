using System;
using Cars.Api.Core.Abstractions;
using Cars.Api.Data.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace Cars.Api.UnitTest
{
	/// <summary>
	/// Базовый класс для unit-тестов
	/// </summary>
	public class UnitTestBase : IDisposable
	{
		/// <summary>
		/// Токен
		/// </summary>
		protected const string AuthenticationToken = "AuthenticationToken";

		/// <summary>
		/// Провайдер даты и времени
		/// </summary>
		protected Mock<IDateTimeProvider> DateTimeProvider { get; private set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="testOutputHelper">Назначение для логов</param>
		public UnitTestBase(ITestOutputHelper testOutputHelper)
		{
			DateTimeProvider = new Mock<IDateTimeProvider>();
			DateTimeProvider.Setup(x => x.UtcNow).Returns(new DateTime(2021, 5, 28));

			Log.Logger = new LoggerConfiguration()
				.WriteTo.Debug()
				.WriteTo.TestOutput(testOutputHelper, LogEventLevel.Verbose)
				.CreateLogger();
		}

		/// <summary>
		/// Создать контекст с БД в памяти
		/// </summary>
		/// <param name="dbSeeder">Сидер БД</param>
		/// <param name="dbName">Название</param>
		/// <returns>Контекст EF</returns>
		protected EfContext CreateInMemoryContext(Action<EfContext> dbSeeder = null)
		{
			var options = new DbContextOptionsBuilder<EfContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
				.Options;

			using (var context = new EfContext(options, DateTimeProvider.Object))
			{
				dbSeeder?.Invoke(context);
				context.SaveChangesAsync().GetAwaiter().GetResult();
			}

			// возвращаем чистый контекст для тестов
			return new EfContext(options, DateTimeProvider.Object);
		}

		/// <inheritdoc/>
		public void Dispose() => GC.SuppressFinalize(this);
	}
}
