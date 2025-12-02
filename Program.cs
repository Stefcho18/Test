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

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

     
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

     
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate(); 

            var existingCategories = context.Categories.Select(c => c.Name).ToList();
            var categoriesToAdd = new List<Category>
            {
                new Category { Name = "Супa" },
                new Category { Name = "Оризови ястия" },
                new Category { Name = "Напитки" },
                new Category { Name = "Спагети"},
                new Category { Name = "Морски дарове" },
                new Category { Name = "Предястия" },
                new Category { Name = "Основни ястия" },
                new Category { Name = "Десерти" },
                new Category { Name = "Вегетариански ястия" },
                new Category { Name = "Пилешки ястия" },
                new Category { Name = "Свински ястия" },
                new Category { Name = "Телешки ястия" },
                new Category { Name = "Салати" },
                new Category { Name = "Суши" },
                new Category { Name = "Специални ястия" }
            };

            var newCategories = categoriesToAdd.Where(c => !existingCategories.Contains(c.Name)).ToList();
            if (newCategories.Any())
            {
                context.Categories.AddRange(newCategories);
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
            pattern: "{controller=MainPage}/{action=MainPage}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
