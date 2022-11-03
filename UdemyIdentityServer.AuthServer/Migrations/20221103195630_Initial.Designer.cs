﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UdemyIdentityServer.AuthServer.Models;

namespace UdemyIdentityServer.AuthServer.Migrations
{
    [DbContext(typeof(CustomDbContext))]
    [Migration("20221103195630_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UdemyIdentityServer.AuthServer.Models.CustomUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CustomUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "İstanbul",
                            Email = "cuneytaran@gmail.com",
                            Password = "12345",
                            UserName = "caran"
                        },
                        new
                        {
                            Id = 2,
                            City = "Konya",
                            Email = "mehmetaran@gmail.com",
                            Password = "12345",
                            UserName = "caran2"
                        },
                        new
                        {
                            Id = 3,
                            City = "İzmir",
                            Email = "cemilaran@gmail.com",
                            Password = "12345",
                            UserName = "caran3"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
