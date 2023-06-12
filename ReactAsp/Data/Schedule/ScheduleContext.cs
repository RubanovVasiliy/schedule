using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule;

public sealed class ScheduleContext : DbContext
{
    public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
    { 
         Database.EnsureCreated();
    }

    public DbSet<Group> Groups { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonGroup> LessonsGroups { get; set; }
    public DbSet<ScheduleLoad> ScheduleLoads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>()
            .HasIndex(e => e.GroupNumber)
            .IsUnique();
        
        modelBuilder.Entity<Teacher>()
            .HasIndex(e => e.FullName)
            .IsUnique();
        
        modelBuilder.Entity<Classroom>()
            .HasIndex(e => e.ClassroomNumber)
            .IsUnique();
        
        modelBuilder.Entity<Subject>()
            .HasIndex(e => e.SubjectName)
            .IsUnique();
    }
    
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var POSTGRES_USER = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var POSTGRES_PASSWORD = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var POSTGRES_DB = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var POSTGRES_HOST = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var POSTGRES_PORT = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        
        optionsBuilder.UseNpgsql(
            $"Host={POSTGRES_HOST};Port={POSTGRES_PORT};Database={POSTGRES_DB};Username={POSTGRES_USER};Password={POSTGRES_PASSWORD};"
        );
        Console.WriteLine($"Host={POSTGRES_HOST};Port={POSTGRES_PORT};Database={POSTGRES_DB};Username={POSTGRES_USER};Password={POSTGRES_PASSWORD}");
        Console.WriteLine("Success");
    }*/
}