using App.Application.Interfaces;
using App.Infrastructure.Services;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Kaxut_new;

public partial class App : Application
{
    public static ServiceProvider? Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=Kaxut.db"));

        services.AddScoped<IQuizService, QuizService>();

        Services = services.BuildServiceProvider();

        base.OnStartup(e);
    }
}
