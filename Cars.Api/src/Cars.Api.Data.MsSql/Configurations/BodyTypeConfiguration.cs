using System;
using System.Collections.Generic;
using System.Linq;
using Cars.Api.Core.Entities;
using Cars.Api.Data.MsSql.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cars.Api.Data.MsSql.Configurations
{
	/// <summary>
	/// Конфигурация <see cref="BodyType"/>
	/// </summary>
	internal class BodyTypeConfiguration : DictionaryBaseConfiguration<BodyType>
	{
		/// <inheritdoc/>
		protected override void ConfigureDictionary(EntityTypeBuilder<BodyType> builder)
		{
			builder.ToTable("body_type");

			var types = new Dictionary<Guid, string>
			{
				[Guid.Parse("00000000-0000-0000-0000-000000000001")] = "Седан",
				[Guid.Parse("00000000-0000-0000-0000-000000000002")] = "Хэтчбек",
				[Guid.Parse("00000000-0000-0000-0000-000000000003")] = "Универсал",
				[Guid.Parse("00000000-0000-0000-0000-000000000004")] = "Минивэн",
				[Guid.Parse("00000000-0000-0000-0000-000000000005")] = "Внедорожник",
				[Guid.Parse("00000000-0000-0000-0000-000000000006")] = "Купе",
			};
			builder.HasData(types.Select(x => BodyType.CreateForMigration(x.Key, x.Value, MigrationConstants.InitialMigrationDate)));
		}
	}
}
