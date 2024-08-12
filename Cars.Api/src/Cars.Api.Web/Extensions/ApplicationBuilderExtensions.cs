using System;
using Microsoft.AspNetCore.Builder;

namespace Cars.Api.Web.Extensions
{
	/// <summary>
	/// Расширения для <see cref="IApplicationBuilder"/>
	/// </summary>
	public static class ApplicationBuilderExtensions
	{
		/// <summary>
		/// Применить пайп в пайплайне обработки запроса ASP.NET Core по условию
		/// </summary>
		/// <param name="application">Билдер пайплайна</param>
		/// <param name="condition">Условие</param>
		/// <param name="action">Пайп</param>
		/// <returns>Билдер пайплайна с пайпом по условию</returns>
		public static IApplicationBuilder UseIf(
			this IApplicationBuilder application,
			bool condition,
			Func<IApplicationBuilder, IApplicationBuilder> action)
		{
			if (application is null)
				throw new ArgumentNullException(nameof(application));

			if (action is null)
				throw new ArgumentNullException(nameof(action));

			if (condition)
				application = action(application);

			return application;
		}
	}
}
