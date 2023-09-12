﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Worker_GrpcService.DAL;

#nullable disable

namespace Worker_GrpcService.DAL.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230912134904_RemoveGendersTableMigration")]
    partial class RemoveGendersTableMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Worker_GrpcService.DAL.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BirthDate")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("birthDate");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("gender");

                    b.Property<bool>("HasChildren")
                        .HasColumnType("boolean")
                        .HasColumnName("hasChildren");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastName");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("patronymic");

                    b.HasKey("Id");

                    b.ToTable("Workers", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthDate = "14.11.2001",
                            FirstName = "Валерия",
                            Gender = "Женский",
                            HasChildren = false,
                            LastName = "Гордеева",
                            Patronymic = "Александровна"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}