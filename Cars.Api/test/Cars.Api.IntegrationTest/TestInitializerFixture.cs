using System;
using System.IO;
using System.Threading;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;
using Respawn;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlConnectionStringBuilder = Microsoft.Data.SqlClient.SqlConnectionStringBuilder;

namespace Cars.Api.IntegrationTest
{
	/// <summary>
	/// Инициализация контейнеров для теста
	/// </summary>
	public class TestInitializerFixture : IDisposable
	{
		private readonly int _databaseTestPort = 8100;

		/// <summary>
		/// Чекпоинт
		/// </summary>
		private readonly Checkpoint _checkpoint;

		/// <summary>
		/// Docker-контейнеры
		/// </summary>
		private readonly ICompositeService _container;

		/// <summary>
		/// конструктор
		/// </summary>
		public TestInitializerFixture()
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();

			// подключение к инициализирующей БД создается без БД
			var initialDbConnectionString = config["Application:DbConnectionString"]
				?? throw new InvalidOperationException("Не задана строка подключения к БД");
			var dbConnection = new SqlConnectionStringBuilder(initialDbConnectionString)
			{
				DataSource = $"localhost,{_databaseTestPort}",
				Encrypt = false,
				MultiSubnetFailover = true,
			};
			DbConnectionString = dbConnection.ToString();

			var dockerComposeFile = Path.Combine(Directory.GetCurrentDirectory(), "docker-compose.yml");

			_container = new Ductus.FluentDocker.Builders.Builder()
				.UseContainer()
				.UseCompose()
				.FromFile(dockerComposeFile)
				.ServiceName("carsapiintegrationtests")
				.WithEnvironment(
					$"MSSQLPORT={_databaseTestPort}",
					$"MSSQLPASSWORD={dbConnection.Password}")
				.ForceRecreate()
				.Build()
				.Start();

			// TODO: очень своеобразное место. нужно добавить ожидание
			// внутри самого docker-compose. Но для ТЗ пока так)
			Thread.Sleep(10000);

			_checkpoint = new Checkpoint
			{
				DbAdapter = DbAdapter.SqlServer,
				SchemasToInclude = new[] { "dbo" },
			};
		}

		/// <summary>
		/// Подключение к тестовой БД
		/// </summary>
		public string DbConnectionString { get; }

		/// <summary>
		/// Очистить ресурсы - удалить контейнеры
		/// </summary>
		public void Dispose()
		{
			try
			{
				_container.Dispose();
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Очистить все таблицы в тестовой БД
		/// </summary>
		public void ClearDatabaseData()
		{
			using var connection = new SqlConnection(DbConnectionString);
			connection.Open();
			_checkpoint.Reset(connection).GetAwaiter().GetResult();
			connection.Close();
		}
	}
}
