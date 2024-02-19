using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Intro.Models;

public partial class SoftUniContext : DbContext
{
    public SoftUniContext()
    {
    }

    public SoftUniContext(DbContextOptions<SoftUniContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<NewTable> NewTables { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Town> Towns { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VEmployeeNameJobTitle> VEmployeeNameJobTitles { get; set; }

    public virtual DbSet<VEmployeesHiredAfter2000> VEmployeesHiredAfter2000s { get; set; }

    public virtual DbSet<VEmployeesHiredAfter20001> VEmployeesHiredAfter20001s { get; set; }

    public virtual DbSet<VEmployeesHiredAfter20002> VEmployeesHiredAfter20002s { get; set; }

    public virtual DbSet<VEmployeesSalary> VEmployeesSalaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-M5SEPFK\\SQLEXPRESS;Database=SoftUni;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasOne(d => d.Town).WithMany(p => p.Addresses).HasConstraintName("FK_Addresses_Towns");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasOne(d => d.Manager).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departments_Employees");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.Address).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Addresses");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Departments");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager).HasConstraintName("FK_Employees_Employees");

            entity.HasMany(d => d.Projects).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeesProject",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeesProjects_Projects"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeesProjects_Employees"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "ProjectId");
                        j.ToTable("EmployeesProjects");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");
                        j.IndexerProperty<int>("ProjectId").HasColumnName("ProjectID");
                    });
        });

        modelBuilder.Entity<NewTable>(entity =>
        {
            entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E17F0010");
        });

        modelBuilder.Entity<VEmployeeNameJobTitle>(entity =>
        {
            entity.ToView("V_EmployeeNameJobTitle");
        });

        modelBuilder.Entity<VEmployeesHiredAfter2000>(entity =>
        {
            entity.ToView("V_EmployeesHiredAfter2000");
        });

        modelBuilder.Entity<VEmployeesHiredAfter20001>(entity =>
        {
            entity.ToView("V_EmployeesHiredAfter20001");
        });

        modelBuilder.Entity<VEmployeesHiredAfter20002>(entity =>
        {
            entity.ToView("V_EmployeesHiredAfter20002");
        });

        modelBuilder.Entity<VEmployeesSalary>(entity =>
        {
            entity.ToView("V_EmployeesSalaries");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
