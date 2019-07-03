﻿// <auto-generated />
using System;
using IOTGW_Admin_Panel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IOTGW_Admin_Panel.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("IOTGW_Admin_Panel.Models.Gateway", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int?>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Gateways");
                });

            modelBuilder.Entity("IOTGW_Admin_Panel.Models.Node", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Config")
                        .IsRequired();

                    b.Property<string>("Description")
                        .HasMaxLength(100);

                    b.Property<int?>("GatewayID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.HasIndex("GatewayID");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("IOTGW_Admin_Panel.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("CompanyName");

                    b.Property<string>("Country");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<DateTime>("EnrollmentDate");

                    b.Property<string>("FirstName")
                        .HasMaxLength(20);

                    b.Property<string>("LastName")
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("PostalCode");

                    b.Property<int>("Roll");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("IOTGW_Admin_Panel.Models.Gateway", b =>
                {
                    b.HasOne("IOTGW_Admin_Panel.Models.User")
                        .WithMany("Gateways")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("IOTGW_Admin_Panel.Models.Node", b =>
                {
                    b.HasOne("IOTGW_Admin_Panel.Models.Gateway")
                        .WithMany("Nodes")
                        .HasForeignKey("GatewayID");
                });
#pragma warning restore 612, 618
        }
    }
}