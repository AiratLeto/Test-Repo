using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cars.Api.Data.MsSql.Migrations
{
	/// <inheritdoc />
	public partial class InitialMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "body_type",
				schema: "dbo",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
					created_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
					name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_body_type", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "brands",
				schema: "dbo",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
					created_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
					name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_brands", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "cars",
				schema: "dbo",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
					brand_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					body_type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					seats_count = table.Column<int>(type: "int", nullable: false),
					url = table.Column<string>(type: "nvarchar(max)", nullable: true),
					created_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_cars", x => x.id);
					table.ForeignKey(
						name: "fk_cars_body_types_body_type_id",
						column: x => x.body_type_id,
						principalSchema: "dbo",
						principalTable: "body_type",
						principalColumn: "id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "fk_cars_brands_brand_id",
						column: x => x.brand_id,
						principalSchema: "dbo",
						principalTable: "brands",
						principalColumn: "id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.InsertData(
				schema: "dbo",
				table: "body_type",
				columns: new[] { "id", "created_on", "name" },
				values: new object[,]
				{
					{ new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Седан" },
					{ new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Хэтчбек" },
					{ new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Универсал" },
					{ new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Минивэн" },
					{ new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Внедорожник" },
					{ new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Купе" }
				});

			migrationBuilder.InsertData(
				schema: "dbo",
				table: "brands",
				columns: new[] { "id", "created_on", "name" },
				values: new object[,]
				{
					{ new Guid("00000000-0000-0000-0000-100000000001"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Audi" },
					{ new Guid("00000000-0000-0000-0000-100000000002"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Ford" },
					{ new Guid("00000000-0000-0000-0000-100000000003"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Jeep" },
					{ new Guid("00000000-0000-0000-0000-100000000004"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Nissan" },
					{ new Guid("00000000-0000-0000-0000-100000000005"), new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Toyota" }
				});

			migrationBuilder.CreateIndex(
				name: "ix_cars_body_type_id",
				schema: "dbo",
				table: "cars",
				column: "body_type_id");

			migrationBuilder.CreateIndex(
				name: "ix_cars_brand_id",
				schema: "dbo",
				table: "cars",
				column: "brand_id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "cars",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "body_type",
				schema: "dbo");

			migrationBuilder.DropTable(
				name: "brands",
				schema: "dbo");
		}
	}
}
