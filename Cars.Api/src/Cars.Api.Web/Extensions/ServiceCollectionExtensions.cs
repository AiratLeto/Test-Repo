using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cars.Api.Web.Extensions
{
	/// <summary>
	/// Расширения для <see cref="IServiceCollection"/>
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Выполняет действия с коллекцией служб, если условие истинно
		/// </summary>
		/// <param name="services">Коллекция служб</param>
		/// <param name="condition">Условие</param>
		/// <param name="action">Действие</param>
		/// <returns>Коллекция служб c действием по условию</returns>
		public static IServiceCollection AddIf(
			this IServiceCollection services,
			bool condition,
			Func<IServiceCollection, IServiceCollection> action)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			if (action is null)
				throw new ArgumentNullException(nameof(action));

			if (condition)
				services = action(services);

			return services;
		}

		/// <summary>
		/// Добавить <see cref="IOptions{TOptions}"/> и <typeparamref name="TOptions"/> в службы, как синглтон
		/// </summary>
		/// <typeparam name="TOptions">Тип опций</typeparam>
		/// <param name="services">Коллекция служб</param>
		/// <param name="configuration">Конфигурация</param>
		/// <returns>Коллекция служб c <typeparamref name="TOptions"/></returns>
		public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
			this IServiceCollection services,
			IConfiguration configuration)
			where TOptions : class, new()
		{
			if (services is null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			if (configuration is null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			services
				.AddOptions<TOptions>()
				.Bind(configuration)
				.ValidateDataAnnotations();
			return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
		}
	}
}
