﻿// <auto-generated />
using System;
using DeliveryApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeliveryApi.Migrations
{
    [DbContext(typeof(DeliveryContext))]
    [Migration("20231013131650_add__roles_in_user")]
    partial class add__roles_in_user
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DeliveryApi.Models.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DishingCartUserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsVegetarian")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DishingCartUserEmail");

                    b.ToTable("Dish");
                });

            modelBuilder.Entity("DeliveryApi.Models.DishingCart", b =>
                {
                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserEmail");

                    b.ToTable("DishingCart");
                });

            modelBuilder.Entity("DeliveryApi.Models.Rating", b =>
                {
                    b.Property<Guid>("DishId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserDtoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("DishId");

                    b.HasIndex("UserDtoId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("DeliveryApi.Models.UserDTO", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DeliveryApi.Models.Dish", b =>
                {
                    b.HasOne("DeliveryApi.Models.DishingCart", null)
                        .WithMany("Dishes")
                        .HasForeignKey("DishingCartUserEmail");
                });

            modelBuilder.Entity("DeliveryApi.Models.Rating", b =>
                {
                    b.HasOne("DeliveryApi.Models.UserDTO", "UserDto")
                        .WithMany()
                        .HasForeignKey("UserDtoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserDto");
                });

            modelBuilder.Entity("DeliveryApi.Models.DishingCart", b =>
                {
                    b.Navigation("Dishes");
                });
#pragma warning restore 612, 618
        }
    }
}