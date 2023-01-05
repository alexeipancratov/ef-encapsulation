using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        
        // builder.Services.AddDbContext<SchoolContext>(
        //     options => options
        //         .UseSqlServer(builder.Configuration["ConnectionString"])
        //         .UseLoggerFactory(CreateLoggerFactory())
        //         .EnableSensitiveDataLogging()
        //     );

        builder.Services.AddScoped(_ =>
            new SchoolContext(builder.Configuration["ConnectionString"], useConsoleLogger: true));
        
        builder.Services.AddTransient<StudentRepository>();

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline

        app.MapControllers();

        app.Run();
    }
}
