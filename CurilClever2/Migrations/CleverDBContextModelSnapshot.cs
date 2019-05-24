﻿// <auto-generated />
using System;
using CurilClever2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CurilClever2.Migrations
{
    [DbContext(typeof(CleverDBContext))]
    partial class CleverDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CurilClever2.Models.CaptureModel", b =>
                {
                    b.Property<string>("hashstring")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("code");

                    b.HasKey("hashstring");

                    b.ToTable("CaptureModels");
                });

            modelBuilder.Entity("CurilClever2.Models.Client", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfBirthday");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<string>("PassportData")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("SecondName")
                        .IsRequired();

                    b.HasKey("id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CurilClever2.Models.ClientComment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Clientid");

                    b.Property<DateTime>("Posted");

                    b.Property<string>("Text");

                    b.Property<int>("Userid");

                    b.HasKey("id");

                    b.HasIndex("Clientid");

                    b.HasIndex("Userid");

                    b.ToTable("ClientComments");
                });

            modelBuilder.Entity("CurilClever2.Models.Hotel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Addres")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Price")
                        .IsRequired();

                    b.Property<int?>("StarsRate")
                        .IsRequired();

                    b.Property<double>("X");

                    b.Property<double>("Y");

                    b.Property<int>("Zoom");

                    b.HasKey("id");

                    b.ToTable("Hotels");
                });

            modelBuilder.Entity("CurilClever2.Models.News", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<string>("ObjectUrl");

                    b.Property<string>("TextFull");

                    b.Property<string>("TextShort");

                    b.Property<int>("Userid");

                    b.HasKey("id");

                    b.HasIndex("Userid");

                    b.ToTable("News");
                });

            modelBuilder.Entity("CurilClever2.Models.Order", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BeginTravelDate");

                    b.Property<int>("Clientid");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndTravelDate");

                    b.Property<int>("Hotelid");

                    b.Property<int>("PayStatus");

                    b.Property<int>("Price");

                    b.Property<int>("TotalPaid");

                    b.HasKey("id");

                    b.HasIndex("Clientid");

                    b.HasIndex("Hotelid");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CurilClever2.Models.OrderComment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Orderid");

                    b.Property<DateTime>("Posted");

                    b.Property<string>("Text");

                    b.Property<int>("Userid");

                    b.HasKey("id");

                    b.HasIndex("Orderid");

                    b.HasIndex("Userid");

                    b.ToTable("OrderComments");
                });

            modelBuilder.Entity("CurilClever2.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CurilClever2.Models.Subscribe", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("SendNews");

                    b.Property<int>("Userid");

                    b.HasKey("id");

                    b.HasIndex("Userid");

                    b.ToTable("Subscribes");
                });

            modelBuilder.Entity("CurilClever2.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessLevel");

                    b.Property<string>("Login");

                    b.Property<string>("PasswordHash");

                    b.Property<int?>("RoleId");

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CurilClever2.Models.Visit", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("dateTime");

                    b.Property<string>("path");

                    b.Property<string>("userName");

                    b.HasKey("id");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("CurilClever2.Models.ClientComment", b =>
                {
                    b.HasOne("CurilClever2.Models.Client", "Client")
                        .WithMany("Comments")
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CurilClever2.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CurilClever2.Models.News", b =>
                {
                    b.HasOne("CurilClever2.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CurilClever2.Models.Order", b =>
                {
                    b.HasOne("CurilClever2.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CurilClever2.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("Hotelid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CurilClever2.Models.OrderComment", b =>
                {
                    b.HasOne("CurilClever2.Models.Order", "Order")
                        .WithMany("Comments")
                        .HasForeignKey("Orderid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CurilClever2.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CurilClever2.Models.Subscribe", b =>
                {
                    b.HasOne("CurilClever2.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CurilClever2.Models.User", b =>
                {
                    b.HasOne("CurilClever2.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}
