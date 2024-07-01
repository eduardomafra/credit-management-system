﻿// <auto-generated />
using System;
using CreditCardService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CreditCardService.Infrastructure.Migrations
{
    [DbContext(typeof(CreditCardDbContext))]
    [Migration("20240630214542_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CreditCardService.Domain.Entities.CreditCard", b =>
                {
                    b.Property<int>("CreditCardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CreditCardId"));

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("CreditLimit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CreditProposalId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirityDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CreditCardId");

                    b.ToTable("CreditCards");
                });
#pragma warning restore 612, 618
        }
    }
}
