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
    public DbSet<GroupOnClass> GroupOnClass { get; set; }
    public DbSet<ScheduleLoad> ScheduleLoads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupOnClass>()
            .HasKey(gc => new { gc.ScheduleId, gc.GroupId });

        modelBuilder.Entity<GroupOnClass>()
            .HasOne(gc => gc.Lesson)
            .WithMany(s => s.GroupsOnClasses)
            .HasForeignKey(gc => gc.ScheduleId);

        modelBuilder.Entity<GroupOnClass>()
            .HasOne(gc => gc.Group)
            .WithMany(g => g.GroupsOnClasses)
            .HasForeignKey(gc => gc.GroupId);
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