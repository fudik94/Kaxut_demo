using App.Application.Interfaces;
using App.Infrastructure.Services;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace Kaxut_new;

public partial class App : Application
{
    public static ServiceProvider? Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder.AddDebug();    
            builder.AddConsole();  
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=Kaxut.db"));

        services.AddScoped<IQuizService, QuizService>();

        Services = services.BuildServiceProvider();

        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            DbInitializer.EnsureSystemQuizzesAsync(db)
                .GetAwaiter()
                .GetResult();
        }

        base.OnStartup(e);
    }
}
