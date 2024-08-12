using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cars.Api.Data.MsSql.Conventions
{
	/// <summary>
	/// Преобразование DateTime в UTC формат
	/// </summary>
	public class DateTimeToUtc : ValueConverter<DateTime, DateTime>
	{
		public DateTimeToUtc()
			: base(dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), dateTime => dateTime)
		{
		}
	}
}
