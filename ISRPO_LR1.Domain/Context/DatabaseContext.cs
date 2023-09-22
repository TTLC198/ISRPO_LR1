using Microsoft.EntityFrameworkCore;

namespace ISRPO_LR1.Domain.Context;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.g_id).HasName("Grade_pk");

            entity.ToTable("Grade");

            entity.HasOne(d => d.g_s).WithMany(p => p.Grades)
                .HasForeignKey(d => d.g_s_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Grade_Student_s_id_fk");

            entity.HasOne(d => d.g_sj).WithMany(p => p.Grades)
                .HasForeignKey(d => d.g_sj_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Grade_Subject_sj_id_fk");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.s_id).HasName("Student_pk");

            entity.ToTable("Student");

            entity.Property(e => e.s_birth_date).HasColumnType("date");
            entity.Property(e => e.s_email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.s_full_name).HasMaxLength(200);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.sj_id).HasName("Subject_pk");

            entity.ToTable("Subject");

            entity.Property(e => e.sj_name).HasMaxLength(70);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
