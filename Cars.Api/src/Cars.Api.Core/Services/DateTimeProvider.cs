using System;
using Cars.Api.Core.Abstractions;

namespace Cars.Api.Core.Services
{
	/// <inheritdoc/>
	public class DateTimeProvider : IDateTimeProvider
	{
		/// <inheritdoc/>
		public DateTime UtcNow => DateTime.UtcNow;
	}
}
