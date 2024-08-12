using System.Text;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cars.Api.Core
{
	/// <summary>
	/// Класс - входная точка проекта, регистрирующий реализованные зависимости текущим проектом
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить службы проекта с логикой
		/// </summary>
		/// <param name="services">Коллекция служб</param>
		/// <returns>Обновленная коллекция служб</returns>
		public static IServiceCollection AddCore(this IServiceCollection services)
		{
			// регистрируем нестандартные кодировки
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			services.AddMediatR(typeof(Entry));
			services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
			return services;
		}
	}
}
