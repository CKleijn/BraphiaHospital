﻿// <auto-generated />
using System;
using DossierManagement.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DossierManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240618194730_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DossierManagement.Features.Dossier.Consult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DossierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DossierId");

                    b.ToTable("Consults");
                });

            modelBuilder.Entity("DossierManagement.Features.Dossier.Dossier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Medications")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("Dossiers");
                });

            modelBuilder.Entity("DossierManagement.Features.Dossier.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BSN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("DossierManagement.Features.Dossier.Consult", b =>
                {
                    b.HasOne("DossierManagement.Features.Dossier.Dossier", null)
                        .WithMany("Consults")
                        .HasForeignKey("DossierId");
                });

            modelBuilder.Entity("DossierManagement.Features.Dossier.Dossier", b =>
                {
                    b.HasOne("DossierManagement.Features.Dossier.Patient", "Patient")
                        .WithOne()
                        .HasForeignKey("DossierManagement.Features.Dossier.Dossier", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("DossierManagement.Features.Dossier.Dossier", b =>
                {
                    b.Navigation("Consults");
                });
#pragma warning restore 612, 618
        }
    }
}
