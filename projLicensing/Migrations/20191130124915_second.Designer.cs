﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projLicensing;

namespace projLicensing.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20191130124915_second")]
    partial class second
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("projLicensing.tbl_api_log", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("api_type");

                    b.Property<DateTime>("entrydate");

                    b.Property<string>("instance_id");

                    b.Property<string>("response");

                    b.HasKey("id");

                    b.ToTable("tbl_api_log");
                });

            modelBuilder.Entity("projLicensing.tbl_instance", b =>
                {
                    b.Property<int>("i_id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("api_project_url");

                    b.Property<int>("company_count");

                    b.Property<int>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<string>("db_connection");

                    b.Property<string>("design_project_url");

                    b.Property<string>("instance_id");

                    b.Property<byte>("is_active");

                    b.Property<int>("last_modified_by");

                    b.Property<DateTime>("last_modified_date");

                    b.Property<string>("organisation");

                    b.Property<string>("premises_type");

                    b.Property<string>("superadmin_password");

                    b.Property<string>("superadmin_username");

                    b.HasKey("i_id");

                    b.ToTable("tbl_instance");
                });

            modelBuilder.Entity("projLicensing.tbl_instance_details", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("company_id");

                    b.Property<int>("created_by");

                    b.Property<DateTime>("created_date");

                    b.Property<int>("employee_count");

                    b.Property<int?>("i_id");

                    b.Property<int>("instance_id");

                    b.Property<int>("is_active");

                    b.Property<int>("last_modified_by");

                    b.Property<DateTime>("last_modified_date");

                    b.HasKey("id");

                    b.HasIndex("i_id");

                    b.ToTable("tbl_instance_details");
                });

            modelBuilder.Entity("projLicensing.tbl_instance_details", b =>
                {
                    b.HasOne("projLicensing.tbl_instance", "tbl_instance")
                        .WithMany()
                        .HasForeignKey("i_id");
                });
#pragma warning restore 612, 618
        }
    }
}
