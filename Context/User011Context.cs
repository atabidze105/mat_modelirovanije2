using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using mat_modelirovanije2.Models;

namespace mat_modelirovanije2.Context;

public partial class User011Context : DbContext
{
    public User011Context()
    {
    }

    public User011Context(DbContextOptions<User011Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Absence> Absences { get; set; }

    public virtual DbSet<AbsenceType> AbsenceTypes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<EducationCalendar> EducationCalendars { get; set; }

    public virtual DbSet<EducationType> EducationTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventStatus> EventStatuses { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialStatus> MaterialStatuses { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<Workingcalendar> Workingcalendars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=89.110.53.87:5522;Database=user011;Username=user011;password=83292");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("absence_pk");

            entity.ToTable("absence", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AbsentEployee).HasColumnName("absent_eployee");
            entity.Property(e => e.DateEnd).HasColumnName("date_end");
            entity.Property(e => e.DateStart).HasColumnName("date_start");
            entity.Property(e => e.SubstituteEmployee).HasColumnName("substitute_employee");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.AbsentEployeeNavigation).WithMany(p => p.AbsenceAbsentEployeeNavigations)
                .HasForeignKey(d => d.AbsentEployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("absence_employee_fk");

            entity.HasOne(d => d.SubstituteEmployeeNavigation).WithMany(p => p.AbsenceSubstituteEmployeeNavigations)
                .HasForeignKey(d => d.SubstituteEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("absence_employee_fk_1");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Absences)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("absence_absence_type_fk");
        });

        modelBuilder.Entity<AbsenceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("absence_type_pk");

            entity.ToTable("absence_type", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pk");

            entity.ToTable("department", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DepartmentDirector).HasColumnName("department_director");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.IdMainpepartment).HasColumnName("id_mainpepartment");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.DepartmentDirectorNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.DepartmentDirector)
                .HasConstraintName("department_employee_fk");

            entity.HasOne(d => d.IdMainpepartmentNavigation).WithMany(p => p.InverseIdMainpepartmentNavigation)
                .HasForeignKey(d => d.IdMainpepartment)
                .HasConstraintName("department_department_fk");

            entity.HasMany(d => d.LowerDeps).WithMany(p => p.MainDeps)
                .UsingEntity<Dictionary<string, object>>(
                    "LowerDepartment",
                    r => r.HasOne<Department>().WithMany()
                        .HasForeignKey("LowerDepId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("lower_department_department_fk_1"),
                    l => l.HasOne<Department>().WithMany()
                        .HasForeignKey("MainDepId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("lower_department_department_fk"),
                    j =>
                    {
                        j.HasKey("MainDepId", "LowerDepId").HasName("lower_department_pk");
                        j.ToTable("lower_department", "mat_modelirovanije");
                        j.IndexerProperty<int>("MainDepId").HasColumnName("main_dep_id");
                        j.IndexerProperty<int>("LowerDepId").HasColumnName("lower_dep_id");
                    });

            entity.HasMany(d => d.MainDeps).WithMany(p => p.LowerDeps)
                .UsingEntity<Dictionary<string, object>>(
                    "LowerDepartment",
                    r => r.HasOne<Department>().WithMany()
                        .HasForeignKey("MainDepId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("lower_department_department_fk"),
                    l => l.HasOne<Department>().WithMany()
                        .HasForeignKey("LowerDepId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("lower_department_department_fk_1"),
                    j =>
                    {
                        j.HasKey("MainDepId", "LowerDepId").HasName("lower_department_pk");
                        j.ToTable("lower_department", "mat_modelirovanije");
                        j.IndexerProperty<int>("MainDepId").HasColumnName("main_dep_id");
                        j.IndexerProperty<int>("LowerDepId").HasColumnName("lower_dep_id");
                    });
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("education_pk");

            entity.ToTable("education", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Educations)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("education_education_type_fk");

            entity.HasMany(d => d.Dates).WithMany(p => p.Educations)
                .UsingEntity<Dictionary<string, object>>(
                    "EducationDate",
                    r => r.HasOne<EducationCalendar>().WithMany()
                        .HasForeignKey("DateId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("education_date_education_calendar_fk"),
                    l => l.HasOne<Education>().WithMany()
                        .HasForeignKey("EducationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("education_date_education_fk"),
                    j =>
                    {
                        j.HasKey("EducationId", "DateId").HasName("education_date_pk");
                        j.ToTable("education_date", "mat_modelirovanije");
                        j.IndexerProperty<int>("EducationId").HasColumnName("education_id");
                        j.IndexerProperty<int>("DateId").HasColumnName("date_id");
                    });

            entity.HasMany(d => d.Materials).WithMany(p => p.Educations)
                .UsingEntity<Dictionary<string, object>>(
                    "EducationMaterial",
                    r => r.HasOne<Material>().WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("education_material_material_fk"),
                    l => l.HasOne<Education>().WithMany()
                        .HasForeignKey("EducationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("education_material_education_fk"),
                    j =>
                    {
                        j.HasKey("EducationId", "MaterialId").HasName("education_material_pk");
                        j.ToTable("education_material", "mat_modelirovanije");
                        j.IndexerProperty<int>("EducationId").HasColumnName("education_id");
                        j.IndexerProperty<int>("MaterialId").HasColumnName("material_id");
                    });
        });

        modelBuilder.Entity<EducationCalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("education_calendar_pk");

            entity.ToTable("education_calendar", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
        });

        modelBuilder.Entity<EducationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("education_type_pk");

            entity.ToTable("education_type", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pk");

            entity.ToTable("employee", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Assistant).HasColumnName("assistant");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.Lastname)
                .HasColumnType("character varying")
                .HasColumnName("lastname");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasColumnType("character varying")
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.PhonePrivate)
                .HasColumnType("character varying")
                .HasColumnName("phone_private");
            entity.Property(e => e.Room)
                .HasColumnType("character varying")
                .HasColumnName("room");
            entity.Property(e => e.Supervisor).HasColumnName("supervisor");

            entity.HasOne(d => d.AssistantNavigation).WithMany(p => p.InverseAssistantNavigation)
                .HasForeignKey(d => d.Assistant)
                .HasConstraintName("employee_employee_fk_1");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_department_fk");

            entity.HasOne(d => d.Job).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_job_fk");

            entity.HasOne(d => d.SupervisorNavigation).WithMany(p => p.InverseSupervisorNavigation)
                .HasForeignKey(d => d.Supervisor)
                .HasConstraintName("employee_employee_fk");

            entity.HasMany(d => d.Educations).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeEducation",
                    r => r.HasOne<Education>().WithMany()
                        .HasForeignKey("EducationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("employee_education_education_fk"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("employee_education_employee_fk"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "EducationId").HasName("employee_education_pk");
                        j.ToTable("employee_education", "mat_modelirovanije");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
                        j.IndexerProperty<int>("EducationId").HasColumnName("education_id");
                    });

            entity.HasMany(d => d.IdWorkingcalendars).WithMany(p => p.IdEmployees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeWorkingcalendar",
                    r => r.HasOne<Workingcalendar>().WithMany()
                        .HasForeignKey("IdWorkingcalendar")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("employee_workingcalendar_workingcalendar_fk"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("employee_workingcalendar_employee_fk"),
                    j =>
                    {
                        j.HasKey("IdEmployee", "IdWorkingcalendar").HasName("employee_workingcalendar_pk");
                        j.ToTable("employee_workingcalendar", "mat_modelirovanije");
                        j.IndexerProperty<int>("IdEmployee").HasColumnName("id_employee");
                        j.IndexerProperty<int>("IdWorkingcalendar").HasColumnName("id_workingcalendar");
                    });
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_pk");

            entity.ToTable("event", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DatetimeEnd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datetime_end");
            entity.Property(e => e.DatetimeStart)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datetime_start");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.IdOrganisator).HasColumnName("id_organisator");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdOrganisatorNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.IdOrganisator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("event_employee_fk");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("event_event_status_fk");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.IdType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("event_event_type_fk");
        });

        modelBuilder.Entity<EventStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_status_pk");

            entity.ToTable("event_status", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_type_pk");

            entity.ToTable("event_type", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_pk");

            entity.ToTable("job", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_pk");

            entity.ToTable("material", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.DateApproval).HasColumnName("date_approval");
            entity.Property(e => e.DateEdited).HasColumnName("date_edited");
            entity.Property(e => e.Domain)
                .HasColumnType("character varying")
                .HasColumnName("domain");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Materials)
                .HasForeignKey(d => d.Author)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("material_employee_fk");

            entity.HasOne(d => d.Status).WithMany(p => p.Materials)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("material_material_status_fk");

            entity.HasOne(d => d.Type).WithMany(p => p.Materials)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("material_material_type_fk");
        });

        modelBuilder.Entity<MaterialStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_status_pk");

            entity.ToTable("material_status", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_type_pk");

            entity.ToTable("material_type", "mat_modelirovanije");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Workingcalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workingcalendar_pk");

            entity.ToTable("workingcalendar", "mat_modelirovanije", tb => tb.HasComment("Список дней исключений в производственном календаре"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Exceptiondate)
                .HasComment("День-исключение")
                .HasColumnName("exceptiondate");
            entity.Property(e => e.Isworkingday)
                .HasComment("0 - будний день, но законодательно принят выходным; 1 - сб или вс, но является рабочим")
                .HasColumnName("isworkingday");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
