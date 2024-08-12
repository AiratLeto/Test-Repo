using System;
using Cars.Api.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cars.Api.Data.MsSql
{
	/// <summary>
	/// Класс - входная точка проекта, регистрирующий реализованные зависимости текущим проектом
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить службы проекта с MsSql
		/// </summary>
		/// <param name="services">Коллекция служб</param>
		/// <param name="optionsAction">Параметры подключения к MsSql</param>
		/// <returns>Обновленная коллекция служб</returns>
		public static IServiceCollection AddMsSql(
			this IServiceCollection services,
			Action<MsSqlDbOptions>? optionsAction)
		{
			var options = new MsSqlDbOptions();
			optionsAction?.Invoke(options);

			return services.AddPostgreSql(options);
		}

		/// <summary>
		/// Добавить службы проекта с MsSql
		/// </summary>
		/// <param name="services">Коллекция служб</param>
		/// <param name="options">Конфиг подключения к MsSql</param>
		/// <returns>Обновленная коллекция служб</returns>
		public static IServiceCollection AddPostgreSql(
			this IServiceCollection services,
			MsSqlDbOptions options)
		{
			if (options == null)
				throw new ArgumentNullException(nameof(options));

			if (string.IsNullOrWhiteSpace(options.ConnectionString))
				throw new ArgumentException(nameof(options.ConnectionString));

			services.AddDbContext<EfContext>(opt =>
			{
				opt.UseSnakeCaseNamingConvention();
				opt.UseSqlServer(options!.ConnectionString);
				opt.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			});

			services.AddTransient<DbMigrator>();
			services.AddScoped<IDbContext>(x => x.GetRequiredService<EfContext>());

			return services;
		}
	}
}
