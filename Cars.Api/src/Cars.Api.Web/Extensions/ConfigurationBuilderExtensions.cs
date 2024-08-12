using System;
using Microsoft.Extensions.Configuration;

namespace Cars.Api.Web.Extensions
{
	/// <summary>
	/// Расширения для <see cref="IConfigurationBuilder"/>
	/// </summary>
	public static class ConfigurationBuilderExtensions
	{
		/// <summary>
		/// Добавить действие над конфигурацией по условию
		/// </summary>
		/// <param name="configurationBuilder">Билдер конфига</param>
		/// <param name="condition">Условие</param>
		/// <param name="action">Действие</param>
		/// <returns>Билдер конфига с действием по условию</returns>
		public static IConfigurationBuilder AddIf(
			this IConfigurationBuilder configurationBuilder,
			bool condition,
			Func<IConfigurationBuilder, IConfigurationBuilder> action)
		{
			if (configurationBuilder is null)
				throw new ArgumentNullException(nameof(configurationBuilder));

			if (action is null)
				throw new ArgumentNullException(nameof(action));

			if (condition)
				configurationBuilder = action(configurationBuilder);

			return configurationBuilder;
		}
	}
}
