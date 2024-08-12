using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Api.Core.Abstractions;
using Cars.Api.Core.Entities;
using Cars.Api.Data.MsSql.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cars.Api.Data.MsSql
{
	/// <summary>
	/// Контекст EF Core для приложения
	/// </summary>
	public class EfContext : DbContext, IDbContext
	{
		private const string DefaultSchema = "dbo";
		private readonly IDateTimeProvider _dateTimeProvider;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="options">Параметры подключения к БД</param>
		/// <param name="dateTimeProvider">Провайдер даты и времени</param>
		public EfContext(
			DbContextOptions<EfContext> options,
			IDateTimeProvider dateTimeProvider)
			: base(options)
			=> _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));

		/// <inheritdoc/>
		public DbSet<Car> Cars { get; set; }

		/// <inheritdoc/>
		public DbSet<Brand> Brands { get; set; }

		/// <inheritdoc/>
		public DbSet<BodyType> BodyTypes { get; set; }

		/// <inheritdoc/>
		public bool IsInMemory => Database.IsInMemory();

		/// <inheritdoc/>
		public void Clean()
		{
			var changedEntriesCopy = ChangeTracker.Entries()
				.Where(e => e.State is EntityState.Added or
							EntityState.Modified or
							EntityState.Deleted)
				.ToList();

			foreach (var entry in changedEntriesCopy)
				entry.State = EntityState.Detached;
		}

		/// <inheritdoc/>
		public override int SaveChanges()
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
			=> SaveChangesAsync(true, default).GetAwaiter().GetResult();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

		/// <inheritdoc/>
		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			var entityEntries = ChangeTracker.Entries().ToArray();

			if (entityEntries.Length > 10)
				entityEntries.AsParallel().ForAll(OnSave);
			else
				foreach (var entityEntry in entityEntries)
					OnSave(entityEntry);

			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		/// <inheritdoc/>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
			await SaveChangesAsync(true, cancellationToken);

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(DefaultSchema);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfContext).Assembly);
		}

		protected sealed override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
			=> configurationBuilder.Properties<DateTime>()
				.HaveConversion(typeof(DateTimeToUtc));

		private void OnSave(EntityEntry entityEntry)
		{
			if (entityEntry.State != EntityState.Unchanged)
				UpdateTimestamp(entityEntry);
		}

		private void UpdateTimestamp(EntityEntry entityEntry)
		{
			var entity = entityEntry.Entity;
			if (entity is null)
				return;

			if (entity is EntityBase table
				&& entityEntry.State == EntityState.Added)
			{
				table.CreatedOn = _dateTimeProvider.UtcNow;
			}
		}
	}
}
