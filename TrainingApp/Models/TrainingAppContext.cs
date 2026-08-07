using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrainingApp.Models;

public partial class TrainingAppContext : DbContext
{
    public TrainingAppContext()
    {
    }

    public TrainingAppContext(DbContextOptions<TrainingAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<AllActivity> AllActivities { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<ProgramsExercise> ProgramsExercises { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activiti__45F4A7F1310322CE");

            entity.ToTable(tb => tb.HasTrigger("trigg_maxDuration"));

            entity.Property(e => e.ActivityId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ActivityID");
            entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            entity.Property(e => e.Note).HasMaxLength(300);

            entity.HasOne(d => d.Exercise).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__Activitie__Exerc__1ED998B2");
        });

        modelBuilder.Entity<AllActivity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AllActivities");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ExerciseTitle).HasMaxLength(100);
            entity.Property(e => e.Note).HasMaxLength(300);
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F3B52AD49");

            entity.ToTable(tb => tb.HasTrigger("trigg_LockExerciseDeactivate"));

            entity.Property(e => e.ExerciseId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ExerciseID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ExerciseTitle).HasMaxLength(100);
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.ProgramId).HasName("PK__Programs__75256038EE47343E");

            entity.Property(e => e.ProgramId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ProgramID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ProgramTitle).HasMaxLength(100);
            entity.Property(e => e.TypeProgram).HasMaxLength(100);
        });

        modelBuilder.Entity<ProgramsExercise>(entity =>
        {
            entity.HasKey(e => new { e.ProgramId, e.ExerciseId }).HasName("PK__Programs__9F222AE86FDE8FED");

            entity.Property(e => e.ProgramId).HasColumnName("ProgramID");
            entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");

            entity.HasOne(d => d.Exercise).WithMany(p => p.ProgramsExercises)
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProgramsE__Exerc__1920BF5C");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramsExercises)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProgramsE__Progr__182C9B23");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
