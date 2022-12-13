using Microsoft.EntityFrameworkCore;

namespace schedule.Models;

public sealed class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Semester> Semesters => Set<Semester>();

}