using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Cars.Api.Worker
{
	/// <summary>
	/// Фильтр для авторизации пользователя, который щемится в дашборд Hangfire
	/// </summary>
	public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
	{
		/// <inheritdoc/>
		public bool Authorize([NotNull] DashboardContext context) => true;
	}
}
