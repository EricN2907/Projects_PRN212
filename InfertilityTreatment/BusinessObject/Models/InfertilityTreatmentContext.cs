using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Models;

public partial class InfertilityTreatmentContext : DbContext
{
    public InfertilityTreatmentContext()
    {
    }

    public InfertilityTreatmentContext(DbContextOptions<InfertilityTreatmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<PatientRequest> PatientRequests { get; set; }

    public virtual DbSet<RoleType> RoleTypes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<TreatmentService> TreatmentServices { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // an toàn hơn Directory.GetCurrentDirectory()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnectionStringDB");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnectionStringDB' not found in appsettings.json.");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TreatmentService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Treatmen__C51BB00A7465B127");
            entity.ToTable("TreatmentService");

            entity.Property(e => e.ServiceName).HasMaxLength(255);
            entity.Property(e => e.Description); // nvarchar(max) theo convention
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.TreatmentServices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Treatment__UserI__3C69FB99");
        });
        //modelBuilder.Entity<Appointment>(entity =>
        //{
        //    entity.ToTable("Appointment"); 

        //    entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC2A5347D7B");

        //    entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
        //    entity.Property(e => e.Status).HasMaxLength(50);

        //    entity.HasOne(d => d.Customer).WithMany(p => p.AppointmentCustomers)
        //        .HasForeignKey(d => d.CustomerId)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK__Appointme__Custo__3F466844");

        //    entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentDoctors)
        //        .HasForeignKey(d => d.DoctorId)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK__Appointme__Docto__403A8C7D");

        //    entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
        //        .HasForeignKey(d => d.ServiceId)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK__Appointme__Servi__412EB0B6");
        //});
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointment");              // ép đúng tên bảng (singular)
            entity.HasKey(e => e.AppointmentId).HasName("PK_Appointment");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime"); // DateTime (NOT NULL ở SQL)
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.RejectReason).HasColumnType("nvarchar(max)");

            entity.Property(e => e.RejectReason)
                  .HasColumnType("NVARCHAR(MAX)") // Định nghĩa RejectReason là kiểu NVARCHAR(MAX)
                  .IsRequired(false);

            entity.Property(e => e.CancelReason)
            .HasColumnType("NVARCHAR(MAX)") // Chọn NVARCHAR(MAX) cho CancelReason
            .IsRequired(false);  // Không bắt buộc phải có giá trị

            // Không cần ánh xạ CanCancel vì đó là thuộc tính tính toán
            entity.Ignore(e => e.CanCancel); // Dùng Ignore để chỉ rõ rằng thuộc tính này không cần ánh xạ vào bảng

            entity.HasOne(d => d.Customer).WithMany(p => p.AppointmentCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Customer");

            entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Doctor");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Service");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__BLog__54379E30794D722B");

            entity.ToTable("BLog");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BLog__UserId__4BAC3F29");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD6E639CEC0");
            entity.ToTable("Feedback");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Rating)
                  .HasDefaultValue(0); // int NOT NULL default 0

            entity.Property(e => e.Comment)
                  .IsRequired()                       // NOT NULL
                  .HasColumnType("nvarchar(max)")     // khớp SQL
                  .HasDefaultValue(string.Empty);     // DEFAULT N''

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Custom__48CFD27E");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__MedicalR__FBDF78E9DD9C7A98");

            entity.ToTable("MedicalRecord");

            entity.Property(e => e.Note).HasColumnName("Notes");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicalRe__Appoi__440B1D61");

            entity.HasOne(d => d.Customer).WithMany(p => p.MedicalRecordCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicalRe__Custo__45F365D3");

            entity.HasOne(d => d.Doctor).WithMany(p => p.MedicalRecordDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicalRe__Docto__44FF419A");
        });

        modelBuilder.Entity<PatientRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__PatientR__33A8517A622E3448");

            entity.ToTable("PatientRequest");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestedDate).HasColumnType("datetime");

            entity.Property(e => e.Note)
                  .HasColumnName("Notes")
                  .HasColumnType("nvarchar(max)");

            entity.HasOne(d => d.Customer).WithMany(p => p.PatientRequestCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PatientRe__Custo__52593CB8");

            entity.HasOne(d => d.Doctor).WithMany(p => p.PatientRequestDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PatientRe__Docto__534D60F1");

            entity.HasOne(d => d.Service).WithMany(p => p.PatientRequests)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PatientRe__Servi__5441852A");
        });

        modelBuilder.Entity<RoleType>(entity =>
        {
            entity.HasKey(e => e.Role).HasName("PK__RoleType__DA15413FC806A252");

            entity.ToTable("RoleType");

            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.ToTable("Schedule");
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B491DDF175A");

            entity.Property(e => e.ScheduleDate).HasColumnType("datetime");
            entity.Property(e => e.SerivceName).HasMaxLength(255);

            entity.HasOne(d => d.Customer).WithMany(p => p.ScheduleCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedules__Custo__4E88ABD4");

            entity.HasOne(d => d.Doctor).WithMany(p => p.ScheduleDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedules__Docto__4F7CD00D");
        });

        modelBuilder.Entity<TreatmentService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Treatmen__C51BB00A7465B127");
            entity.ToTable("TreatmentService");

            entity.Property(e => e.ServiceName).HasMaxLength(255);
            entity.Property(e => e.Description);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.TreatmentServices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Treatment__UserI__3C69FB99");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C34DF8560");

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.StartActiveDate).HasColumnType("datetime");
            // Age: int? theo convention là INT NULL nên không cần set thêm

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__398D8EEE");
        });

     
       
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
