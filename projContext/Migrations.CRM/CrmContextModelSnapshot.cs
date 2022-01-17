﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projContext.DB.CRM;

namespace projContext.Migrations.CRM
{
    [DbContext(typeof(CrmContext))]
    partial class CrmContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("projContext.DB.CRM.tblCustomerIPFilter", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowedAllIp");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.HasKey("CustomerId");

                    b.ToTable("tblCustomerIPFilter");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblCustomerIPFilterDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CustomerId");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("tblCustomerIPFilterDetails");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblCustomerMarkup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookingType");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("EffectiveFromDt");

                    b.Property<DateTime>("EffectiveToDt");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("MarkupAmount");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<int>("Nid");

                    b.HasKey("Id");

                    b.ToTable("tblCustomerMarkup");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblCustomerMaster", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlternateContactNo")
                        .HasMaxLength(16);

                    b.Property<string>("AlternateEmail")
                        .HasMaxLength(254);

                    b.Property<string>("City")
                        .HasMaxLength(254);

                    b.Property<string>("Code")
                        .HasMaxLength(32);

                    b.Property<string>("ContactNo")
                        .HasMaxLength(16);

                    b.Property<int>("CountryId");

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<int>("CustomerType");

                    b.Property<DateTime>("EffectiveFromDt");

                    b.Property<DateTime>("EffectiveToDt");

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
                        .HasMaxLength(128);

                    b.Property<string>("OfficeAddress")
                        .HasMaxLength(254);

                    b.Property<int>("OrgId");

                    b.Property<string>("Pincode")
                        .HasMaxLength(32);

                    b.Property<int>("StateId");

                    b.HasKey("CustomerId");

                    b.ToTable("tblCustomerMaster");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblCustomerNotification", b =>
                {
                    b.Property<int>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<int>("CustomerId");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<int>("NotificationType");

                    b.Property<bool>("SendDeviceNotification");

                    b.Property<bool>("SendEmail");

                    b.Property<bool>("SendSms");

                    b.Property<ulong>("UserId");

                    b.HasKey("Sno");

                    b.ToTable("tblCustomerNotification");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblCustomerWalletAmount", b =>
                {
                    b.Property<ulong>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<int>("EmployeeId");

                    b.Property<ulong>("Nid");

                    b.Property<DateTime?>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<double>("WalletAmount");

                    b.HasKey("Sno");

                    b.ToTable("tblCustomerWalletAmount");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblPaymentRequest", b =>
                {
                    b.Property<ulong>("PaymentRequestId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApprovalRemarks")
                        .HasMaxLength(128);

                    b.Property<byte>("ApprovalStatus");

                    b.Property<ulong?>("ApprovedBy");

                    b.Property<DateTime?>("ApprovedDt");

                    b.Property<int?>("CustomerId");

                    b.Property<bool>("IsDeleted");

                    b.Property<ulong>("Nid");

                    b.Property<int>("RequestType");

                    b.Property<double>("RequestedAmt");

                    b.Property<ulong>("RequestedBy");

                    b.Property<DateTime?>("RequestedDt");

                    b.Property<string>("RequestedRemarks")
                        .HasMaxLength(128);

                    b.Property<DateTime?>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<byte>("Status");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<string>("TransactionNumber")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int>("TransactionType");

                    b.Property<string>("UploadImages");

                    b.HasKey("PaymentRequestId");

                    b.ToTable("tblPaymentRequest");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblWalletBalanceAlert", b =>
                {
                    b.Property<int>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("CreatedBy");

                    b.Property<DateTime>("CreatedDt");

                    b.Property<int>("CustomerId");

                    b.Property<double>("MinBalance");

                    b.Property<ulong?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<string>("ModifyRemarks")
                        .HasMaxLength(128);

                    b.Property<ulong>("Nid");

                    b.HasKey("Sno");

                    b.ToTable("tblWalletBalanceAlert");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblWalletDetailLedger", b =>
                {
                    b.Property<int>("Sno")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Credit");

                    b.Property<int>("CustomerId");

                    b.Property<double>("Debit");

                    b.Property<int>("EmployeeId");

                    b.Property<ulong>("Nid");

                    b.Property<ulong>("PaymentRequestId");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("TransactionDetails")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("TransactionDt");

                    b.Property<int>("TransactionType");

                    b.HasKey("Sno");

                    b.ToTable("tblWalletDetailLedger");
                });

            modelBuilder.Entity("projContext.DB.CRM.tblCustomerIPFilterDetails", b =>
                {
                    b.HasOne("projContext.DB.CRM.tblCustomerIPFilter", "tblCustomerIPFilter")
                        .WithMany("tblCustomerIPFilterDetails")
                        .HasForeignKey("CustomerId");
                });
#pragma warning restore 612, 618
        }
    }
}
