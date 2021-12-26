﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projContext.DB.Masters;

namespace projContext.Migrations.Master
{
    [DbContext(typeof(MasterContext))]
    partial class MasterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("projContext.DB.Masters.tblBankMaster", b =>
                {
                    b.Property<int>("BankId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BankName")
                        .HasMaxLength(256);

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("IsActive");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.HasKey("BankId");

                    b.ToTable("tblBankMaster");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblCompanyMaster", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlternateContactNo")
                        .HasMaxLength(16);

                    b.Property<string>("AlternateEmail")
                        .HasMaxLength(254);

                    b.Property<string>("City")
                        .HasMaxLength(254);

                    b.Property<string>("ContactNo")
                        .HasMaxLength(16);

                    b.Property<int>("CountryId");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<string>("Email")
                        .HasMaxLength(254);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Locality")
                        .HasMaxLength(254);

                    b.Property<string>("Logo");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(254);

                    b.Property<string>("OfficeAddress")
                        .HasMaxLength(254);

                    b.Property<string>("Pincode")
                        .HasMaxLength(32);

                    b.Property<int>("StateId");

                    b.HasKey("CompanyId");

                    b.ToTable("tblCompanyMaster");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblCountry", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(10);

                    b.Property<string>("ContactPrefix")
                        .HasMaxLength(100);

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("IsActive");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("CountryId");

                    b.ToTable("tblCountry");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblCurrency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("IsActive");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Symbol")
                        .HasMaxLength(8);

                    b.HasKey("CurrencyId");

                    b.ToTable("tblCurrency");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblFileMaster", b =>
                {
                    b.Property<string>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64);

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<byte[]>("File");

                    b.Property<int>("FileType");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.HasKey("FileId");

                    b.ToTable("tblFileMaster");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblLocationMaster", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlternateContactNo")
                        .HasMaxLength(16);

                    b.Property<string>("AlternateEmail")
                        .HasMaxLength(254);

                    b.Property<string>("City")
                        .HasMaxLength(254);

                    b.Property<string>("ContactNo")
                        .HasMaxLength(16);

                    b.Property<int>("CountryId");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<string>("Email")
                        .HasMaxLength(254);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Locality")
                        .HasMaxLength(254);

                    b.Property<int>("LocationType");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(254);

                    b.Property<string>("OfficeAddress")
                        .HasMaxLength(254);

                    b.Property<string>("Pincode")
                        .HasMaxLength(32);

                    b.Property<int>("StateId");

                    b.HasKey("LocationId");

                    b.ToTable("tblLocationMaster");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblOrganisation", b =>
                {
                    b.Property<int>("OrgId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlternateContactNo")
                        .HasMaxLength(16);

                    b.Property<string>("AlternateEmail")
                        .HasMaxLength(254);

                    b.Property<string>("City")
                        .HasMaxLength(254);

                    b.Property<string>("ContactNo")
                        .HasMaxLength(16);

                    b.Property<int>("CountryId");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<string>("Email")
                        .HasMaxLength(254);

                    b.Property<string>("Locality")
                        .HasMaxLength(254);

                    b.Property<string>("Logo");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(254);

                    b.Property<string>("OfficeAddress")
                        .HasMaxLength(254);

                    b.Property<string>("Pincode")
                        .HasMaxLength(32);

                    b.Property<int>("StateId");

                    b.HasKey("OrgId");

                    b.ToTable("tblOrganisation");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblState", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(10);

                    b.Property<int?>("CountryId");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("IsActive");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.HasKey("StateId");

                    b.HasIndex("CountryId");

                    b.ToTable("tblState");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblZoneMaster", b =>
                {
                    b.Property<int>("ZoneId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlternateContactNo")
                        .HasMaxLength(16);

                    b.Property<string>("AlternateEmail")
                        .HasMaxLength(254);

                    b.Property<string>("City")
                        .HasMaxLength(254);

                    b.Property<string>("ContactNo")
                        .HasMaxLength(16);

                    b.Property<int>("CountryId");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<string>("Email")
                        .HasMaxLength(254);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Locality")
                        .HasMaxLength(254);

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(254);

                    b.Property<string>("OfficeAddress")
                        .HasMaxLength(254);

                    b.Property<string>("Pincode")
                        .HasMaxLength(32);

                    b.Property<int>("StateId");

                    b.HasKey("ZoneId");

                    b.ToTable("tblZoneMaster");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblState", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblCountry", "tblCountry")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });
#pragma warning restore 612, 618
        }
    }
}
