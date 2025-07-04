﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderModule.Infrastructure;

#nullable disable

namespace OrderModule.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20250212163322_RenameOrderItemProductId")]
    partial class RenameOrderItemProductId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Order")
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrderModule.Domain.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerEmail")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("ShopifyCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("ShopifyCustomerId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("ShopifyOrderId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("ShopifyUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ShopifyCustomerId");

                    b.HasIndex("ShopifyOrderId")
                        .IsUnique();

                    b.ToTable("Orders", "Order");
                });

            modelBuilder.Entity("OrderModule.Domain.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OrderProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PhysicalCardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OrderProductId");

                    b.ToTable("OrderItems", "Order");
                });

            modelBuilder.Entity("OrderModule.Domain.OrderProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderProducts", "Order");
                });

            modelBuilder.Entity("OrderModule.Domain.Sync", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Syncs", "Order");
                });

            modelBuilder.Entity("OrderModule.Domain.OrderItem", b =>
                {
                    b.HasOne("OrderModule.Domain.OrderProduct", "Product")
                        .WithMany("Items")
                        .HasForeignKey("OrderProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OrderModule.Domain.OrderProduct", b =>
                {
                    b.HasOne("OrderModule.Domain.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Common.Shopify.ShopifyProductId", "ShopifyProductId", b1 =>
                        {
                            b1.Property<Guid>("OrderProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("ProductId")
                                .HasColumnType("decimal(20,0)");

                            b1.Property<decimal>("VariantId")
                                .HasColumnType("decimal(20,0)");

                            b1.HasKey("OrderProductId");

                            b1.ToTable("OrderProducts", "Order");

                            b1.WithOwner()
                                .HasForeignKey("OrderProductId");
                        });

                    b.Navigation("Order");

                    b.Navigation("ShopifyProductId")
                        .IsRequired();
                });

            modelBuilder.Entity("OrderModule.Domain.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("OrderModule.Domain.OrderProduct", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
