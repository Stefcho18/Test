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


        //using (var scope = app.Services.CreateScope())
        //{
        //    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //    context.Database.Migrate();

        //    var existingCategories = context.Categories.Select(c => c.Name).ToList();
        //    var categoriesToAdd = new List<CategoryViewModel>
        //    {
        //        new CategoryViewModel { Name = "Супa" },
        //        new CategoryViewModel { Name = "Оризови ястия" },
        //        new CategoryViewModel { Name = "Напитки" },
        //        new CategoryViewModel { Name = "Спагети"},
        //        new CategoryViewModel { Name = "Морски дарове" },
        //        new CategoryViewModel { Name = "Предястия" },
        //        new CategoryViewModel { Name = "Основни ястия" },
        //        new CategoryViewModel { Name = "Десерти" },
        //        new CategoryViewModel { Name = "Вегетариански ястия" },
        //        new CategoryViewModel { Name = "Пилешки ястия" },
        //        new CategoryViewModel { Name = "Свински ястия" },
        //        new CategoryViewModel { Name = "Телешки ястия" },
        //        new CategoryViewModel { Name = "Салати" },
        //        new CategoryViewModel { Name = "Суши" },
        //        new CategoryViewModel { Name = "Специални ястия" }
        //    };

        //    var newCategories = categoriesToAdd.Where(c => !existingCategories.Contains(c.Name)).ToList();
        //    if (newCategories.Any())
        //    {
        //        context.Categories.AddRange(newCategories);
        //        context.SaveChanges();
        //    }
        //}


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
