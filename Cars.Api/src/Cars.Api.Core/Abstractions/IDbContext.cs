using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cars.Api.Core.Abstractions
{
	/// <summary>
	/// Контекст EF Core для приложения
	/// </summary>
	public interface IDbContext
	{
		/// <summary>
		/// Машины
		/// </summary>
		DbSet<Car> Cars { get; }

		/// <summary>
		/// Бренды
		/// </summary>
		DbSet<Brand> Brands { get; }

		/// <summary>
		/// Типы кузова
		/// </summary>
		DbSet<BodyType> BodyTypes { get; }

		/// <summary>
		/// БД в памяти
		/// </summary>
		bool IsInMemory { get; }

		/// <summary>
		/// Очистить UnitOfWork
		/// </summary>
		void Clean();

		/// <summary>
		/// Сохранить изменения в БД
		/// </summary>
		/// <param name="acceptAllChangesOnSuccess">Применить изменения при успешном сохранении в контекст</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Количество обновленных записей</returns>
		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

		/// <summary>
		/// Сохранить изменения в БД
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Количество обновленных записей</returns>
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
