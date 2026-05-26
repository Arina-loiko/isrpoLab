using LabTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace LabTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<LabWork> LabWorks => Set<LabWork>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LabWork>()
            .HasOne(l => l.Student)
            .WithMany(s => s.LabWorks)
            .HasForeignKey(l => l.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LabWork>()
            .HasOne(l => l.Subject)
            .WithMany(s => s.LabWorks)
            .HasForeignKey(l => l.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, LastName = "Лойко", FirstName = "Арина", MiddleName = "Станиславна", Group = "ИСП-232" },
            new Student { Id = 2, LastName = "Иванов", FirstName = "Пётр", MiddleName = "Сергеевич", Group = "ИСП-232" },
            new Student { Id = 3, LastName = "Сидорова", FirstName = "Мария", MiddleName = "Алексеевна", Group = "ИСП-232" }
        );

        modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = 1, Name = "Базы данных", TeacherName = "Петров А.В." },
            new Subject { Id = 2, Name = "Веб-разработка", TeacherName = "Смирнова О.И." },
            new Subject { Id = 3, Name = "ИСРПО", TeacherName = "Козлов Д.Н." }
        );

        modelBuilder.Entity<LabWork>().HasData(
            new LabWork { Id = 1, StudentId = 1, SubjectId = 1, LabNumber = 1, Title = "Проектирование БД", Status = "Сдана", Grade = 9, SubmittedDate = new DateTime(2026, 3, 10) },
            new LabWork { Id = 2, StudentId = 1, SubjectId = 2, LabNumber = 1, Title = "HTML/CSS основы", Status = "Сдана", Grade = 10, SubmittedDate = new DateTime(2026, 3, 15) },
            new LabWork { Id = 3, StudentId = 2, SubjectId = 1, LabNumber = 1, Title = "Проектирование БД", Status = "На доработке", Grade = null, SubmittedDate = null },
            new LabWork { Id = 4, StudentId = 3, SubjectId = 3, LabNumber = 1, Title = "Введение в ИСРПО", Status = "Не сдана", Grade = null, SubmittedDate = null }
        );
    }
}
