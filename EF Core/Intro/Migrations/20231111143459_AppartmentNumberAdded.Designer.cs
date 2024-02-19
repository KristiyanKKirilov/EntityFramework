﻿// <auto-generated />
using System;
using Intro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Intro.Migrations
{
    [DbContext(typeof(SoftUniContext))]
    [Migration("20231111143459_AppartmentNumberAdded")]
    partial class AppartmentNumberAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeProject", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId", "ProjectId");

                    b.ToTable("EmployeeProject");
                });

            modelBuilder.Entity("EmployeesProject", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("EmployeeID");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectID");

                    b.HasKey("EmployeeId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("EmployeesProjects", (string)null);
                });

            modelBuilder.Entity("Intro.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AddressID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("AddressText")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("AppartmentNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("AppartmentNumber");

                    b.Property<int?>("TownId")
                        .HasColumnType("int")
                        .HasColumnName("TownID");

                    b.HasKey("AddressId");

                    b.HasIndex("TownId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Intro.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<int>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("ManagerID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("DepartmentId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Intro.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EmployeeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("AddressID");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FullEmailAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Full Email Address");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("ManagerID");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money");

                    b.HasKey("EmployeeId");

                    b.HasIndex("AddressId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Intro.Models.NewTable", b =>
                {
                    b.Property<int?>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("AddressID");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EmployeeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FullEmailAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Full Email Address");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("ManagerID");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money");

                    b.ToTable("NewTable");
                });

            modelBuilder.Entity("Intro.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProjectID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"));

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("smalldatetime");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Intro.Models.Town", b =>
                {
                    b.Property<int>("TownId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TownID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TownId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("TownId");

                    b.ToTable("Towns");
                });

            modelBuilder.Entity("Intro.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id")
                        .HasName("PK__Users__3214EC07E17F0010");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Intro.Models.VEmployeeNameJobTitle", b =>
                {
                    b.Property<string>("FullName")
                        .HasMaxLength(152)
                        .IsUnicode(false)
                        .HasColumnType("varchar(152)")
                        .HasColumnName("Full Name");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Job Title");

                    b.ToTable((string)null);

                    b.ToView("V_EmployeeNameJobTitle", (string)null);
                });

            modelBuilder.Entity("Intro.Models.VEmployeesHiredAfter2000", b =>
                {
                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.ToTable((string)null);

                    b.ToView("V_EmployeesHiredAfter2000", (string)null);
                });

            modelBuilder.Entity("Intro.Models.VEmployeesHiredAfter20001", b =>
                {
                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.ToTable((string)null);

                    b.ToView("V_EmployeesHiredAfter20001", (string)null);
                });

            modelBuilder.Entity("Intro.Models.VEmployeesHiredAfter20002", b =>
                {
                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.ToTable((string)null);

                    b.ToView("V_EmployeesHiredAfter20002", (string)null);
                });

            modelBuilder.Entity("Intro.Models.VEmployeesSalary", b =>
                {
                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money");

                    b.ToTable((string)null);

                    b.ToView("V_EmployeesSalaries", (string)null);
                });

            modelBuilder.Entity("EmployeesProject", b =>
                {
                    b.HasOne("Intro.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_EmployeesProjects_Employees");

                    b.HasOne("Intro.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .IsRequired()
                        .HasConstraintName("FK_EmployeesProjects_Projects");
                });

            modelBuilder.Entity("Intro.Models.Address", b =>
                {
                    b.HasOne("Intro.Models.Town", "Town")
                        .WithMany("Addresses")
                        .HasForeignKey("TownId")
                        .HasConstraintName("FK_Addresses_Towns");

                    b.Navigation("Town");
                });

            modelBuilder.Entity("Intro.Models.Department", b =>
                {
                    b.HasOne("Intro.Models.Employee", "Manager")
                        .WithMany("Departments")
                        .HasForeignKey("ManagerId")
                        .IsRequired()
                        .HasConstraintName("FK_Departments_Employees");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Intro.Models.Employee", b =>
                {
                    b.HasOne("Intro.Models.Address", "Address")
                        .WithMany("Employees")
                        .HasForeignKey("AddressId")
                        .HasConstraintName("FK_Employees_Addresses");

                    b.HasOne("Intro.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .IsRequired()
                        .HasConstraintName("FK_Employees_Departments");

                    b.HasOne("Intro.Models.Employee", "Manager")
                        .WithMany("InverseManager")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK_Employees_Employees");

                    b.Navigation("Address");

                    b.Navigation("Department");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Intro.Models.Address", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Intro.Models.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Intro.Models.Employee", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("InverseManager");
                });

            modelBuilder.Entity("Intro.Models.Town", b =>
                {
                    b.Navigation("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}