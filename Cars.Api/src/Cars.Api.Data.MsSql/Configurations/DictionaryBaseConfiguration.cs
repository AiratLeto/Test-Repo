using Cars.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cars.Api.Data.MsSql.Configurations
{
	/// <summary>
	/// Базовая конфигурация для базовой сущности <see cref="DictionaryBase"/>
	/// </summary>
	/// <typeparam name="TEntity">Тип сущности</typeparam>
	internal abstract class DictionaryBaseConfiguration<TEntity> : EntityBaseConfiguration<TEntity>
		 where TEntity : DictionaryBase
	{
		/// <inheritdoc/>
		public override void ConfigureChild(EntityTypeBuilder<TEntity> builder)
		{
			builder.Property(x => x.Name)
				.IsRequired()
				.UsePropertyAccessMode(PropertyAccessMode.Field);

			ConfigureDictionary(builder);
		}

		/// <summary>
		/// Конфигурация сущности, не считая полей базового класса  <see cref="DictionaryBase"/>
		/// </summary>
		/// <param name="builder">Строитель конфигурации</param>
		protected abstract void ConfigureDictionary(EntityTypeBuilder<TEntity> builder);
	}
}
