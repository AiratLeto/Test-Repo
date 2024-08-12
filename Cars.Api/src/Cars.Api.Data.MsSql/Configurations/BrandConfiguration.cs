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
	/// Конфигурация <see cref="Brand"/>
	/// </summary>
	internal class BrandConfiguration : DictionaryBaseConfiguration<Brand>
	{
		/// <inheritdoc/>
		protected override void ConfigureDictionary(EntityTypeBuilder<Brand> builder)
		{
			builder.ToTable("brands");

			var brands = new Dictionary<Guid, string>
			{
				[Guid.Parse("00000000-0000-0000-0000-100000000001")] = "Audi",
				[Guid.Parse("00000000-0000-0000-0000-100000000002")] = "Ford",
				[Guid.Parse("00000000-0000-0000-0000-100000000003")] = "Jeep",
				[Guid.Parse("00000000-0000-0000-0000-100000000004")] = "Nissan",
				[Guid.Parse("00000000-0000-0000-0000-100000000005")] = "Toyota",
			};
			builder.HasData(brands.Select(x => Brand.CreateForMigration(x.Key, x.Value, MigrationConstants.InitialMigrationDate)));
		}
	}
}
