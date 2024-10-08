﻿// <auto-generated />
using System;
using Cars.Api.Data.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cars.Api.Data.MsSql.Migrations
{
    [DbContext(typeof(EfContext))]
    [Migration("20230919000728_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cars.Api.Core.Entities.BodyType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_body_type");

                    b.ToTable("body_type", "dbo");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Седан"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Хэтчбек"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Универсал"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000004"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Минивэн"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000005"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Внедорожник"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000006"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Купе"
                        });
                });

            modelBuilder.Entity("Cars.Api.Core.Entities.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_brands");

                    b.ToTable("brands", "dbo");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-100000000001"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Audi"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-100000000002"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Ford"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-100000000003"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Jeep"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-100000000004"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Nissan"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-100000000005"),
                            CreatedOn = new DateTime(2023, 9, 18, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Toyota"
                        });
                });

            modelBuilder.Entity("Cars.Api.Core.Entities.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("BodyTypeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("body_type_id");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("brand_id");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<int>("SeatsCount")
                        .HasColumnType("int")
                        .HasColumnName("seats_count");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_cars");

                    b.HasIndex("BodyTypeId")
                        .HasDatabaseName("ix_cars_body_type_id");

                    b.HasIndex("BrandId")
                        .HasDatabaseName("ix_cars_brand_id");

                    b.ToTable("cars", "dbo");
                });

            modelBuilder.Entity("Cars.Api.Core.Entities.Car", b =>
                {
                    b.HasOne("Cars.Api.Core.Entities.BodyType", "BodyType")
                        .WithMany("Cars")
                        .HasForeignKey("BodyTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_cars_body_types_body_type_id");

                    b.HasOne("Cars.Api.Core.Entities.Brand", "Brand")
                        .WithMany("Cars")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_cars_brands_brand_id");

                    b.Navigation("BodyType");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Cars.Api.Core.Entities.BodyType", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("Cars.Api.Core.Entities.Brand", b =>
                {
                    b.Navigation("Cars");
                });
#pragma warning restore 612, 618
        }
    }
}
