using System;
using System.Linq;
using System.Linq.Expressions;
using Cars.Api.Core.Exceptions;
using Cars.Api.Core.Models;

namespace Cars.Api.Core.Extensions
{
	/// <summary>
	/// Расширения <see cref="IQueryable"/>
	/// </summary>
	public static class IQueryableExtensions
	{
		/// <summary>
		/// Применить пагинацию
		/// <see cref="IPaginationQuery"/>
		/// </summary>
		/// <typeparam name="T">Тип IQueryable</typeparam>
		/// <param name="queryable">IQueryable</param>
		/// <param name="pagination">Пагинация</param>
		/// <returns>IQueryable с пагинацией</returns>
		public static IQueryable<T> SkipTake<T>(this IQueryable<T> queryable, IPaginationQuery pagination)
		{
			if (queryable == null)
				throw new ArgumentNullException(nameof(queryable));

			if (pagination == null || pagination.PageNumber <= 0 || pagination.PageSize <= 0)
				return queryable;

			return queryable
				.Skip((pagination.PageNumber - 1) * pagination.PageSize)
				.Take(pagination.PageSize);
		}

		/// <summary>
		/// Применить сортировку
		/// <see cref="IPaginationQuery"/>
		/// </summary>
		/// <typeparam name="T">Тип IQueryable</typeparam>
		/// <param name="queryable">IQueryable</param>
		/// <param name="orderBy">Сортировка</param>
		/// <returns>IQueryable с сортировкой</returns>
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, IOrderByQuery orderBy)
		{
			if (queryable == null)
				throw new ArgumentNullException(nameof(queryable));

			if (string.IsNullOrWhiteSpace(orderBy?.OrderBy))
				return queryable;

			return queryable.OrderByField(orderBy.OrderBy, orderBy.IsAscending);
		}

		/// <summary>
		/// Сортировать по полю, указанному в текстовом виде
		/// Стащено отсюда: https://stackoverflow.com/a/22227975
		/// </summary>
		/// <typeparam name="T">Тип сущностей IQueryable</typeparam>
		/// <param name="queryable">IQueryable</param>
		/// <param name="propertyName">Поле для сортировки</param>
		/// <param name="isAscending">По возрастанию ли</param>
		/// <returns>IQueryable отсортированный</returns>
		public static IOrderedQueryable<T> OrderByField<T>(this IQueryable<T> queryable, string propertyName, bool isAscending = true)
		{
			if (queryable == null)
				throw new ArgumentNullException(nameof(queryable));

			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			var param = Expression.Parameter(queryable.ElementType, "p");
			var prop = (Expression)param;
			foreach (var property in propertyName.Split('.'))
				prop = ExtractProperty(prop, property);

			var exp = Expression.Lambda(prop, param);
			var method = isAscending ? "OrderBy" : "OrderByDescending";

			// PostgreSQL сортирует DateTime? через NULL FIRST по умолчанию, в какую бы сторону сортировка ни шла
			if (exp.ReturnType == typeof(DateTime?))
			{
				queryable = queryable.OrderByHasValue(propertyName, false);
				method = isAscending ? "ThenBy" : "ThenByDescending";
			}

			var types = new[] { queryable.ElementType, exp.Body.Type };
			var mce = Expression.Call(typeof(Queryable), method, types, queryable.Expression, exp);
#pragma warning disable CS8603 // Possible null reference return.
			return queryable.Provider.CreateQuery<T>(mce) as IOrderedQueryable<T>;
#pragma warning restore CS8603 // Possible null reference return.
		}

		/// <summary>
		/// Сортировать по полю по убыванию, указанному в текстовом виде
		/// Стащено отсюда: https://stackoverflow.com/a/22227975
		/// </summary>
		/// <typeparam name="T">Тип сущностей IQueryable</typeparam>
		/// <param name="queryable">IQueryable</param>
		/// <param name="field">Поле для сортировки</param>
		/// <returns>IQueryable отсортированный</returns>
		public static IOrderedQueryable<T> OrderByFieldDescending<T>(this IQueryable<T> queryable, string field)
			=> queryable.OrderByField(field, false);

		/// <summary>
		/// Сортировать по наличию свойства
		/// Стащено отсюда: https://stackoverflow.com/a/22227975
		/// </summary>
		/// <typeparam name="T">Тип сущностей IQueryable</typeparam>
		/// <param name="queryable">IQueryable</param>
		/// <param name="propertyName">Поле для сортировки</param>
		/// <param name="isAscending">По возрастанию ли</param>
		/// <returns>IQueryable отсортированный</returns>
		// ReSharper disable once SuggestBaseTypeForParameter
		private static IOrderedQueryable<T> OrderByHasValue<T>(this IQueryable<T> queryable, string propertyName, bool isAscending = true)
		{
			if (queryable == null)
				throw new ArgumentNullException(nameof(queryable));

			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			var param = Expression.Parameter(typeof(T), "p");
			var prop = (Expression)param;
			foreach (var property in propertyName.Split('.'))
				prop = ExtractProperty(prop, property);
			prop = Expression.Property(prop, "HasValue");

			var exp = Expression.Lambda(prop, param);
			var method = isAscending ? "OrderBy" : "OrderByDescending";
			var types = new[] { queryable.ElementType, exp.Body.Type };
			var mce = Expression.Call(typeof(Queryable), method, types, queryable.Expression, exp);
#pragma warning disable CS8603 // Possible null reference return.
			return queryable.Provider.CreateQuery<T>(mce) as IOrderedQueryable<T>;
#pragma warning restore CS8603 // Possible null reference return.
		}

		/// <summary>
		/// Применить название свойства к выражению доступа к свойству
		/// </summary>
		/// <param name="expression">Выражение доступа к свойству</param>
		/// <param name="property">Свойство</param>
		/// <exception cref="InvalidOrderByExpressionException">Если не удалось преобразовать к свойству или полю</exception>
		/// <returns>Выражение со свойством или полем или исключение</returns>
		private static Expression ExtractProperty(Expression expression, string property)
		{
			try
			{
				return Expression.PropertyOrField(expression, property);
			}
			catch (ArgumentException)
			{
				throw new InvalidOrderByExpressionException();
			}
		}
	}
}
