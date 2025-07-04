﻿// <auto-generated />
using System;
using InventoryModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventoryModule.Migrations
{
    [DbContext(typeof(InventoryDbContext))]
    [Migration("20250212142910_AddPhysicalCard")]
    partial class AddPhysicalCard
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Inventory")
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("InventoryModule.Domain.Entities.Illustration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Illustrations", "Inventory");
                });

            modelBuilder.Entity("InventoryModule.Domain.Entities.PhysicalCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryCodeWarehouse")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("IllustrationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("SoldAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("VideoCardId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("IllustrationId");

                    b.HasIndex("TagId")
                        .IsUnique();

                    b.ToTable("PhysicalCards", "Inventory");
                });

            modelBuilder.Entity("InventoryModule.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(131071)
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ItemCount")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SKU")
                        .HasMaxLength(31)
                        .HasColumnType("nvarchar(31)");

                    b.Property<decimal>("ShopifyProductId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("ShopifyVariantId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("VariantName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ShopifyProductId", "ShopifyVariantId")
                        .IsUnique();

                    b.ToTable("Products", "Inventory");
                });

            modelBuilder.Entity("InventoryModule.Domain.Entities.SyncJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SyncJobs", "Inventory");
                });

            modelBuilder.Entity("InventoryModule.Domain.Entities.PhysicalCard", b =>
                {
                    b.HasOne("InventoryModule.Domain.Entities.Illustration", "Illustration")
                        .WithMany()
                        .HasForeignKey("IllustrationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Illustration");
                });
#pragma warning restore 612, 618
        }
    }
}
