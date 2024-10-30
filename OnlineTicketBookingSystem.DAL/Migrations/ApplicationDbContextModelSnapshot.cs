﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineTicketBookingSystem.DAL.Data;

#nullable disable

namespace OnlineTicketBookingSystem.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.AdministrativeRegion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AdministrativeRegions");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.AdministrativeUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AdministrativeUnits");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Buses", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BusType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmptySeats")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicensePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int?>("TotalSeats")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Buses");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.District", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("AdministrativeUnitId")
                        .HasColumnType("int");

                    b.Property<string>("CodeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceCode")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Code");

                    b.HasIndex("AdministrativeUnitId");

                    b.HasIndex("ProvinceCode");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameVI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.GroupRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoleId");

                    b.ToTable("GroupRoles");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Province", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AdministrativeRegionId")
                        .HasColumnType("int");

                    b.Property<int?>("AdministrativeUnitId")
                        .HasColumnType("int");

                    b.Property<string>("CodeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.HasIndex("AdministrativeRegionId");

                    b.HasIndex("AdministrativeUnitId");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Seats", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SeatNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Tickets", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("SeatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeatId");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.TripAssignments", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DriverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("TripId");

                    b.ToTable("TripAssignments");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Trips", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartureTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Distance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndPoint")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EstimatedArrivalTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StartPoint")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BusId");

                    b.HasIndex("EndPoint");

                    b.HasIndex("StartPoint");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CodeExpired")
                        .HasColumnType("datetime2");

                    b.Property<string>("CodeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("DistrictCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStatus")
                        .HasColumnType("bit");

                    b.Property<string>("LastLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WardCode")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DistrictCode");

                    b.HasIndex("GroupId");

                    b.HasIndex("ProvinceCode");

                    b.HasIndex("WardCode");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Ward", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("AdministrativeUnitId")
                        .HasColumnType("int");

                    b.Property<string>("CodeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DistrictCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.HasIndex("AdministrativeUnitId");

                    b.HasIndex("DistrictCode");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.District", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.AdministrativeUnit", "AdministrativeUnit")
                        .WithMany()
                        .HasForeignKey("AdministrativeUnitId");

                    b.HasOne("OnlineTicketBookingSystem.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceCode");

                    b.Navigation("AdministrativeUnit");

                    b.Navigation("Province");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.GroupRole", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineTicketBookingSystem.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Province", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.AdministrativeRegion", "AdministrativeRegion")
                        .WithMany()
                        .HasForeignKey("AdministrativeRegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineTicketBookingSystem.Models.AdministrativeUnit", "AdministrativeUnit")
                        .WithMany()
                        .HasForeignKey("AdministrativeUnitId");

                    b.Navigation("AdministrativeRegion");

                    b.Navigation("AdministrativeUnit");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Seats", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.Buses", "Buses")
                        .WithMany()
                        .HasForeignKey("BusId");

                    b.Navigation("Buses");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Tickets", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.Seats", "Seats")
                        .WithMany()
                        .HasForeignKey("SeatId");

                    b.HasOne("OnlineTicketBookingSystem.Models.Trips", "Trips")
                        .WithMany()
                        .HasForeignKey("TripId");

                    b.HasOne("OnlineTicketBookingSystem.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Seats");

                    b.Navigation("Trips");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.TripAssignments", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.User", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId");

                    b.HasOne("OnlineTicketBookingSystem.Models.Trips", "Trips")
                        .WithMany()
                        .HasForeignKey("TripId");

                    b.Navigation("Driver");

                    b.Navigation("Trips");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Trips", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.Buses", "Buses")
                        .WithMany()
                        .HasForeignKey("BusId");

                    b.HasOne("OnlineTicketBookingSystem.Models.Province", "EndProvince")
                        .WithMany()
                        .HasForeignKey("EndPoint");

                    b.HasOne("OnlineTicketBookingSystem.Models.Province", "StartProvince")
                        .WithMany()
                        .HasForeignKey("StartPoint");

                    b.Navigation("Buses");

                    b.Navigation("EndProvince");

                    b.Navigation("StartProvince");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.User", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictCode");

                    b.HasOne("OnlineTicketBookingSystem.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("OnlineTicketBookingSystem.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceCode");

                    b.HasOne("OnlineTicketBookingSystem.Models.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardCode");

                    b.Navigation("District");

                    b.Navigation("Group");

                    b.Navigation("Province");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("OnlineTicketBookingSystem.Models.Ward", b =>
                {
                    b.HasOne("OnlineTicketBookingSystem.Models.AdministrativeUnit", "AdministrativeUnit")
                        .WithMany()
                        .HasForeignKey("AdministrativeUnitId");

                    b.HasOne("OnlineTicketBookingSystem.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictCode");

                    b.Navigation("AdministrativeUnit");

                    b.Navigation("District");
                });
#pragma warning restore 612, 618
        }
    }
}
