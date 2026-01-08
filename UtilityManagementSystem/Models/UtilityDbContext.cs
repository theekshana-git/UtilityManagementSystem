using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace UtilityManagementSystem.Models;

public partial class UtilityDbContext : IdentityDbContext<IdentityUser>
{
    public UtilityDbContext()
    {
    }

    public UtilityDbContext(DbContextOptions<UtilityDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Meter> Meters { get; set; }

    public virtual DbSet<MeterReading> MeterReadings { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Tariff> Tariffs { get; set; }

    public virtual DbSet<UtilityTariffHistory> UtilityTariffHistories { get; set; }

    public virtual DbSet<UtilityType> UtilityTypes { get; set; }

    public virtual DbSet<VwComplaintsSummary> VwComplaintsSummaries { get; set; }

    public virtual DbSet<VwMonthlyRevenue> VwMonthlyRevenues { get; set; }

    public virtual DbSet<VwTopConsumer> VwTopConsumers { get; set; }

    public virtual DbSet<VwUnpaidBill> VwUnpaidBills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NID96F5\\SQLEXPRESS;Database=UtilityManagementSystem;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.ToTable("Bill");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.BillDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ReadingId).HasColumnName("ReadingID");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Unpaid");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_Customer");

            entity.HasOne(d => d.GeneratedByNavigation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.GeneratedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_Employee");

            entity.HasOne(d => d.Reading).WithMany(p => p.Bills)
                .HasForeignKey(d => d.ReadingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_MeterReading");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.ToTable("Complaint");

            entity.Property(e => e.ComplaintId).HasColumnName("ComplaintID");
            entity.Property(e => e.ComplaintDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ResolutionNote)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Complaint_Customer");

            entity.HasOne(d => d.HandledByNavigation).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.HandledBy)
                .HasConstraintName("FK_Complaint_Employee");

            entity.HasOne(d => d.Utility).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.UtilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Complaint_UtilityType");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasIndex(e => e.ContactNumber, "UQ_Customer_ContactNumber").IsUnique();

            entity.HasIndex(e => e.Email, "UQ_Customer_Email").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CustomerType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasIndex(e => e.Email, "UQ_Employee_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_Employee_Username").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HireDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Meter>(entity =>
        {
            entity.ToTable("Meter");

            entity.Property(e => e.MeterId).HasColumnName("MeterID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Meters)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Meter_Customer");

            entity.HasOne(d => d.Utility).WithMany(p => p.Meters)
                .HasForeignKey(d => d.UtilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Meter_UtilityType");
        });

        modelBuilder.Entity<MeterReading>(entity =>
        {
            entity.HasKey(e => e.ReadingId);

            entity.ToTable("MeterReading", tb => tb.HasTrigger("TR_SetPreviousReading"));

            entity.Property(e => e.ReadingId).HasColumnName("ReadingID");
            entity.Property(e => e.Consumption)
                .HasComputedColumnSql("([CurrentReading]-[PreviousReading])", true)
                .HasColumnType("decimal(11, 2)");
            entity.Property(e => e.CurrentReading).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MeterId).HasColumnName("MeterID");
            entity.Property(e => e.Notes)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PreviousReading)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Meter).WithMany(p => p.MeterReadings)
                .HasForeignKey(d => d.MeterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeterReading_Meter");

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.MeterReadings)
                .HasForeignKey(d => d.RecordedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeterReading_Employee");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment", tb => tb.HasTrigger("TR_UpdateOutstandingBalance"));

            entity.HasIndex(e => e.ReferenceNumber, "UQ_Payment_ReferenceNumber").IsUnique();

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Bill).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Bill");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Customer");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ProcessedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Employee");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.FilePath)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GeneratedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReportType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Summary)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.GeneratedByNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.GeneratedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Employee");
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.ToTable("Tariff", tb => tb.HasTrigger("TR_CloseTariff"));

            entity.Property(e => e.TariffId).HasColumnName("TariffID");
            entity.Property(e => e.FixedCharge)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RatePerUnit).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SlabEnd).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SlabStart).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");

            entity.HasOne(d => d.Utility).WithMany(p => p.Tariffs)
                .HasForeignKey(d => d.UtilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tariff_UtilityType");
        });

        modelBuilder.Entity<UtilityTariffHistory>(entity =>
        {
            entity.HasKey(e => e.RecordId);

            entity.ToTable("UtilityTariffHistory");

            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.ChangedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TariffId).HasColumnName("TariffID");
            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.UtilityTariffHistories)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UTH_Employee");

            entity.HasOne(d => d.Tariff).WithMany(p => p.UtilityTariffHistories)
                .HasForeignKey(d => d.TariffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UTH_Tariff");

            entity.HasOne(d => d.Utility).WithMany(p => p.UtilityTariffHistories)
                .HasForeignKey(d => d.UtilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UTH_UtilityType");
        });

        modelBuilder.Entity<UtilityType>(entity =>
        {
            entity.HasKey(e => e.UtilityId);

            entity.ToTable("UtilityType");

            entity.HasIndex(e => e.UtilityName, "UQ_UtilityType_UtilityName").IsUnique();

            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UtilityName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwComplaintsSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ComplaintsSummary");

            entity.Property(e => e.ComplaintId).HasColumnName("ComplaintID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");
        });

        modelBuilder.Entity<VwMonthlyRevenue>(entity => {
            entity.HasNoKey();
            entity.ToView("vw_MonthlyRevenue");
        });

        modelBuilder.Entity<VwTopConsumer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TopConsumers");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.TotalConsumption).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VwUnpaidBill>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UnpaidBills");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
