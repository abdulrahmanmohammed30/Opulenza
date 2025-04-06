﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Opulenza.Infrastructure.Common.Persistence;

#nullable disable

namespace Opulenza.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250406075237_UpdateProductDescriptionMaxCharacters")]
    partial class UpdateProductDescriptionMaxCharacters
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("BaseEntitySequence");

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("CategoriesId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Common.BaseEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR [BaseEntitySequence]");

                    SqlServerPropertyBuilderExtensions.UseSequence(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Roles.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Users.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(88)
                        .HasColumnType("nvarchar(88)");

                    b.Property<DateTime>("RefreshTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Common.File", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("OriginalFileName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.ToTable((string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Carts.Cart", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalPriceAfterDiscount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Carts.CartItem", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasIndex("CartId");

                    b.ToTable("CartItems", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Categories.Category", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasIndex("ParentId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Invoices.Invoice", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<string>("InvoiceUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasIndex("OrderId")
                        .IsUnique()
                        .HasFilter("[OrderId] IS NOT NULL");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Orders.Order", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<int?>("ShipmentId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Orders.OrderItem", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("Tax")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("TaxIncluded")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Payments.Payment", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.HasIndex("OrderId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Products.Product", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<string>("Brand")
                        .HasMaxLength(80)
                        .HasColumnType("varchar");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<decimal?>("DiscountPrice")
                        .HasColumnType("DECIMAL(18,2)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("Price")
                        .HasColumnType("DECIMAL(18,2)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("varchar");

                    b.Property<int?>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<decimal>("Tax")
                        .HasColumnType("DECIMAL(18,2)");

                    b.Property<bool>("TaxIncluded")
                        .HasColumnType("bit");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Ratings.Rating", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("ReviewText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Shipments.Shipment", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("ShippingCompany")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("ShippingTracKUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar");

                    b.Property<string>("ShippingTrackId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UserAddressId")
                        .HasColumnType("int");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserAddressId");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Users.UserAddress", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("UserAddresses", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Wishlists.WishListItem", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.BaseEntity");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasIndex("ProductId")
                        .IsUnique()
                        .HasFilter("[ProductId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("WishlistItems");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Categories.CategoryImage", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.File");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFeaturedImage")
                        .HasColumnType("bit");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryImages", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Products.ProductImage", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.File");

                    b.Property<bool>("IsFeaturedImage")
                        .HasColumnType("bit");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages", (string)null);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Users.UserImage", b =>
                {
                    b.HasBaseType("Opulenza.Domain.Common.File");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("UserImages", (string)null);
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Categories.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Opulenza.Domain.Entities.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Roles.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Roles.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Carts.Cart", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithOne("Cart")
                        .HasForeignKey("Opulenza.Domain.Entities.Carts.Cart", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Carts.CartItem", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Carts.Cart", null)
                        .WithMany("Items")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Opulenza.Domain.Entities.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Categories.Category", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Categories.Category", null)
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Invoices.Invoice", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Orders.Order", null)
                        .WithOne()
                        .HasForeignKey("Opulenza.Domain.Entities.Invoices.Invoice", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Orders.Order", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Orders.OrderItem", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Orders.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Opulenza.Domain.Entities.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Payments.Payment", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Orders.Order", null)
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Ratings.Rating", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Products.Product", "Product")
                        .WithMany("Ratings")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Opulenza.Domain.Entities.Ratings.Rating", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Shipments.Shipment", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Orders.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Opulenza.Domain.Entities.Users.UserAddress", "UserAddress")
                        .WithMany()
                        .HasForeignKey("UserAddressId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Order");

                    b.Navigation("UserAddress");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Users.UserAddress", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithOne("Address")
                        .HasForeignKey("Opulenza.Domain.Entities.Users.UserAddress", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Wishlists.WishListItem", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Products.Product", "Product")
                        .WithOne()
                        .HasForeignKey("Opulenza.Domain.Entities.Wishlists.WishListItem", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithMany("WishListItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Categories.CategoryImage", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Categories.Category", null)
                        .WithMany("Images")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Products.ProductImage", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Products.Product", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Users.UserImage", b =>
                {
                    b.HasOne("Opulenza.Domain.Entities.Users.ApplicationUser", null)
                        .WithOne("Image")
                        .HasForeignKey("Opulenza.Domain.Entities.Users.UserImage", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Users.ApplicationUser", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Cart");

                    b.Navigation("Image");

                    b.Navigation("WishListItems");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Carts.Cart", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Categories.Category", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Orders.Order", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Opulenza.Domain.Entities.Products.Product", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
