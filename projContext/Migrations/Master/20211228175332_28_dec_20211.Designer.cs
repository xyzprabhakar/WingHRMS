﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projContext.DB.Masters;

namespace projContext.Migrations.Master
{
    [DbContext(typeof(MasterContext))]
    [Migration("20211228175332_28_dec_20211")]
    partial class _28_dec_20211
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("OrgId");

                    b.Property<string>("Pincode")
                        .HasMaxLength(32);

                    b.Property<int>("StateId");

                    b.HasKey("CompanyId");

                    b.HasIndex("OrgId");

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

                    b.Property<int?>("ZoneId");

                    b.HasKey("LocationId");

                    b.HasIndex("ZoneId");

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

            modelBuilder.Entity("projContext.DB.Masters.tblRoleClaim", b =>
                {
                    b.Property<int>("RoleClaimId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<int>("DocumentMaster");

                    b.Property<bool>("IsDeleted");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<byte>("PermissionType");

                    b.Property<int?>("RoleId");

                    b.HasKey("RoleClaimId");

                    b.HasIndex("RoleId");

                    b.ToTable("tblRoleClaim");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblRoleMaster", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("IsActive");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("RoleName")
                        .HasMaxLength(64);

                    b.HasKey("RoleId");

                    b.ToTable("tblRoleMaster");
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

            modelBuilder.Entity("projContext.DB.Masters.tblUserAllClaim", b =>
                {
                    b.Property<int>("RoleClaimId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DocumentMaster");

                    b.Property<bool>("IsDeleted");

                    b.Property<byte>("PermissionType");

                    b.Property<ulong?>("UserId");

                    b.HasKey("RoleClaimId");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserAllClaim");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserAllLocationPermission", b =>
                {
                    b.Property<ulong>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LocationId");

                    b.Property<ulong?>("UserId");

                    b.HasKey("Sno");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserAllLocationPermission");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserClaim", b =>
                {
                    b.Property<int>("RoleClaimId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<int>("DocumentMaster");

                    b.Property<bool>("IsDeleted");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<byte>("PermissionType");

                    b.Property<ulong?>("UserId");

                    b.HasKey("RoleClaimId");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserClaim");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserCompanyPermission", b =>
                {
                    b.Property<ulong>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CompanyId");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("HaveZoneCompanyAccess");

                    b.Property<bool>("IsDeleted");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<ulong?>("UserId");

                    b.HasKey("Sno");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserCompanyPermission");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserLocationPermission", b =>
                {
                    b.Property<ulong>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("LocationId");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<ulong?>("UserId");

                    b.HasKey("Sno");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserLocationPermission");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserLoginLog", b =>
                {
                    b.Property<ulong>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DeviceDetails")
                        .HasMaxLength(256);

                    b.Property<string>("FromLocation")
                        .HasMaxLength(128);

                    b.Property<string>("IPAddress")
                        .HasMaxLength(256);

                    b.Property<string>("Latitude")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LoginDateTime");

                    b.Property<bool>("LoginStatus")
                        .HasMaxLength(128);

                    b.Property<string>("Longitude")
                        .HasMaxLength(128);

                    b.Property<ulong?>("UserId");

                    b.HasKey("Sno");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserLoginLog");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserOTP", b =>
                {
                    b.Property<string>("SecurityStamp")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256);

                    b.Property<string>("DescId");

                    b.Property<DateTime>("EffectiveFromDt");

                    b.Property<DateTime>("EffectiveToDt");

                    b.Property<string>("OTP");

                    b.Property<string>("TempUserId")
                        .HasMaxLength(256);

                    b.Property<ulong?>("UserId");

                    b.HasKey("SecurityStamp");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserOTP");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserOrganisationPermission", b =>
                {
                    b.Property<ulong>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("HaveAllCompanyAccess");

                    b.Property<bool>("IsDeleted");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<int?>("OrgId");

                    b.Property<ulong?>("UserId");

                    b.HasKey("Sno");

                    b.HasIndex("OrgId");

                    b.HasIndex("UserId");

                    b.ToTable("tblUserOrganisationPermission");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserZonePermission", b =>
                {
                    b.Property<ulong>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<bool>("HaveAllLocationAccess");

                    b.Property<bool>("IsDeleted");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<ulong?>("UserId");

                    b.Property<int?>("ZoneId");

                    b.HasKey("Sno");

                    b.HasIndex("UserId");

                    b.HasIndex("ZoneId");

                    b.ToTable("tblUserZonePermission");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUsersMaster", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<ulong>("Id");

                    b.Property<bool>("IsActive");

                    b.Property<byte>("LoginFailCount");

                    b.Property<DateTime>("LoginFailCountdt");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<string>("Password")
                        .HasMaxLength(512);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(16);

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("UserName")
                        .HasMaxLength(64);

                    b.Property<int>("UserType");

                    b.Property<byte>("is_logged_blocked");

                    b.Property<DateTime>("logged_blocked_Enddt");

                    b.Property<DateTime>("logged_blocked_dt");

                    b.HasKey("UserId");

                    b.ToTable("tblUsersMaster");
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

                    b.Property<int?>("CompanyId");

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

                    b.HasIndex("CompanyId");

                    b.ToTable("tblZoneMaster");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblCompanyMaster", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblOrganisation", "tblOrganisation")
                        .WithMany()
                        .HasForeignKey("OrgId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblLocationMaster", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblZoneMaster", "tblZoneMaster")
                        .WithMany()
                        .HasForeignKey("ZoneId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblRoleClaim", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblRoleMaster", "tblRoleMaster")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblState", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblCountry", "tblCountry")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserAllClaim", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserAllLocationPermission", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblLocationMaster", "tblLocationMaster")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserClaim", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserCompanyPermission", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblCompanyMaster", "tblCompanyMaster")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserLocationPermission", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblLocationMaster", "tblLocationMaster")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserLoginLog", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserOTP", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserOrganisationPermission", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblOrganisation", "tblOrganisation")
                        .WithMany()
                        .HasForeignKey("OrgId");

                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblUserZonePermission", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblUsersMaster", "tblUsersMaster")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("projContext.DB.Masters.tblZoneMaster", "tblZoneMaster")
                        .WithMany()
                        .HasForeignKey("ZoneId");
                });

            modelBuilder.Entity("projContext.DB.Masters.tblZoneMaster", b =>
                {
                    b.HasOne("projContext.DB.Masters.tblCompanyMaster", "tblCompanyMaster")
                        .WithMany()
                        .HasForeignKey("CompanyId");
                });
#pragma warning restore 612, 618
        }
    }
}
