﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Helpers;

#nullable disable

namespace WebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240103114445_photo2")]
    partial class photo2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApi.Entities.Medicine", b =>
                {
                    b.Property<int>("MedicineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedicineId"), 1L, 1);

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumOfDays")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("Reminder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Repeat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MedicineId");

                    b.HasIndex("PatientId");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("WebApi.Entities.Patient", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PatientId"), 1L, 1);

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("HeartRate")
                        .HasColumnType("float");

                    b.Property<string>("Illness")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("O2")
                        .HasColumnType("float");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<double?>("Radius")
                        .HasColumnType("float");

                    b.Property<double?>("SafeZoneLatitude")
                        .HasColumnType("float");

                    b.Property<double?>("SafeZoneLongitude")
                        .HasColumnType("float");

                    b.Property<double?>("Temperature")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PatientId");

                    b.HasIndex("UserId", "PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("WebApi.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PhoneNumber")
                        .HasColumnType("int");

                    b.Property<string>("ResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResetTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("VerificationTokenExpiry")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApi.Entities.Medicine", b =>
                {
                    b.HasOne("WebApi.Entities.Patient", "Patient")
                        .WithMany("Medicines")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("WebApi.Entities.Patient", b =>
                {
                    b.HasOne("WebApi.Entities.User", "User")
                        .WithMany("Patients")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApi.Entities.Patient", b =>
                {
                    b.Navigation("Medicines");
                });

            modelBuilder.Entity("WebApi.Entities.User", b =>
                {
                    b.Navigation("Patients");
                });
#pragma warning restore 612, 618
        }
    }
}
