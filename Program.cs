using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Models;

namespace Test;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ⚙️ Връзка към базата от appsettings.json
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // 🗄️ Регистрация на контекста само веднъж!
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // 🔐 Identity конфигурация
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // 🧩 Инициализация на категории при първоначално стартиране
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate(); // добавено за сигурност, че миграциите се прилагат

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Супи" },
                    new Category { Name = "Оризови ястия" },
                    new Category { Name = "Напитки" }
                );
                context.SaveChanges();
            }
        }

        // 🔧 Pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
