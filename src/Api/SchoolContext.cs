using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api;

public sealed class SchoolContext : DbContext
{
    private readonly string _connectionString;
    private readonly bool _useConsoleLogger;
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    // public SchoolContext(DbContextOptions<SchoolContext> options)
    //     : base(options)
    // {
    // }

    // Only use params that change depending on the environment.
    public SchoolContext(string connectionString, bool useConsoleLogger)
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);

        if (_useConsoleLogger)
        {
            optionsBuilder
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableSensitiveDataLogging();
        }
        else
        {
            // EF logs SQL queries and other events by default.
            optionsBuilder
                .UseLoggerFactory(CreateEmptyLoggerFactory());
        }
    }

    private static ILoggerFactory CreateEmptyLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddFilter((_, _) => false));
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            .AddConsole()
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(x =>
        {
            x.ToTable("Student").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("StudentID");
            x.Property(p => p.Email);
            x.Property(p => p.Name);
            x.HasMany(p => p.Enrollments).WithOne(p => p.Student);
            // x.Navigation(s => s.Enrollments).AutoInclude(); // With AutoInclude we don't really need our own repository anymore.
        });
        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable("Course").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("CourseID");
            x.Property(p => p.Name);
        });
        modelBuilder.Entity<Enrollment>(x =>
        {
            x.ToTable("Enrollment").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("EnrollmentID");
            x.HasOne(p => p.Student).WithMany(p => p.Enrollments);
            x.HasOne(p => p.Course).WithMany();
            // x.Navigation(e => e.Course).AutoInclude();
            x.Property(p => p.Grade);
        });
        modelBuilder.Entity<Sports>(x =>
        {
            x.ToTable("Sports").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("SportsID");
            x.Property(p => p.Name);
        });
        modelBuilder.Entity<SportsEnrollment>(x =>
        {
            x.ToTable("SportsEnrollment").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("SportsEnrollmentID");
            x.HasOne(p => p.Student).WithMany(p => p.SportsEnrollments);
            x.Property(p => p.SportsId);
            x.Property(p => p.Grade);
        });
    }
}
