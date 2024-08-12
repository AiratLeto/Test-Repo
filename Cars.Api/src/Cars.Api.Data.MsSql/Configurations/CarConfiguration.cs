using Cars.Api.Core.Entities;
using Cars.Api.Data.MsSql.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cars.Api.Data.MsSql.Configurations
{
	/// <summary>
	/// Конфигурация <see cref="Car"/>
	/// </summary>
	internal class CarConfiguration : EntityBaseConfiguration<Car>
	{
		/// <inheritdoc/>
		public override void ConfigureChild(EntityTypeBuilder<Car> builder)
		{
			builder.ToTable("cars");

			builder.Property(x => x.BrandId).IsRequired();
			builder.Property(x => x.Name)
				.IsRequired()
				.UsePropertyAccessMode(PropertyAccessMode.Field);
			builder.Property(x => x.BodyTypeId).IsRequired();
			builder.Property(x => x.SeatsCount)
				.IsRequired()
				.UsePropertyAccessMode(PropertyAccessMode.Field);
			builder.Property(x => x.Url)
				.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasOne(x => x.Brand)
				.WithMany(x => x.Cars)
				.HasForeignKey(x => x.BrandId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Restrict);
			builder.SetPropertyAccessModeField(x => x.Brand, Car.BrandField);

			builder.HasOne(x => x.BodyType)
				.WithMany(x => x.Cars)
				.HasForeignKey(x => x.BodyTypeId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Restrict);
			builder.SetPropertyAccessModeField(x => x.BodyType, Car.BodyTypeField);
		}
	}
}
